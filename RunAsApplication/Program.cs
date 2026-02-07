namespace RunAsApplication
{
    internal static class Program
    {
        private static Mutex? _mutex = null;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string mutexName = "RunAsApplication_SingleInstance_Mutex_24833BA9-80A9-4B40-8B90-5D3E40F4E957";
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

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.Run(new MyApplicationContext());

            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }
}