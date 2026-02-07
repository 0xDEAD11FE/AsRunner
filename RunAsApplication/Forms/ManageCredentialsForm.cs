using WinApiWrapper;

namespace RunAsApplication;

public partial class ManageCredentialsForm : Form
{
    public ManageCredentialsForm()
    {
        InitializeComponent();
        LoadCredentials();
    }

    private void LoadCredentials()
    {
        listViewCredentials.Items.Clear();
        var credentials = CredentialManager.GetTrayCredentials();

        foreach (var (targetName, userName, domain) in credentials)
        {
            var item = new ListViewItem(new[] { domain, userName });
            item.Tag = targetName;
            listViewCredentials.Items.Add(item);
        }

        buttonDelete.Enabled = listViewCredentials.SelectedItems.Count > 0;
    }

    private void buttonDelete_Click(object sender, EventArgs e)
    {
        if (listViewCredentials.SelectedItems.Count == 0)
            return;

        var selectedItem = listViewCredentials.SelectedItems[0];
        string targetName = selectedItem.Tag as string ?? string.Empty;

        var result = MessageBox.Show(
            $"Удалить сохраненные учетные данные для {selectedItem.SubItems[0].Text}\\{selectedItem.SubItems[1].Text}?",
            "Подтверждение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                if (CredentialManager.DeleteCredential(targetName))
                {
                    LoadCredentials();
                }
                else
                {
                    MessageBox.Show("Не удалось удалить учетные данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void buttonClose_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void listViewCredentials_SelectedIndexChanged(object sender, EventArgs e)
    {
        buttonDelete.Enabled = listViewCredentials.SelectedItems.Count > 0;
    }

    private void buttonRefresh_Click(object sender, EventArgs e)
    {
        LoadCredentials();
    }

    private void listViewCredentials_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (listViewCredentials.SelectedItems.Count == 0)
            return;

        var selectedItem = listViewCredentials.SelectedItems[0];
        string oldTargetName = selectedItem.Tag as string ?? string.Empty;
        string domain = selectedItem.SubItems[0].Text;
        string userName = selectedItem.SubItems[1].Text;

        // Получаем существующий пароль
        string? existingPassword = CredentialManager.GetPassword(oldTargetName);

        // Открываем форму редактирования
        using (var form = new PromptUserPasswordForm(domain, userName, existingPassword))
        {
            if (form.ShowDialog() == DialogResult.OK &&
                !string.IsNullOrWhiteSpace(form.Domain) &&
                !string.IsNullOrWhiteSpace(form.UserName) &&
                !string.IsNullOrWhiteSpace(form.Password))
            {
                try
                {
                    string newTargetName = $"Tray:{form.Domain}\\{form.UserName}";

                    // Если ключ изменился, удаляем старую запись
                    if (oldTargetName != newTargetName)
                    {
                        CredentialManager.DeleteCredential(oldTargetName);
                    }

                    // Сохраняем новую/обновленную запись
                    CredentialManager.SavePassword(newTargetName, form.UserName, form.Password);

                    // Обновляем список
                    LoadCredentials();

                    MessageBox.Show("Учетные данные успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
