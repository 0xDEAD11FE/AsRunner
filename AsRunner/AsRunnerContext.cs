namespace AsRunner;

public class AsRunnerContext : ApplicationContext
{
    private readonly MainForm _mainForm;
    private readonly AppLauncher _appLauncher;
    private readonly MenuBuilder _menuBuilder;

    public AsRunnerContext() : base()
    {
        _mainForm = new MainForm();
        var config = ConfigReader.Reader.ReadConfig();

        _appLauncher = new AppLauncher();
        _menuBuilder = new MenuBuilder(_appLauncher);

        _menuBuilder.BuildMenu(config, _mainForm.TrayMenuStrip);
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
