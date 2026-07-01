namespace AsRunner;

partial class PromptUserPasswordForm
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
        buttonApply = new Button();
        buttonCancel = new Button();
        textBoxDomain = new TextBox();
        textBoxUserName = new TextBox();
        textBoxPassword = new TextBox();
        buttonShowPassword = new Button();
        labelDomain = new Label();
        labelUserName = new Label();
        labelPassword = new Label();
        SuspendLayout();
        // 
        // buttonApply
        // 
        buttonApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonApply.DialogResult = DialogResult.OK;
        buttonApply.Location = new Point(177, 155);
        buttonApply.Name = "buttonApply";
        buttonApply.Size = new Size(90, 28);
        buttonApply.TabIndex = 3;
        buttonApply.Text = "ОК";
        buttonApply.UseVisualStyleBackColor = true;
        // 
        // buttonCancel
        // 
        buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        buttonCancel.DialogResult = DialogResult.Cancel;
        buttonCancel.Location = new Point(273, 155);
        buttonCancel.Name = "buttonCancel";
        buttonCancel.Size = new Size(90, 28);
        buttonCancel.TabIndex = 4;
        buttonCancel.Text = "Отмена";
        buttonCancel.UseVisualStyleBackColor = true;
        // 
        // textBoxDomain
        // 
        textBoxDomain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textBoxDomain.Location = new Point(15, 28);
        textBoxDomain.Name = "textBoxDomain";
        textBoxDomain.PlaceholderText = "DOMAIN или WORKGROUP";
        textBoxDomain.Size = new Size(348, 23);
        textBoxDomain.TabIndex = 0;
        // 
        // textBoxUserName
        // 
        textBoxUserName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textBoxUserName.Location = new Point(15, 70);
        textBoxUserName.Name = "textBoxUserName";
        textBoxUserName.PlaceholderText = "Имя пользователя";
        textBoxUserName.Size = new Size(348, 23);
        textBoxUserName.TabIndex = 1;
        // 
        // textBoxPassword
        // 
        textBoxPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        textBoxPassword.Location = new Point(15, 112);
        textBoxPassword.Name = "textBoxPassword";
        textBoxPassword.PlaceholderText = "Пароль";
        textBoxPassword.Size = new Size(315, 23);
        textBoxPassword.TabIndex = 2;
        textBoxPassword.UseSystemPasswordChar = true;
        //
        // buttonShowPassword
        //
        buttonShowPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        buttonShowPassword.Location = new Point(333, 111);
        buttonShowPassword.Name = "buttonShowPassword";
        buttonShowPassword.Size = new Size(30, 25);
        buttonShowPassword.TabStop = false;
        buttonShowPassword.Text = "👁";
        buttonShowPassword.UseVisualStyleBackColor = true;
        buttonShowPassword.MouseDown += buttonShowPassword_MouseDown;
        buttonShowPassword.MouseUp += buttonShowPassword_MouseUp;
        //
        // labelDomain
        // 
        labelDomain.AutoSize = true;
        labelDomain.Location = new Point(15, 10);
        labelDomain.Name = "labelDomain";
        labelDomain.Size = new Size(44, 15);
        labelDomain.TabIndex = 5;
        labelDomain.Text = "Домен";
        // 
        // labelUserName
        // 
        labelUserName.AutoSize = true;
        labelUserName.Location = new Point(15, 52);
        labelUserName.Name = "labelUserName";
        labelUserName.Size = new Size(109, 15);
        labelUserName.TabIndex = 6;
        labelUserName.Text = "Имя пользователя";
        // 
        // labelPassword
        // 
        labelPassword.AutoSize = true;
        labelPassword.Location = new Point(15, 94);
        labelPassword.Name = "labelPassword";
        labelPassword.Size = new Size(49, 15);
        labelPassword.TabIndex = 7;
        labelPassword.Text = "Пароль";
        // 
        // PromptUserPasswordForm
        // 
        AcceptButton = buttonApply;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = buttonCancel;
        ClientSize = new Size(378, 198);
        Controls.Add(labelPassword);
        Controls.Add(labelUserName);
        Controls.Add(labelDomain);
        Controls.Add(buttonShowPassword);
        Controls.Add(textBoxPassword);
        Controls.Add(textBoxUserName);
        Controls.Add(textBoxDomain);
        Controls.Add(buttonCancel);
        Controls.Add(buttonApply);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PromptUserPasswordForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Учетные данные";
        TopMost = true;
        FormClosing += PromptUserPasswordForm_FormClosing;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button buttonApply;
    private Button buttonCancel;
    private TextBox textBoxDomain;
    private TextBox textBoxUserName;
    private TextBox textBoxPassword;
    private Button buttonShowPassword;
    private Label labelDomain;
    private Label labelUserName;
    private Label labelPassword;
}
