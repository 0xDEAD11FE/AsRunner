namespace AsRunner;

internal static class Program
{
    private static Mutex? _mutex = null;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
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
}
