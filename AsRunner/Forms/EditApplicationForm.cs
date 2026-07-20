using System.Drawing;
using ConfigReader.Models;
using WinApiWrapper;

namespace AsRunner;

public partial class EditApplicationForm : Form
{
    public string ResultGroup { get; private set; } = string.Empty;
    public ApplicationConfig? Result { get; private set; }

    /// <summary>Назначенная комбинация (Keys.None — не задана).</summary>
    private Keys _hotkeyKeys = Keys.None;

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
            SetHotkey(Hotkey.TryParse(existing.Hotkey, out var hk) ? hk : Keys.None);
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

        string? hotkey = _hotkeyKeys == Keys.None ? null : Hotkey.Format(_hotkeyKeys);

        ResultGroup = comboBoxGroup.Text.Trim();
        Result = new ApplicationConfig(file, userName, domain, alias, args, checkBoxShowInFolderMenu.Checked, hotkey);
        DialogResult = DialogResult.OK;
    }

    // ===== Горячая клавиша =====

    private void textBoxHotkey_KeyDown(object sender, KeyEventArgs e)
    {
        e.SuppressKeyPress = true;   // не печатаем символ в поле

        // Одиночный модификатор (Ctrl/Alt/Shift) — ждём основную клавишу.
        if (Hotkey.IsModifierOnly(e.KeyCode))
            return;

        // Backspace/Delete — сброс.
        if (e.KeyCode is Keys.Back or Keys.Delete)
        {
            SetHotkey(Keys.None);
            return;
        }

        // Назначаем только полную комбинацию (модификатор + клавиша);
        // «голую» клавишу без модификатора игнорируем.
        if (Hotkey.IsComplete(e.KeyData))
            SetHotkey(e.KeyData);
    }

    private void buttonClearHotkey_Click(object sender, EventArgs e) => SetHotkey(Keys.None);

    private void SetHotkey(Keys keyData)
    {
        _hotkeyKeys = keyData;

        if (_hotkeyKeys == Keys.None)
        {
            textBoxHotkey.Text = string.Empty;
            labelHotkeyStatus.Text = string.Empty;
            return;
        }

        textBoxHotkey.Text = Hotkey.Format(_hotkeyKeys);

        // Живая проверка занятости в системе (свои хоткеи на время окна сняты).
        bool free = HotKeys.IsFree(Hotkey.Modifiers(_hotkeyKeys), Hotkey.VirtualKey(_hotkeyKeys));
        labelHotkeyStatus.Text = free ? "свободна" : "занята";
        labelHotkeyStatus.ForeColor = free ? Color.Green : Color.Firebrick;
    }
}
