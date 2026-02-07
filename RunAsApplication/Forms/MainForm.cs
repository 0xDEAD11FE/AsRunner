namespace RunAsApplication
{
    public partial class MainForm : Form
    {
        public ContextMenuStrip TrayMenuStrip => trayMenuStrip;

        public MainForm()
        {
            InitializeComponent();
        }

        private void trayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            using (var form = new ManageCredentialsForm())
            {
                form.ShowDialog();
            }
        }
    }
}
