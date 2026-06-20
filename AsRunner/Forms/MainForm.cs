namespace AsRunner;

public partial class MainForm : Form
{
    public ContextMenuStrip TrayMenuStrip => trayMenuStrip;

    /// <summary>Запрос на открытие окна управления (двойной клик по иконке трея).</summary>
    public event EventHandler? ManageRequested;

    public MainForm()
    {
        InitializeComponent();
    }

    private void trayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        ManageRequested?.Invoke(this, EventArgs.Empty);
    }
}
