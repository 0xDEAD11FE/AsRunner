using System.Diagnostics;
using System.Net.Http;

namespace AsRunner;

public class AsRunnerContext : ApplicationContext
{
    private readonly MainForm _mainForm;
    private readonly AppLauncher _appLauncher;
    private readonly MenuBuilder _menuBuilder;

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
                    _mainForm.ShowUpdateBalloon(info.Version.ToString(), () => OnUpdateClicked(info));
            }, null));
    }

    private async void OnUpdateClicked(UpdateInfo info)
    {
        // Нет .exe в ассетах — просто открываем страницу релиза.
        if (string.IsNullOrEmpty(info.InstallerUrl))
        {
            if (!string.IsNullOrEmpty(info.ReleaseUrl))
                Process.Start(new ProcessStartInfo(info.ReleaseUrl) { UseShellExecute = true });
            return;
        }

        var confirm = MessageBox.Show(
            $"Обновить AsRunner до версии {info.Version}?\nПриложение закроется и переустановится.",
            "Обновление AsRunner", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
            return;

        try
        {
            var temp = Path.Combine(Path.GetTempPath(), $"AsRunnerSetup-{info.Version}.exe");
            using (var http = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
            {
                http.DefaultRequestHeaders.UserAgent.ParseAdd("AsRunner-UpdateCheck");
                var bytes = await http.GetByteArrayAsync(info.InstallerUrl);
                await File.WriteAllBytesAsync(temp, bytes);
            }

            // Запускаем установщик (ShellExecute → UAC). Он сам закроет это приложение и обновит.
            Process.Start(new ProcessStartInfo(temp) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось загрузить обновление:\n{ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
            _updateCheckTimer?.Dispose();

            // Disposing формы освобождает её components-контейнер,
            // в т.ч. NotifyIcon — иначе иконка остаётся в трее до наведения мыши.
            _mainForm?.Dispose();

            // Освобождаем GDI-ресурсы иконок пунктов меню.
            _menuBuilder?.Dispose();
        }

        base.Dispose(disposing);
    }
}
