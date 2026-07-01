namespace AsRunner;

public partial class PromptUserPasswordForm : Form
{
    public string? Password { get; private set; }
    public string? Domain { get; private set; }
    public string? UserName { get; private set; }

    private readonly bool _isEditMode;
    private readonly string? _existingPassword;

    public PromptUserPasswordForm(string? domain, string? user, string? existingPassword = null)
    {
        InitializeComponent();
        textBoxDomain.Text = domain ?? string.Empty;
        textBoxUserName.Text = user ?? string.Empty;

        _isEditMode = existingPassword != null;
        _existingPassword = existingPassword;

        if (_isEditMode)
        {
            this.Text = "Редактирование учетных данных";
            textBoxPassword.PlaceholderText = "Оставьте пустым, чтобы не менять";
        }
    }

    // Пароль виден только пока кнопка-глаз удерживается (удобно при шаринге экрана).
    // Кнопка захватывает мышь на MouseDown, поэтому MouseUp сработает даже при отпускании вне неё.
    private void buttonShowPassword_MouseDown(object sender, MouseEventArgs e)
    {
        textBoxPassword.UseSystemPasswordChar = false;
    }

    private void buttonShowPassword_MouseUp(object sender, MouseEventArgs e)
    {
        textBoxPassword.UseSystemPasswordChar = true;
    }

    private void PromptUserPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (DialogResult == DialogResult.OK)
        {
            this.Domain = textBoxDomain.Text;
            this.UserName = textBoxUserName.Text;

            // В режиме редактирования, если пароль пустой, используем существующий
            if (_isEditMode && string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                this.Password = _existingPassword;
            }
            else
            {
                this.Password = textBoxPassword.Text;
            }
        }
    }
}
