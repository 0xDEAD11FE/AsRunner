namespace AsRunner;

public partial class MainForm : Form
{
    public ContextMenuStrip TrayMenuStrip => trayMenuStrip;

    /// <summary>Запрос на открытие окна управления (двойной клик по иконке трея).</summary>
    public event EventHandler? ManageRequested;

    private Action? _balloonClick;

    public MainForm()
    {
        InitializeComponent();
    }

    /// <summary>Показать нативный тост о доступном обновлении; onClick — по клику по тосту.</summary>
    public void ShowUpdateBalloon(string version, Action onClick)
    {
        _balloonClick = onClick;
        trayIcon.BalloonTipTitle = "Доступно обновление AsRunner";
        trayIcon.BalloonTipText = $"Версия {version} доступна. Нажмите, чтобы обновить.";
        trayIcon.ShowBalloonTip(10000);
    }

    private void trayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        ManageRequested?.Invoke(this, EventArgs.Empty);
    }

    private void trayIcon_BalloonTipClicked(object? sender, EventArgs e)
    {
        _balloonClick?.Invoke();
    }
}
