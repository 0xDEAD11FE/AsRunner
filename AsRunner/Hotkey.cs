using WinApiWrapper;

namespace AsRunner;

/// <summary>
/// Разбор/формат горячей клавиши и конвертация в модификаторы+vk для RegisterHotKey.
/// Комбинация хранится как строка «Ctrl+Alt+K». Поддерживаются модификаторы
/// Ctrl/Alt/Shift (Win в пикере не ловим — его перехватывает система).
/// </summary>
internal static class Hotkey
{
    public static uint Modifiers(Keys keyData)
    {
        uint m = 0;
        if ((keyData & Keys.Control) == Keys.Control) m |= HotKeys.ModControl;
        if ((keyData & Keys.Alt) == Keys.Alt) m |= HotKeys.ModAlt;
        if ((keyData & Keys.Shift) == Keys.Shift) m |= HotKeys.ModShift;
        return m;
    }

    public static uint VirtualKey(Keys keyData) => (uint)(keyData & Keys.KeyCode);

    /// <summary>Комбинация пригодна: есть основная клавиша и хотя бы один модификатор.</summary>
    public static bool IsComplete(Keys keyData)
        => (keyData & Keys.KeyCode) != Keys.None && Modifiers(keyData) != 0;

    /// <summary>Основная клавиша — сама по себе модификатор (Ctrl/Alt/Shift), ещё не полная комбинация.</summary>
    public static bool IsModifierOnly(Keys keyCode) =>
        keyCode is Keys.ControlKey or Keys.Menu or Keys.ShiftKey
                or Keys.LControlKey or Keys.RControlKey
                or Keys.LMenu or Keys.RMenu
                or Keys.LShiftKey or Keys.RShiftKey or Keys.None;

    public static string Format(Keys keyData)
    {
        var parts = new List<string>();
        if ((keyData & Keys.Control) == Keys.Control) parts.Add("Ctrl");
        if ((keyData & Keys.Alt) == Keys.Alt) parts.Add("Alt");
        if ((keyData & Keys.Shift) == Keys.Shift) parts.Add("Shift");
        parts.Add(KeyName(keyData & Keys.KeyCode));
        return string.Join("+", parts);
    }

    public static bool TryParse(string? s, out Keys keyData)
    {
        keyData = Keys.None;
        if (string.IsNullOrWhiteSpace(s))
            return false;

        Keys result = Keys.None;
        foreach (var raw in s.Split('+', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            switch (raw.ToLowerInvariant())
            {
                case "ctrl":
                case "control":
                    result |= Keys.Control;
                    break;
                case "alt":
                    result |= Keys.Alt;
                    break;
                case "shift":
                    result |= Keys.Shift;
                    break;
                default:
                    if (!TryParseKey(raw, out var k))
                        return false;
                    result |= k & Keys.KeyCode;
                    break;
            }
        }

        keyData = result;
        return IsComplete(result);
    }

    // Цифры показываем как «1», а не «D1»; остальное — имя из перечисления Keys.
    private static string KeyName(Keys key) => key switch
    {
        >= Keys.D0 and <= Keys.D9 => ((char)('0' + (key - Keys.D0))).ToString(),
        _ => key.ToString(),
    };

    private static bool TryParseKey(string token, out Keys key)
    {
        // Обратно к KeyName: одиночная цифра «1» -> Keys.D1.
        if (token.Length == 1 && token[0] is >= '0' and <= '9')
        {
            key = Keys.D0 + (token[0] - '0');
            return true;
        }
        return Enum.TryParse(token, ignoreCase: true, out key);
    }
}
