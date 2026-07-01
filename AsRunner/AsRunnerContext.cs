namespace AsRunner;

public class AsRunnerContext : ApplicationContext
{
    private readonly MainForm _mainForm;
    private readonly AppLauncher _appLauncher;
    private readonly MenuBuilder _menuBuilder;

    public AsRunnerContext() : base()
    {
        _mainForm = new MainForm();
        _mainForm.ManageRequested += OnManageRequested;

        // Привязываем время жизни приложения к главной форме: когда её окно
        // закрывается (в т.ч. по запросу Restart Manager при обновлении через
        // установщик), цикл сообщений завершается и процесс корректно выходит.
        MainForm = _mainForm;
        // Форсируем создание (скрытого, невидимого) окна формы, чтобы shutdown-
        // запрос RM/сессии дошёл именно до неё, а не только до окна NotifyIcon.
        _ = _mainForm.Handle;

        var config = ConfigReader.Reader.ReadConfig();

        _appLauncher = new AppLauncher();
        _menuBuilder = new MenuBuilder(_appLauncher);

        _menuBuilder.BuildMenu(config, _mainForm.TrayMenuStrip);
    }

    private void OnManageRequested(object? sender, EventArgs e)
    {
        // Если конфиг был сохранён — перечитываем и пересобираем меню.
        if (FormManager.ShowManagementForm())
            ReloadMenu();
    }

    private void ReloadMenu()
    {
        try
        {
            var config = ConfigReader.Reader.ReadConfig();
            _menuBuilder.BuildMenu(config, _mainForm.TrayMenuStrip);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Не удалось перечитать конфигурацию:\n\n{ex.Message}",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Disposing формы освобождает её components-контейнер,
            // в т.ч. NotifyIcon — иначе иконка остаётся в трее до наведения мыши.
            _mainForm?.Dispose();

            // Освобождаем GDI-ресурсы иконок пунктов меню.
            _menuBuilder?.Dispose();
        }

        base.Dispose(disposing);
    }
}
