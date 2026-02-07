using RunAsApplication.Models;
using WinApiWrapper;

namespace RunAsApplication;

internal class AppLaucnher
{
    public void Execute(ApplicationConfig entry)
    {
        string? domain = entry.Domain;
        string? userName = entry.UserName;
        string? password = null;

        // Если есть домен и имя пользователя, пытаемся получить пароль
        if (!string.IsNullOrWhiteSpace(domain) && !string.IsNullOrWhiteSpace(userName))
        {
            string credKey = $"Tray:{domain}\\{userName}";
            password = CredentialManager.GetPassword(credKey);
        }

        // Если пароля нет или не заполнены domain/username, показываем форму
        if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            var result = PromptForCredentials(domain, userName);
            if (result == null)
                return;

            (domain, userName, password) = result.Value;

            // Сохраняем пароль
            string credKey = $"Tray:{domain}\\{userName}";
            CredentialManager.SavePassword(credKey, userName, password);
        }

        try
        {
            WinApiLauncher.LaunchNetOnly(entry.FilePath, domain, userName, password);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while launching {entry.FilePath}. Exception: {ex}");
        }
    }

    private (string domain, string userName, string password)? PromptForCredentials(string? domain, string? user)
    {
        using (var form = new PromptUserPasswordForm(domain, user))
        {
            if (form.ShowDialog() == DialogResult.OK &&
                !string.IsNullOrWhiteSpace(form.Domain) &&
                !string.IsNullOrWhiteSpace(form.UserName) &&
                !string.IsNullOrWhiteSpace(form.Password))
            {
                return (form.Domain, form.UserName, form.Password);
            }

            return null;
        }
    }

}
