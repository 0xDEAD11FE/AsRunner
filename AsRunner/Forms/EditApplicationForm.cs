using ConfigReader.Models;
using WinApiWrapper;

namespace AsRunner;

public partial class EditApplicationForm : Form
{
    public string ResultGroup { get; private set; } = string.Empty;
    public ApplicationConfig? Result { get; private set; }

    public EditApplicationForm(IEnumerable<string> existingGroups, string? group = null, ApplicationConfig? existing = null)
    {
        InitializeComponent();

        foreach (var g in existingGroups.Where(g => !string.IsNullOrWhiteSpace(g)).Distinct())
            comboBoxGroup.Items.Add(g);

        LoadCredentials(existing?.Domain, existing?.UserName);

        if (existing != null)
        {
            Text = "Изменить приложение";
            comboBoxGroup.Text = group ?? string.Empty;
            textBoxFile.Text = existing.FilePath;
            textBoxAlias.Text = existing.Alias ?? string.Empty;
            textBoxArgs.Text = existing.Arguments ?? string.Empty;
            checkBoxShowInFolderMenu.Checked = existing.ShowInFolderMenu;
        }
        else
        {
            Text = "Добавить приложение";
            if (group != null)
                comboBoxGroup.Text = group;
        }
    }

    /// <summary>Пункт выпадающего списка учётных данных.</summary>
    private sealed class CredOption
    {
        public string? Domain { get; init; }
        public string? UserName { get; init; }
        public bool IsNone => Domain is null && UserName is null;

        public override string ToString() =>
            IsNone ? "(спрашивать при запуске)" : $"{Domain}\\{UserName}";
    }

    private void LoadCredentials(string? selectDomain, string? selectUser)
    {
        comboBoxAccount.Items.Clear();
        comboBoxAccount.Items.Add(new CredOption()); // «спрашивать при запуске»

        foreach (var (_, userName, domain) in CredentialManager.GetTrayCredentials())
            comboBoxAccount.Items.Add(new CredOption { Domain = domain, UserName = userName });

        if (!string.IsNullOrWhiteSpace(selectDomain) && !string.IsNullOrWhiteSpace(selectUser))
        {
            var match = comboBoxAccount.Items.Cast<CredOption>().FirstOrDefault(c =>
                string.Equals(c.Domain, selectDomain, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(c.UserName, selectUser, StringComparison.OrdinalIgnoreCase));

            // учётка записи может отсутствовать в Credential Manager (правили конфиг руками) —
            // показываем её отдельным пунктом, чтобы не потерять при сохранении
            if (match == null)
            {
                match = new CredOption { Domain = selectDomain, UserName = selectUser };
                comboBoxAccount.Items.Add(match);
            }

            comboBoxAccount.SelectedItem = match;
        }
        else
        {
            comboBoxAccount.SelectedIndex = 0;
        }
    }

    private void buttonBrowse_Click(object sender, EventArgs e)
    {
        try
        {
            var dir = Path.GetDirectoryName(textBoxFile.Text);
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                openFileDialog.InitialDirectory = dir;
        }
        catch
        {
            // некорректный путь в поле — игнорируем
        }

        if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            textBoxFile.Text = openFileDialog.FileName;
    }

    private void buttonAddCred_Click(object sender, EventArgs e)
    {
        var result = FormManager.ShowPromptForm(null, null);
        if (result == null)
            return;

        var (domain, userName, password) = result.Value;
        CredentialManager.SavePassword(CredentialManager.TrayTargetName(domain, userName), userName, password);
        SensitiveData.Clear(password);

        LoadCredentials(domain, userName);
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
        var file = textBoxFile.Text.Trim();
        if (string.IsNullOrWhiteSpace(file))
        {
            MessageBox.Show("Укажите путь к приложению.", "Проверка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cred = comboBoxAccount.SelectedItem as CredOption;
        string? domain = cred is { IsNone: false } ? cred.Domain : null;
        string? userName = cred is { IsNone: false } ? cred.UserName : null;
        string? alias = string.IsNullOrWhiteSpace(textBoxAlias.Text) ? null : textBoxAlias.Text.Trim();
        string? args = string.IsNullOrWhiteSpace(textBoxArgs.Text) ? null : textBoxArgs.Text.Trim();

        ResultGroup = comboBoxGroup.Text.Trim();
        Result = new ApplicationConfig(file, userName, domain, alias, args, checkBoxShowInFolderMenu.Checked);
        DialogResult = DialogResult.OK;
    }
}
