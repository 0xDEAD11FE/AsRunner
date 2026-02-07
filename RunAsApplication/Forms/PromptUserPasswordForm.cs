namespace RunAsApplication;

public partial class PromptUserPasswordForm : Form
{
    public string? Password { get; private set; }

    public PromptUserPasswordForm(string user)
    {
        InitializeComponent();
        textBoxUserName.Text = user;
    }

    private void PromptUserPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (DialogResult == DialogResult.OK)
            this.Password = textBoxPassword.Text;
    }
}
