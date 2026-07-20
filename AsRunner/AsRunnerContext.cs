namespace AsRunner;

public class AsRunnerContext : ApplicationContext
{
    private readonly MainForm _mainForm;
    private readonly AppLauncher _appLauncher;
    private readonly MenuBuilder _menuBuilder;
    private readonly HotkeyManager _hotkeys;

    // Одноразовый таймер: проверку обновлений запускаем уже из UI-потока
    // (с установленным WinForms SynchronizationContext) через пару секунд после старта.
    private readonly System.Windows.Forms.Timer _updateCheckTimer;

    public AsRunnerContext() : base()
    {
        _mainForm = new MainForm();
        _mainForm.ManageRequested += OnManageRequested;

        var config = ConfigReader.Reader.ReadConfig();

        _appLauncher = new AppLauncher();
        _menuBuilder = new MenuBuilder(_appLauncher);

        _menuBuilder.BuildMenu(config, _mainForm.TrayMenuStrip);

        _hotkeys = new HotkeyManager();
        _hotkeys.Activated += app => _appLauncher.Execute(app);
        _hotkeys.Register(config);

        _updateCheckTimer = new System.Windows.Forms.Timer { Interval = 3000 };
        _updateCheckTimer.Tick += OnUpdateCheckTick;
        _updateCheckTimer.Start();
    }

    private void OnUpdateCheckTick(object? sender, EventArgs e)
    {
        _updateCheckTimer.Stop();

        // Мы в UI-потоке — контекст WinForms установлен; через него вернём результат на UI.
        var ui = SynchronizationContext.Current;
        _ = UpdateChecker.CheckAsync(info =>
            ui?.Post(_ =>
            {
                if (!_mainForm.IsDisposed)
                    _mainForm.ShowUpdateBalloon(info.DisplayVersion, () => OnUpdateClicked(info));
            }, null),
            SettingsManager.IncludePrereleases);
    }

    private void OnUpdateClicked(UpdateInfo info) => _ = UpdateInstaller.PromptAndInstallAsync(info, null);

    private void OnManageRequested(object? sender, EventArgs e)
    {
        // Пока открыто окно управления — снимаем свои хоткеи, чтобы проба
        // «свободна/занята» в редакторе не видела их как занятые нами же.
        _hotkeys.UnregisterAll();

        // Если конфиг был сохранён — перечитываем и пересобираем меню.
        if (FormManager.ShowManagementForm())
            ReloadMenu();

        // Перерегистрируем хоткеи (конфиг мог измениться).
        try
        {
            _hotkeys.Register(ConfigReader.Reader.ReadConfig());
        }
        catch
        {
            // битый конфиг — просто останемся без хоткеев до следующего успешного чтения
        }
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
            _updateCheckTimer?.Dispose();

            // Disposing формы освобождает её components-контейнер,
            // в т.ч. NotifyIcon — иначе иконка остаётся в трее до наведения мыши.
            _mainForm?.Dispose();

            // Освобождаем GDI-ресурсы иконок пунктов меню.
            _menuBuilder?.Dispose();

            // Снимаем глобальные хоткеи и уничтожаем окно-приёмник.
            _hotkeys?.Dispose();
        }

        base.Dispose(disposing);
    }
}
