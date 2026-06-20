using ConfigReader;
using ConfigReader.Models;
using WinApiWrapper;

namespace AsRunner;

public partial class ManagementForm : Form
{
    /// <summary>true, если конфигурация была сохранена (нужно пересобрать меню).</summary>
    public bool ConfigChanged { get; private set; }

    private const string DefaultGroup = "Приложения";

    /// <summary>Строка редактора: группа + конфиг приложения (группа — ключ словаря, не поле модели).</summary>
    private sealed class Entry
    {
        public string Group = DefaultGroup;
        public ApplicationConfig Cfg = null!;
    }

    private readonly List<Entry> _entries = new();

    // Снимок состояния на момент загрузки/сохранения. С ним сравниваем после
    // каждой правки, чтобы кнопка «Сохранить» отражала реальное отличие
    // (откатил изменения вручную → кнопка снова выключается).
    private List<(string Group, ApplicationConfig Cfg)> _baseline = new();

    /// <summary>Есть несохранённые изменения в списке приложений.</summary>
    private bool _hasChanges;

    public ManagementForm()
    {
        InitializeComponent();
        LoadApplications();
        LoadCredentials();
    }

    private List<(string Group, ApplicationConfig Cfg)> Snapshot() =>
        _entries.Select(e => (e.Group, e.Cfg)).ToList();

    private void RecomputeDirty()
    {
        _hasChanges = !Snapshot().SequenceEqual(_baseline);
        UpdateSaveButton();
    }

    private void UpdateSaveButton() => buttonSaveApps.Enabled = _hasChanges;

    // ===== Вкладка «Приложения» =====

    private IEnumerable<string> Groups =>
        _entries.Select(e => e.Group).Where(g => !string.IsNullOrWhiteSpace(g)).Distinct();

    private Entry? SelectedEntry =>
        listViewApps.SelectedItems.Count > 0 ? listViewApps.SelectedItems[0].Tag as Entry : null;

    private void LoadApplications()
    {
        _entries.Clear();
        try
        {
            foreach (var (group, apps) in Reader.ReadConfig())
                foreach (var app in apps)
                    _entries.Add(new Entry { Group = group, Cfg = app });
        }
        catch
        {
            // нет/битый конфиг — начинаем с пустого списка
        }

        RefreshAppList();

        // только что загрузили — это и есть базовое состояние
        _baseline = Snapshot();
        _hasChanges = false;
        UpdateSaveButton();
    }

    private void RefreshAppList()
    {
        listViewApps.BeginUpdate();
        listViewApps.Items.Clear();
        listViewApps.Groups.Clear();

        var groupLookup = new Dictionary<string, ListViewGroup>();

        foreach (var entry in _entries)
        {
            if (!groupLookup.TryGetValue(entry.Group, out var lvg))
            {
                lvg = new ListViewGroup(entry.Group);
                groupLookup[entry.Group] = lvg;
                listViewApps.Groups.Add(lvg);
            }

            string name = !string.IsNullOrWhiteSpace(entry.Cfg.Alias)
                ? entry.Cfg.Alias
                : Path.GetFileNameWithoutExtension(entry.Cfg.FilePath);

            string account = (!string.IsNullOrWhiteSpace(entry.Cfg.Domain) && !string.IsNullOrWhiteSpace(entry.Cfg.UserName))
                ? $"{entry.Cfg.Domain}\\{entry.Cfg.UserName}"
                : "—";

            listViewApps.Items.Add(new ListViewItem(new[] { name, entry.Cfg.FilePath, account }, lvg) { Tag = entry });
        }

        listViewApps.EndUpdate();
        UpdateAppButtons();
    }

    private void UpdateAppButtons()
    {
        bool has = listViewApps.SelectedItems.Count > 0;
        buttonEditApp.Enabled = has;
        buttonDeleteApp.Enabled = has;
    }

    private static string NormalizeGroup(string group) =>
        string.IsNullOrWhiteSpace(group) ? DefaultGroup : group.Trim();

    private void buttonAddApp_Click(object sender, EventArgs e)
    {
        using var dlg = new EditApplicationForm(Groups);
        if (dlg.ShowDialog(this) == DialogResult.OK && dlg.Result != null)
        {
            _entries.Add(new Entry { Group = NormalizeGroup(dlg.ResultGroup), Cfg = dlg.Result });
            RefreshAppList();
            RecomputeDirty();
        }
    }

