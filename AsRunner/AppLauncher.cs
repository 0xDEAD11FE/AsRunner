using ConfigReader.Models;
using WinApiWrapper;

namespace AsRunner;

internal class AppLauncher
{
    public void Execute(ApplicationConfig entry)
    {
        string? domain = entry.Domain;
        string? userName = entry.UserName;
        string? password = null;

        // Если есть домен и имя пользователя, пытаемся получить пароль
        if (!string.IsNullOrWhiteSpace(domain) && !string.IsNullOrWhiteSpace(userName))
        {
            password = CredentialManager.GetPassword(CredentialManager.TrayTargetName(domain, userName));
        }

        // Если пароля нет или не заполнены domain/username, показываем форму
        if (string.IsNullOrWhiteSpace(domain) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            var result = PromptForCredentials(domain, userName);
            if (result == null)
                return;

            (domain, userName, password) = result.Value;

            // Сохраняем пароль
            CredentialManager.SavePassword(CredentialManager.TrayTargetName(domain, userName), userName, password);
        }

        try
        {
            ProcessLauncher.LaunchNetOnly(entry.FilePath, domain, userName, password);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while launching {entry.FilePath}. Exception: {ex}");
        }
        finally
        {
            // Сокращаем время жизни пароля в открытом виде.
            SensitiveData.Clear(password);
            password = null;
        }
    }

    private (string domain, string userName, string password)? PromptForCredentials(string? domain, string? user)
    {
        return FormManager.ShowPromptForm(domain, user);
    }

}
