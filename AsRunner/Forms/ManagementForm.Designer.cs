namespace AsRunner;

partial class ManagementForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        tabControl = new TabControl();
        tabPageApps = new TabPage();
        listViewApps = new ListView();
        columnName = new ColumnHeader();
        columnPath = new ColumnHeader();
        columnAccount = new ColumnHeader();
        columnFolderMenu = new ColumnHeader();
        buttonAddApp = new Button();
        buttonEditApp = new Button();
        buttonDeleteApp = new Button();
        buttonSaveApps = new Button();
        tabPageCreds = new TabPage();
        listViewCreds = new ListView();
        columnDomain = new ColumnHeader();
        columnUser = new ColumnHeader();
        buttonDeleteCred = new Button();
        buttonRefreshCred = new Button();
        labelCredHint = new Label();
        buttonClose = new Button();
        checkBoxAutoStart = new CheckBox();
        checkBoxFolderMenu = new CheckBox();
        checkBoxBetaUpdates = new CheckBox();
        buttonCheckUpdates = new Button();
        tabPageSettings = new TabPage();
        tabControl.SuspendLayout();
        tabPageApps.SuspendLayout();
        tabPageCreds.SuspendLayout();
        tabPageSettings.SuspendLayout();
        SuspendLayout();
        //
        // tabControl
        //
        tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabControl.Controls.Add(tabPageApps);
        tabControl.Controls.Add(tabPageCreds);
        tabControl.Controls.Add(tabPageSettings);
        tabControl.Location = new Point(12, 12);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(560, 400);
        tabControl.TabIndex = 0;
        //
        // tabPageApps
        //
        tabPageApps.Controls.Add(listViewApps);
        tabPageApps.Controls.Add(buttonAddApp);
        tabPageApps.Controls.Add(buttonEditApp);
        tabPageApps.Controls.Add(buttonDeleteApp);
        tabPageApps.Controls.Add(buttonSaveApps);
        tabPageApps.Location = new Point(4, 24);
        tabPageApps.Name = "tabPageApps";
        tabPageApps.Padding = new Padding(3);
        tabPageApps.Size = new Size(552, 372);
        tabPageApps.TabIndex = 0;
        tabPageApps.Text = "Приложения";
        tabPageApps.UseVisualStyleBackColor = true;
        //
        // listViewApps
        //
        listViewApps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        listViewApps.Columns.AddRange(new ColumnHeader[] { columnName, columnAccount, columnFolderMenu, columnPath });
        listViewApps.FullRowSelect = true;
        listViewApps.Location = new Point(6, 6);
        listViewApps.MultiSelect = false;
        listViewApps.Name = "listViewApps";
        listViewApps.OwnerDraw = true;
        listViewApps.Size = new Size(540, 322);
        listViewApps.TabIndex = 0;
        listViewApps.UseCompatibleStateImageBehavior = false;
        listViewApps.View = View.Details;
        listViewApps.SelectedIndexChanged += listViewApps_SelectedIndexChanged;
        listViewApps.MouseDoubleClick += listViewApps_MouseDoubleClick;
        listViewApps.DrawColumnHeader += listViewApps_DrawColumnHeader;
        listViewApps.DrawItem += listViewApps_DrawItem;
        listViewApps.DrawSubItem += listViewApps_DrawSubItem;
        //
        // columnName
        //
        columnName.Text = "Имя";
        columnName.Width = 160;
        //
        // columnPath
        //
        columnPath.Text = "Путь";
        columnPath.Width = 300;
        //
        // columnAccount
        //
        columnAccount.Text = "Учётка";
        columnAccount.Width = 140;
        //
        // columnFolderMenu
        //
        // Заголовок без текста — рисуем значок Проводника (owner-draw).
        columnFolderMenu.Text = "";
        columnFolderMenu.TextAlign = HorizontalAlignment.Center;
        columnFolderMenu.Width = 44;
        //
        // buttonAddApp
        //
        buttonAddApp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        buttonAddApp.Location = new Point(6, 334);
        buttonAddApp.Name = "buttonAddApp";
        buttonAddApp.Size = new Size(90, 28);
        buttonAddApp.TabIndex = 1;
        buttonAddApp.Text = "Добавить";
        buttonAddApp.UseVisualStyleBackColor = true;
        buttonAddApp.Click += buttonAddApp_Click;
        //
        // buttonEditApp
        //
        buttonEditApp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        buttonEditApp.Enabled = false;
        buttonEditApp.Location = new Point(102, 334);
        buttonEditApp.Name = "buttonEditApp";
        buttonEditApp.Size = new Size(90, 28);
        buttonEditApp.TabIndex = 2;
        buttonEditApp.Text = "Изменить";
        buttonEditApp.UseVisualStyleBackColor = true;
        buttonEditApp.Click += buttonEditApp_Click;
        //
        // buttonDeleteApp
        //
        buttonDeleteApp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        buttonDeleteApp.Enabled = false;
        buttonDeleteApp.Location = new Point(198, 334);
        buttonDeleteApp.Name = "buttonDeleteApp";
        buttonDeleteApp.Size = new Size(90, 28);
        buttonDeleteApp.TabIndex = 3;
        buttonDeleteApp.Text = "Удалить";
        buttonDeleteApp.UseVisualStyleBackColor = true;
        buttonDeleteApp.Click += buttonDeleteApp_Click;
        //
        // buttonSaveApps
        //
        buttonSaveApps.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonSaveApps.Enabled = false;
        buttonSaveApps.Location = new Point(456, 334);
        buttonSaveApps.Name = "buttonSaveApps";
        buttonSaveApps.Size = new Size(90, 28);
        buttonSaveApps.TabIndex = 4;
        buttonSaveApps.Text = "Сохранить";
        buttonSaveApps.UseVisualStyleBackColor = true;
        buttonSaveApps.Click += buttonSaveApps_Click;
        //
        // tabPageCreds
        //
        tabPageCreds.Controls.Add(listViewCreds);
        tabPageCreds.Controls.Add(buttonDeleteCred);
        tabPageCreds.Controls.Add(buttonRefreshCred);
        tabPageCreds.Controls.Add(labelCredHint);
        tabPageCreds.Location = new Point(4, 24);
        tabPageCreds.Name = "tabPageCreds";
        tabPageCreds.Padding = new Padding(3);
        tabPageCreds.Size = new Size(552, 372);
        tabPageCreds.TabIndex = 1;
        tabPageCreds.Text = "Учётные данные";
        tabPageCreds.UseVisualStyleBackColor = true;
        //
        // listViewCreds
        //
        listViewCreds.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        listViewCreds.Columns.AddRange(new ColumnHeader[] { columnDomain, columnUser });
        listViewCreds.FullRowSelect = true;
        listViewCreds.Location = new Point(6, 6);
        listViewCreds.MultiSelect = false;
        listViewCreds.Name = "listViewCreds";
        listViewCreds.Size = new Size(540, 322);
        listViewCreds.TabIndex = 0;
        listViewCreds.UseCompatibleStateImageBehavior = false;
        listViewCreds.View = View.Details;
        listViewCreds.SelectedIndexChanged += listViewCreds_SelectedIndexChanged;
        listViewCreds.MouseDoubleClick += listViewCreds_MouseDoubleClick;
        //
        // columnDomain
        //
        columnDomain.Text = "Домен";
        columnDomain.Width = 200;
        //
        // columnUser
        //
        columnUser.Text = "Имя пользователя";
        columnUser.Width = 250;
        //
        // buttonDeleteCred
        //
        buttonDeleteCred.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        buttonDeleteCred.Enabled = false;
        buttonDeleteCred.Location = new Point(6, 334);
        buttonDeleteCred.Name = "buttonDeleteCred";
        buttonDeleteCred.Size = new Size(90, 28);
        buttonDeleteCred.TabIndex = 1;
        buttonDeleteCred.Text = "Удалить";
        buttonDeleteCred.UseVisualStyleBackColor = true;
        buttonDeleteCred.Click += buttonDeleteCred_Click;
        //
        // buttonRefreshCred
        //
        buttonRefreshCred.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        buttonRefreshCred.Location = new Point(102, 334);
        buttonRefreshCred.Name = "buttonRefreshCred";
        buttonRefreshCred.Size = new Size(90, 28);
        buttonRefreshCred.TabIndex = 2;
        buttonRefreshCred.Text = "Обновить";
        buttonRefreshCred.UseVisualStyleBackColor = true;
        buttonRefreshCred.Click += buttonRefreshCred_Click;
        //
        // labelCredHint
        //
        labelCredHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        labelCredHint.AutoSize = true;
        labelCredHint.ForeColor = SystemColors.GrayText;
        labelCredHint.Location = new Point(200, 341);
        labelCredHint.Name = "labelCredHint";
        labelCredHint.Text = "Двойной клик — правка";
        //
        // buttonClose
        //
        buttonClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonClose.Location = new Point(488, 421);
        buttonClose.Name = "buttonClose";
        buttonClose.Size = new Size(84, 28);
        buttonClose.TabIndex = 1;
        buttonClose.Text = "Закрыть";
        buttonClose.UseVisualStyleBackColor = true;
        buttonClose.Click += buttonClose_Click;
        //
        // tabPageSettings
        //
        tabPageSettings.Controls.Add(checkBoxAutoStart);
        tabPageSettings.Controls.Add(checkBoxFolderMenu);
        tabPageSettings.Controls.Add(checkBoxBetaUpdates);
        tabPageSettings.Controls.Add(buttonCheckUpdates);
        tabPageSettings.Location = new Point(4, 24);
        tabPageSettings.Name = "tabPageSettings";
        tabPageSettings.Padding = new Padding(3);
        tabPageSettings.Size = new Size(552, 372);
        tabPageSettings.TabIndex = 2;
        tabPageSettings.Text = "Настройки";
        tabPageSettings.UseVisualStyleBackColor = true;
        //
        // checkBoxAutoStart
        //
        checkBoxAutoStart.AutoSize = true;
        checkBoxAutoStart.Location = new Point(16, 18);
        checkBoxAutoStart.Name = "checkBoxAutoStart";
        checkBoxAutoStart.TabIndex = 0;
        checkBoxAutoStart.Text = "Запускать при старте Windows";
        checkBoxAutoStart.UseVisualStyleBackColor = true;
        //
        // checkBoxFolderMenu
        //
        checkBoxFolderMenu.AutoSize = true;
        checkBoxFolderMenu.Location = new Point(16, 48);
        checkBoxFolderMenu.Name = "checkBoxFolderMenu";
        checkBoxFolderMenu.TabIndex = 1;
        checkBoxFolderMenu.Text = "Интеграция с контекстным меню папок Explorer";
        checkBoxFolderMenu.UseVisualStyleBackColor = true;
        //
        // checkBoxBetaUpdates
        //
        checkBoxBetaUpdates.AutoSize = true;
        checkBoxBetaUpdates.Location = new Point(16, 78);
        checkBoxBetaUpdates.Name = "checkBoxBetaUpdates";
        checkBoxBetaUpdates.TabIndex = 2;
        checkBoxBetaUpdates.Text = "Получать бета-версии (пред-релизы)";
        checkBoxBetaUpdates.UseVisualStyleBackColor = true;
        //
        // buttonCheckUpdates
        //
        buttonCheckUpdates.Location = new Point(16, 116);
        buttonCheckUpdates.Name = "buttonCheckUpdates";
        buttonCheckUpdates.Size = new Size(180, 28);
        buttonCheckUpdates.TabIndex = 3;
        buttonCheckUpdates.Text = "Проверить обновления";
        buttonCheckUpdates.UseVisualStyleBackColor = true;
        buttonCheckUpdates.Click += buttonCheckUpdates_Click;
        //
        // ManagementForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 461);
        Controls.Add(tabControl);
        Controls.Add(buttonClose);
        MinimumSize = new Size(420, 300);
        Name = "ManagementForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Управление AsRunner";
        tabControl.ResumeLayout(false);
        tabPageApps.ResumeLayout(false);
        tabPageCreds.ResumeLayout(false);
        tabPageCreds.PerformLayout();
        tabPageSettings.ResumeLayout(false);
        tabPageSettings.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TabControl tabControl;
    private TabPage tabPageApps;
    private ListView listViewApps;
    private ColumnHeader columnName;
    private ColumnHeader columnPath;
    private ColumnHeader columnAccount;
    private ColumnHeader columnFolderMenu;
    private Button buttonAddApp;
    private Button buttonEditApp;
    private Button buttonDeleteApp;
    private Button buttonSaveApps;
    private TabPage tabPageCreds;
    private ListView listViewCreds;
    private ColumnHeader columnDomain;
    private ColumnHeader columnUser;
    private Button buttonDeleteCred;
    private Button buttonRefreshCred;
    private Label labelCredHint;
    private Button buttonClose;
    private CheckBox checkBoxAutoStart;
    private CheckBox checkBoxFolderMenu;
    private CheckBox checkBoxBetaUpdates;
    private Button buttonCheckUpdates;
    private TabPage tabPageSettings;
}
