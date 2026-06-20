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