    private void buttonEditApp_Click(object sender, EventArgs e)
    {
        var entry = SelectedEntry;
        if (entry == null)
            return;

        using var dlg = new EditApplicationForm(Groups, entry.Group, entry.Cfg);
        if (dlg.ShowDialog(this) != DialogResult.OK || dlg.Result == null)
            return;

        var newGroup = NormalizeGroup(dlg.ResultGroup);

        // record-равенство: если ничего не изменилось — не помечаем как «грязное»
        if (newGroup == entry.Group && dlg.Result == entry.Cfg)
            return;

        entry.Group = newGroup;
        entry.Cfg = dlg.Result;
        RefreshAppList();
        RecomputeDirty();
    }

    private void buttonDeleteApp_Click(object sender, EventArgs e)
    {
        var entry = SelectedEntry;
        if (entry == null)
            return;

        _entries.Remove(entry);
        RefreshAppList();
        RecomputeDirty();
    }

    private void buttonSaveApps_Click(object sender, EventArgs e)
    {
        var config = new RootConfig();
        foreach (var entry in _entries)
        {
            var key = NormalizeGroup(entry.Group);
            if (!config.TryGetValue(key, out var list))
            {
                list = new List<ApplicationConfig>();
                config[key] = list;
            }
            list.Add(entry.Cfg);
        }

        try
        {
            Reader.WriteConfig(config);
            ConfigChanged = true;

            // сохранённое состояние становится новым базовым;
            // обратная связь — кнопка «Сохранить» гаснет (изменений больше нет).
            _baseline = Snapshot();
            _hasChanges = false;
            UpdateSaveButton();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось сохранить конфигурацию: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void listViewApps_SelectedIndexChanged(object sender, EventArgs e) => UpdateAppButtons();

    private void listViewApps_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (SelectedEntry != null)
            buttonEditApp_Click(sender, e);
    }

    // ===== Вкладка «Учётные данные» =====

    private void LoadCredentials()
    {
        listViewCreds.Items.Clear();
        foreach (var (targetName, userName, domain) in CredentialManager.GetTrayCredentials())
            listViewCreds.Items.Add(new ListViewItem(new[] { domain, userName }) { Tag = targetName });

        buttonDeleteCred.Enabled = listViewCreds.SelectedItems.Count > 0;
    }

    private void listViewCreds_SelectedIndexChanged(object sender, EventArgs e) =>
        buttonDeleteCred.Enabled = listViewCreds.SelectedItems.Count > 0;

    private void buttonDeleteCred_Click(object sender, EventArgs e)
    {
        if (listViewCreds.SelectedItems.Count == 0)
            return;

        var selected = listViewCreds.SelectedItems[0];
        string targetName = selected.Tag as string ?? string.Empty;

        var result = MessageBox.Show(
            $"Удалить учётные данные для {selected.SubItems[0].Text}\\{selected.SubItems[1].Text}?",
            "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (result != DialogResult.Yes)
            return;

        if (CredentialManager.DeleteCredential(targetName))
            LoadCredentials();
        else
            MessageBox.Show("Не удалось удалить учётные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void buttonRefreshCred_Click(object sender, EventArgs e) => LoadCredentials();

    private void listViewCreds_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (listViewCreds.SelectedItems.Count == 0)
            return;

        var selected = listViewCreds.SelectedItems[0];
        string oldTarget = selected.Tag as string ?? string.Empty;
        string domain = selected.SubItems[0].Text;
        string userName = selected.SubItems[1].Text;

        string? existing = CredentialManager.GetPassword(oldTarget);
        var result = FormManager.ShowPromptForm(domain, userName, existing);
        if (result == null)
            return;

        var (newDomain, newUser, password) = result.Value;
        string newTarget = CredentialManager.TrayTargetName(newDomain, newUser);

        // ключ изменился — удаляем старую запись
        if (!string.Equals(oldTarget, newTarget, StringComparison.OrdinalIgnoreCase))
            CredentialManager.DeleteCredential(oldTarget);

        CredentialManager.SavePassword(newTarget, newUser, password);
        SensitiveData.Clear(password);
        LoadCredentials();
    }

    private void buttonClose_Click(object sender, EventArgs e) => Close();
}
