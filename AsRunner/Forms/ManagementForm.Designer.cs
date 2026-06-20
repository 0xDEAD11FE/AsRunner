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
        tabControl.SuspendLayout();
        tabPageApps.SuspendLayout();
        tabPageCreds.SuspendLayout();
        SuspendLayout();
        //
        // tabControl
        //
        tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabControl.Controls.Add(tabPageApps);
        tabControl.Controls.Add(tabPageCreds);
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
        listViewApps.Columns.AddRange(new ColumnHeader[] { columnName, columnPath, columnAccount });
        listViewApps.FullRowSelect = true;
        listViewApps.Location = new Point(6, 6);
        listViewApps.MultiSelect = false;
        listViewApps.Name = "listViewApps";
        listViewApps.Size = new Size(540, 322);
        listViewApps.TabIndex = 0;
        listViewApps.UseCompatibleStateImageBehavior = false;
        listViewApps.View = View.Details;
        listViewApps.SelectedIndexChanged += listViewApps_SelectedIndexChanged;
        listViewApps.MouseDoubleClick += listViewApps_MouseDoubleClick;
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
        ResumeLayout(false);
    }

    #endregion

    private TabControl tabControl;
    private TabPage tabPageApps;
    private ListView listViewApps;
    private ColumnHeader columnName;
    private ColumnHeader columnPath;
    private ColumnHeader columnAccount;
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
}
