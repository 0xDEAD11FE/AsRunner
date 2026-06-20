namespace AsRunner;

public class MyApplicationContext : ApplicationContext
{
    private readonly MainForm _mainForm;
    private readonly AppLauncher _appLauncher;
    private readonly MenuBuilder _menuBuilder;

    public MyApplicationContext() : base()
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
        }

        base.Dispose(disposing);
    }
}
