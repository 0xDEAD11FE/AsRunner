namespace RunAsApplication;

partial class ManageCredentialsForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        listViewCredentials = new ListView();
        columnHeaderDomain = new ColumnHeader();
        columnHeaderUserName = new ColumnHeader();
        buttonDelete = new Button();
        buttonClose = new Button();
        buttonRefresh = new Button();
        labelHint = new Label();
        SuspendLayout();
        // 
        // listViewCredentials
        // 
        listViewCredentials.Columns.AddRange(new ColumnHeader[] { columnHeaderDomain, columnHeaderUserName });
        listViewCredentials.FullRowSelect = true;
        listViewCredentials.Location = new Point(12, 12);
        listViewCredentials.MultiSelect = false;
        listViewCredentials.Name = "listViewCredentials";
        listViewCredentials.Size = new Size(460, 237);
        listViewCredentials.TabIndex = 0;
        listViewCredentials.UseCompatibleStateImageBehavior = false;
        listViewCredentials.View = View.Details;
        listViewCredentials.SelectedIndexChanged += listViewCredentials_SelectedIndexChanged;
        listViewCredentials.MouseDoubleClick += listViewCredentials_MouseDoubleClick;
        // 
        // columnHeaderDomain
        // 
        columnHeaderDomain.Text = "Домен";
        columnHeaderDomain.Width = 200;
        // 
        // columnHeaderUserName
        // 
        columnHeaderUserName.Text = "Имя пользователя";
        columnHeaderUserName.Width = 250;
        // 
        // buttonDelete
        // 
        buttonDelete.Enabled = false;
        buttonDelete.Location = new Point(12, 255);
        buttonDelete.Name = "buttonDelete";
        buttonDelete.Size = new Size(100, 30);
        buttonDelete.TabIndex = 1;
        buttonDelete.Text = "Удалить";
        buttonDelete.UseVisualStyleBackColor = true;
        buttonDelete.Click += buttonDelete_Click;
        // 
        // buttonClose
        // 
        buttonClose.Location = new Point(372, 255);
        buttonClose.Name = "buttonClose";
        buttonClose.Size = new Size(100, 30);
        buttonClose.TabIndex = 2;
        buttonClose.Text = "Закрыть";
        buttonClose.UseVisualStyleBackColor = true;
        buttonClose.Click += buttonClose_Click;
        // 
        // buttonRefresh
        // 
        buttonRefresh.Location = new Point(118, 255);
        buttonRefresh.Name = "buttonRefresh";
        buttonRefresh.Size = new Size(100, 30);
        buttonRefresh.TabIndex = 3;
        buttonRefresh.Text = "Обновить";
        buttonRefresh.UseVisualStyleBackColor = true;
        buttonRefresh.Click += buttonRefresh_Click;
        // 
        // labelHint
        // 
        labelHint.AutoSize = true;
        labelHint.ForeColor = SystemColors.GrayText;
        labelHint.Location = new Point(224, 261);
        labelHint.Name = "labelHint";
        labelHint.Size = new Size(142, 15);
        labelHint.TabIndex = 4;
        labelHint.Text = "Двойной клик для правки";
        // 
        // ManageCredentialsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(484, 297);
        Controls.Add(labelHint);
        Controls.Add(buttonRefresh);
        Controls.Add(buttonClose);
        Controls.Add(buttonDelete);
        Controls.Add(listViewCredentials);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ManageCredentialsForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Управление учетными данными";
        ResumeLayout(false);
    }

    #endregion

    private ListView listViewCredentials;
    private ColumnHeader columnHeaderDomain;
    private ColumnHeader columnHeaderUserName;
    private Button buttonDelete;
    private Button buttonClose;
    private Button buttonRefresh;
    private Label labelHint;
}
