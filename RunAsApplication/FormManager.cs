namespace RunAsApplication;

internal static class FormManager
{
    private static PromptUserPasswordForm? _activePromptForm;
    private static ManageCredentialsForm? _activeManageCredentialsForm;

    public static (string domain, string userName, string password)? ShowPromptForm(string? domain, string? userName, string? existingPassword = null)
    {
        if (_activePromptForm != null && !_activePromptForm.IsDisposed)
        {
            _activePromptForm.Activate();
            _activePromptForm.BringToFront();
            return null;
        }

        _activePromptForm = new PromptUserPasswordForm(domain, userName, existingPassword);
        
        try
        {
            if (_activePromptForm.ShowDialog() == DialogResult.OK &&
                !string.IsNullOrWhiteSpace(_activePromptForm.Domain) &&
                !string.IsNullOrWhiteSpace(_activePromptForm.UserName) &&
                !string.IsNullOrWhiteSpace(_activePromptForm.Password))
            {
                return (_activePromptForm.Domain, _activePromptForm.UserName, _activePromptForm.Password);
            }

            return null;
        }
        finally
        {
            _activePromptForm?.Dispose();
            _activePromptForm = null;
        }
    }

    public static void ShowManageCredentialsForm()
    {
        if (_activeManageCredentialsForm != null && !_activeManageCredentialsForm.IsDisposed)
        {
            _activeManageCredentialsForm.Activate();
            _activeManageCredentialsForm.BringToFront();
            return;
        }

        _activeManageCredentialsForm = new ManageCredentialsForm();
        
        try
        {
            _activeManageCredentialsForm.ShowDialog();
        }
        finally
        {
            _activeManageCredentialsForm?.Dispose();
            _activeManageCredentialsForm = null;
        }
    }
}
