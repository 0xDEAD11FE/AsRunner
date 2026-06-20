namespace AsRunner;

internal static class FormManager
{
    private static PromptUserPasswordForm? _activePromptForm;
    private static ManagementForm? _activeManagementForm;

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

    /// <summary>Показывает окно управления. Возвращает true, если конфиг был сохранён.</summary>
    public static bool ShowManagementForm()
    {
        if (_activeManagementForm != null && !_activeManagementForm.IsDisposed)
        {
            _activeManagementForm.Activate();
            _activeManagementForm.BringToFront();
            return false;
        }

        _activeManagementForm = new ManagementForm();

        try
        {
            _activeManagementForm.ShowDialog();
            return _activeManagementForm.ConfigChanged;
        }
        finally
        {
            _activeManagementForm?.Dispose();
            _activeManagementForm = null;
        }
    }
}
