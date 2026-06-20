namespace WinApiWrapper;

/// <summary>
/// Утилиты для работы с чувствительными данными в памяти.
/// </summary>
public static class SensitiveData
{
    /// <summary>
    /// Затирает содержимое строки нулями прямо в памяти.
    /// ВАЖНО: вызывать только для уникальных строк (полученных из TextBox,
    /// Encoding.GetString и т.п.). Нельзя применять к строковым литералам/
    /// интернированным строкам — это повредит общий экземпляр.
    /// Полноценной защиты managed-строк это не даёт (GC мог сделать копии),
    /// но сокращает время жизни пароля в открытом виде.
    /// </summary>
    public static unsafe void Clear(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        fixed (char* p = value)
        {
            for (int i = 0; i < value.Length; i++)
                p[i] = '\0';
        }
    }
}
