using ConfigReader.Models;
using Microsoft.Win32;
using WinApiWrapper;

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
        Shell.NotifyAssociationsChanged();
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

        using var rootShell = root.CreateSubKey("shell");

        // Как в трее: одна группа → плоский список, несколько → подменю на группу.
        if (config.Count == 1)
        {
            WriteApps(rootShell, config.Values.First(), exe);
        }
        else
        {
            int g = 0;
            foreach (var (groupName, apps) in config)
            {
                using var group = rootShell.CreateSubKey($"grp{g:D3}");
                group.SetValue("MUIVerb", groupName);
                group.SetValue("SubCommands", "");   // вложенное подменю группы
                using var groupShell = group.CreateSubKey("shell");
                WriteApps(groupShell, apps, exe);
                g++;
            }
        }

        Shell.NotifyAssociationsChanged();
    }

    private static void WriteApps(RegistryKey shell, IEnumerable<ApplicationConfig> apps, string exe)
    {
        int i = 0;
        foreach (var app in apps)
        {
            string label = !string.IsNullOrWhiteSpace(app.Alias)
                ? app.Alias
                : Path.GetFileNameWithoutExtension(app.FilePath);

            using var item = shell.CreateSubKey($"app{i:D3}");
            item.SetValue("MUIVerb", label);
            item.SetValue("Icon", $"\"{app.FilePath}\",0");

            using var command = item.CreateSubKey("command");
            // %V без кавычек и последним аргументом: для корня диска %V = "C:\", а
            // "%V" дало бы \" (экранированную кавычку) и битый путь. Пробелы в пути
            // собираем обратно в приложении (Program.TryGetFolderRun).
            command.SetValue("", $"\"{exe}\" --folder-run --exe \"{app.FilePath}\" --path %V");

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
