using ConfigReader.Models;
using Microsoft.Win32;

namespace AsRunner;

/// <summary>
/// Интеграция в контекстное меню «пустого места внутри папки»
/// (HKCU\...\Directory\Background\shell). Каскадное подменю «AsRunner» со
/// списком приложений; клик запускает приложение с рабочим каталогом = этой папкой.
/// Per-user, без прав администратора.
/// </summary>
internal static class FolderContextMenu
{
    private const string ShellKeyPath = @"Software\Classes\Directory\Background\shell\AsRunner";

    public static bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(ShellKeyPath);
        return key is not null;
    }

    public static void Disable()
    {
        Registry.CurrentUser.DeleteSubKeyTree(ShellKeyPath, throwOnMissingSubKey: false);
    }

    /// <summary>Пересоздаёт подменю из текущего конфига.</summary>
    public static void Enable(RootConfig config)
    {
        Disable();

        var exe = Environment.ProcessPath;
        if (string.IsNullOrEmpty(exe))
            return;

        using var root = Registry.CurrentUser.CreateSubKey(ShellKeyPath);
        root.SetValue("MUIVerb", "AsRunner");
        root.SetValue("SubCommands", "");            // пустое → строить подменю из дочернего shell
        root.SetValue("Icon", $"\"{exe}\",0");

        using var shell = root.CreateSubKey("shell");

        int i = 0;
        foreach (var app in config.Values.SelectMany(list => list))
        {
            string label = !string.IsNullOrWhiteSpace(app.Alias)
                ? app.Alias
                : Path.GetFileNameWithoutExtension(app.FilePath);

            using var item = shell.CreateSubKey($"app{i:D3}");
            item.SetValue("MUIVerb", label);
            item.SetValue("Icon", $"\"{app.FilePath}\",0");

            using var command = item.CreateSubKey("command");
            command.SetValue("", $"\"{exe}\" --folder-run --exe \"{app.FilePath}\" --path \"%V\"");

            i++;
        }
    }

    /// <summary>Если интеграция включена — переписать подменю под изменившийся конфиг.</summary>
    public static void Sync(RootConfig config)
    {
        if (IsEnabled())
            Enable(config);
    }
}
