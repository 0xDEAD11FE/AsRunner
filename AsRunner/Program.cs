using ConfigReader.Models;

namespace AsRunner;

internal static class Program
{
    private static Mutex? _mutex = null;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // Режим действия: запуск конкретного приложения в папке из контекстного меню.
        // Не поднимаем трей и не завязываемся на single-instance — сделали дело и вышли.
        if (TryGetFolderRun(args, out var exePath, out var folder))
        {
            RunFolderAction(exePath, folder);
            return;
        }

        const string mutexName = "AsRunner_SingleInstance_Mutex_24833BA9-80A9-4B40-8B90-5D3E40F4E957";
        _mutex = new Mutex(true, mutexName, out bool createdNew);

        if (!createdNew)
        {
            MessageBox.Show(
                "Приложение уже запущено.",
                "Предупреждение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            AsRunnerContext context;
            try
            {
                context = new AsRunnerContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Не удалось запустить приложение.\n\n{ex.Message}",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(context);
        }
        finally
        {
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }

    /// <summary>Разбирает аргументы вида: --folder-run --exe "&lt;path&gt;" --path &lt;folder&gt; (путь — последним, без кавычек).</summary>
    private static bool TryGetFolderRun(string[] args, out string exePath, out string folder)
    {
        exePath = string.Empty;
        folder = string.Empty;

        if (args.Length == 0 || !args.Contains("--folder-run"))
            return false;

        int exeIdx = Array.IndexOf(args, "--exe");
        if (exeIdx >= 0 && exeIdx + 1 < args.Length)
            exePath = args[exeIdx + 1];

        // --path передан последним и без кавычек; путь с пробелами склеиваем обратно.
        int pathIdx = Array.IndexOf(args, "--path");
        if (pathIdx >= 0 && pathIdx + 1 < args.Length)
            folder = string.Join(' ', args[(pathIdx + 1)..]);

        // Корень диска: "C:" → "C:\" (иначе рабочий каталог невалиден).
        if (folder.Length == 2 && folder[1] == ':')
            folder += "\\";

        return !string.IsNullOrEmpty(exePath) && !string.IsNullOrEmpty(folder);
    }

    private static void RunFolderAction(string exePath, string folder)
    {
        // Нужно для темы/шрифтов, если появится форма запроса пароля.
        ApplicationConfiguration.Initialize();

        RootConfig config;
        try
        {
            config = ConfigReader.Reader.ReadConfig();
        }
        catch
        {
            return;
        }

        var entry = config.Values
            .SelectMany(list => list)
            .FirstOrDefault(a => string.Equals(a.FilePath, exePath, StringComparison.OrdinalIgnoreCase));

        if (entry is null)
            return;

        new AppLauncher().Execute(entry, folder);
    }
}
