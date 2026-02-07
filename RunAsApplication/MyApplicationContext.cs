namespace RunAsApplication;

public class MyApplicationContext : ApplicationContext
{
    private AppLaucnher _appLaucnher;
    private MenuBuilder _menuBuilder;

    public MyApplicationContext() : base(new MainForm())
    {
        var config = ConfigReader.Reader.ReadConfig();

        _appLaucnher = new AppLaucnher();
        _menuBuilder = new MenuBuilder(_appLaucnher);

        _menuBuilder.BuildMenu(config, ((MainForm)MainForm!).TrayMenuStrip);
    }
}
