namespace WinApiWrapper;

/// <summary>
/// Глобальные горячие клавиши через RegisterHotKey/UnregisterHotKey (user32).
/// Система сама доставляет только зарегистрированную комбинацию — это не
/// клавиатурный хук, остального ввода мы не касаемся.
/// </summary>
public static class HotKeys
{
    // Модификаторы fsModifiers для RegisterHotKey.
    public const uint ModAlt = 0x0001;      // MOD_ALT
    public const uint ModControl = 0x0002;  // MOD_CONTROL
    public const uint ModShift = 0x0004;    // MOD_SHIFT
    public const uint ModWin = 0x0008;      // MOD_WIN
    public const uint ModNoRepeat = 0x4000; // MOD_NOREPEAT — не срабатывать на автоповтор

    public const int WmHotKey = 0x0312;     // WM_HOTKEY

    /// <summary>Регистрирует комбинацию (id уникален в пределах hWnd). false — уже занято/зарезервировано.</summary>
    public static bool Register(IntPtr hWnd, int id, uint modifiers, uint virtualKey)
        => Methods.RegisterHotKey(hWnd, id, modifiers | ModNoRepeat, virtualKey);

    public static void Unregister(IntPtr hWnd, int id)
        => Methods.UnregisterHotKey(hWnd, id);

    /// <summary>
    /// Проба: свободна ли комбинация в системе. Пытаемся зарегистрировать на временный
    /// id и тут же снимаем. Не сообщает, кто именно занял, — только занято/свободно.
    /// </summary>
    public static bool IsFree(uint modifiers, uint virtualKey)
    {
        const int probeId = 0x3FFF; // временный id, не пересекается с рабочими (1..N)
        if (!Methods.RegisterHotKey(IntPtr.Zero, probeId, modifiers | ModNoRepeat, virtualKey))
            return false;
        Methods.UnregisterHotKey(IntPtr.Zero, probeId);
        return true;
    }
}
