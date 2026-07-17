namespace AsRunner;

partial class EditApplicationForm
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
        components = new System.ComponentModel.Container();
        labelGroup = new Label();
        comboBoxGroup = new ComboBox();
        labelFile = new Label();
        textBoxFile = new TextBox();
        buttonBrowse = new Button();
        labelAlias = new Label();
        textBoxAlias = new TextBox();
        labelAccount = new Label();
        comboBoxAccount = new ComboBox();
        buttonAddCred = new Button();
        labelArgs = new Label();
        textBoxArgs = new TextBox();
        buttonOk = new Button();
        buttonCancel = new Button();
        openFileDialog = new OpenFileDialog();
        checkBoxShowInFolderMenu = new CheckBox();
        SuspendLayout();
        //
        // labelGroup
        //
        labelGroup.AutoSize = true;
        labelGroup.Location = new Point(12, 9);
        labelGroup.Name = "labelGroup";
        labelGroup.Text = "Группа";
        //
        // comboBoxGroup
        //
        comboBoxGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        comboBoxGroup.Location = new Point(12, 27);
        comboBoxGroup.Name = "comboBoxGroup";
        comboBoxGroup.Size = new Size(420, 23);
        comboBoxGroup.TabIndex = 0;
        //
        // labelFile
        //
        labelFile.AutoSize = true;
        labelFile.Location = new Point(12, 60);
        labelFile.Name = "labelFile";
        labelFile.Text = "Файл приложения";
        //
        // textBoxFile
        //
        textBoxFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textBoxFile.Location = new Point(12, 78);
        textBoxFile.Name = "textBoxFile";
        textBoxFile.Size = new Size(330, 23);
        textBoxFile.TabIndex = 1;
        //
        // buttonBrowse
        //
        buttonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        buttonBrowse.Location = new Point(348, 77);
        buttonBrowse.Name = "buttonBrowse";
        buttonBrowse.Size = new Size(84, 25);
        buttonBrowse.TabIndex = 2;
        buttonBrowse.Text = "Обзор…";
        buttonBrowse.UseVisualStyleBackColor = true;
        buttonBrowse.Click += buttonBrowse_Click;
        //
        // labelAlias
        //
        labelAlias.AutoSize = true;
        labelAlias.Location = new Point(12, 111);
        labelAlias.Name = "labelAlias";
        labelAlias.Text = "Alias (необязательно)";
        //
        // textBoxAlias
        //
        textBoxAlias.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textBoxAlias.Location = new Point(12, 129);
        textBoxAlias.Name = "textBoxAlias";
        textBoxAlias.Size = new Size(420, 23);
        textBoxAlias.TabIndex = 3;
        //
        // labelAccount
        //
        labelAccount.AutoSize = true;
        labelAccount.Location = new Point(12, 162);
        labelAccount.Name = "labelAccount";
        labelAccount.Text = "Учётная запись";
        //
        // comboBoxAccount
        //
        comboBoxAccount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        comboBoxAccount.DropDownStyle = ComboBoxStyle.DropDownList;
        comboBoxAccount.Location = new Point(12, 180);
        comboBoxAccount.Name = "comboBoxAccount";
        comboBoxAccount.Size = new Size(330, 23);
        comboBoxAccount.TabIndex = 4;
        //
        // buttonAddCred
        //
        buttonAddCred.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        buttonAddCred.Location = new Point(348, 179);
        buttonAddCred.Name = "buttonAddCred";
        buttonAddCred.Size = new Size(84, 25);
        buttonAddCred.TabIndex = 5;
        buttonAddCred.Text = "Добавить…";
        buttonAddCred.UseVisualStyleBackColor = true;
        buttonAddCred.Click += buttonAddCred_Click;
        //
        // labelArgs
        //
        labelArgs.AutoSize = true;
        labelArgs.Location = new Point(12, 213);
        labelArgs.Name = "labelArgs";
        labelArgs.Text = "Аргументы (необязательно; {folder} = путь папки)";
        //
        // textBoxArgs
        //
        textBoxArgs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textBoxArgs.Location = new Point(12, 231);
        textBoxArgs.Name = "textBoxArgs";
        textBoxArgs.Size = new Size(420, 23);
        textBoxArgs.TabIndex = 6;
        //
        // checkBoxShowInFolderMenu
        //
        checkBoxShowInFolderMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        checkBoxShowInFolderMenu.AutoSize = true;
        checkBoxShowInFolderMenu.Location = new Point(12, 264);
        checkBoxShowInFolderMenu.Name = "checkBoxShowInFolderMenu";
        checkBoxShowInFolderMenu.Text = "Показывать в контекстном меню папок Explorer";
        checkBoxShowInFolderMenu.UseVisualStyleBackColor = true;
        //
        // buttonOk
        //
        buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonOk.Location = new Point(260, 302);
        buttonOk.Name = "buttonOk";
        buttonOk.Size = new Size(84, 28);
        buttonOk.TabIndex = 7;
        buttonOk.Text = "ОК";
        buttonOk.UseVisualStyleBackColor = true;
        buttonOk.Click += buttonOk_Click;
        //
        // buttonCancel
        //
        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonCancel.DialogResult = DialogResult.Cancel;
        buttonCancel.Location = new Point(348, 302);
        buttonCancel.Name = "buttonCancel";
        buttonCancel.Size = new Size(84, 28);
        buttonCancel.TabIndex = 8;
        buttonCancel.Text = "Отмена";
        buttonCancel.UseVisualStyleBackColor = true;
        //
        // openFileDialog
        //
        openFileDialog.Filter = "Программы (*.exe)|*.exe|Все файлы (*.*)|*.*";
        openFileDialog.Title = "Выбор приложения";
        //
        // EditApplicationForm
        //
        AcceptButton = buttonOk;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = buttonCancel;
        ClientSize = new Size(444, 340);
        Controls.Add(checkBoxShowInFolderMenu);
        Controls.Add(buttonCancel);
        Controls.Add(buttonOk);
        Controls.Add(textBoxArgs);
        Controls.Add(labelArgs);
        Controls.Add(buttonAddCred);
        Controls.Add(comboBoxAccount);
        Controls.Add(labelAccount);
        Controls.Add(textBoxAlias);
        Controls.Add(labelAlias);
        Controls.Add(buttonBrowse);
        Controls.Add(textBoxFile);
        Controls.Add(labelFile);
        Controls.Add(comboBoxGroup);
        Controls.Add(labelGroup);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "EditApplicationForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Приложение";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label labelGroup;
    private ComboBox comboBoxGroup;
    private Label labelFile;
    private TextBox textBoxFile;
    private Button buttonBrowse;
    private Label labelAlias;
    private TextBox textBoxAlias;
    private Label labelAccount;
    private ComboBox comboBoxAccount;
    private Button buttonAddCred;
    private Label labelArgs;
    private TextBox textBoxArgs;
    private Button buttonOk;
    private Button buttonCancel;
    private OpenFileDialog openFileDialog;
    private CheckBox checkBoxShowInFolderMenu;
}
