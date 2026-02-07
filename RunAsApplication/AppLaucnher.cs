using RunAsApplication.Models;
using WinApiWrapper;

namespace RunAsApplication;

internal class AppLaucnher
{
    public void Execute(ApplicationConfig entry)
    {
        string credKey = $"Tray:{entry.Domain}\\{entry.UserName}";
        string? password = CredentialManager.GetPassword(credKey);

        if (password is not { Length: > 0 })
        {
            // Если пароля нет, вызываем UI-форму ввода (пароль возвращается строкой)
            password = PromptForPassword(entry.UserName);
            if (password != null)
                CredentialManager.SavePassword(credKey, entry.UserName, password);
            else
                return;
        }

        try
        {
            WinApiLauncher.LaunchNetOnly(entry.FilePath, entry.Domain, entry.UserName, password);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while launching {entry.FilePath}. Exception: {ex}");
        }
    }

    private string? PromptForPassword(string user)
    {
        using (var form = new PromptUserPasswordForm(user))
            return form.ShowDialog() == DialogResult.OK ? form.Password : null;
    }

}
