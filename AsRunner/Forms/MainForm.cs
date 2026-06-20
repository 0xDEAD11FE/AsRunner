namespace AsRunner;

public partial class MainForm : Form
{
    public ContextMenuStrip TrayMenuStrip => trayMenuStrip;

    public MainForm()
    {
        InitializeComponent();
    }

    private void trayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        FormManager.ShowManageCredentialsForm();
    }
}
