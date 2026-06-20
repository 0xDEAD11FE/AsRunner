using WinApiWrapper.Models;

namespace WinApiWrapper;

/// <summary>
/// Извлечение иконок файлов через Shell API.
/// Возвращает нативный хэндл иконки (HICON), чтобы не тянуть в эту сборку
/// зависимость от System.Drawing — конвертацию в Bitmap делает потребитель,
/// после чего обязан освободить хэндл через <see cref="DestroyIcon"/>.
/// </summary>
public static class IconExtractor
{
    /// <summary>
    /// Возвращает HICON малой (16x16) иконки файла, либо IntPtr.Zero,
    /// если файл не найден или иконку получить не удалось.
    /// Вызывающий код обязан освободить ненулевой результат через DestroyIcon.
    /// </summary>
    public static IntPtr ExtractSmallIcon(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return IntPtr.Zero;

        var info = new SHFILEINFO();
        IntPtr result = Methods.SHGetFileInfo(
            path,
            0,
            ref info,
            (uint)System.Runtime.InteropServices.Marshal.SizeOf<SHFILEINFO>(),
            Constants.ShellFileInfo.Icon | Constants.ShellFileInfo.SmallIcon);

        return result == IntPtr.Zero ? IntPtr.Zero : info.hIcon;
    }

    /// <summary>
    /// Освобождает хэндл иконки, полученный из <see cref="ExtractSmallIcon"/>.
    /// </summary>
    public static void DestroyIcon(IntPtr hIcon)
    {
        if (hIcon != IntPtr.Zero)
            Methods.DestroyIcon(hIcon);
    }
}
