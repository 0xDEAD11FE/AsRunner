namespace RunAsApplication;

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
        textBoxUserName = new TextBox();
        textBoxPassword = new TextBox();
        label1 = new Label();
        label2 = new Label();
        SuspendLayout();
        // 
        // buttonApply
        // 
        buttonApply.DialogResult = DialogResult.OK;
        buttonApply.Location = new Point(12, 115);
        buttonApply.Name = "buttonApply";
        buttonApply.Size = new Size(75, 23);
        buttonApply.TabIndex = 0;
        buttonApply.Text = "Принять";
        buttonApply.UseVisualStyleBackColor = true;
        // 
        // buttonCancel
        // 
        buttonCancel.DialogResult = DialogResult.Cancel;
        buttonCancel.Location = new Point(93, 115);
        buttonCancel.Name = "buttonCancel";
        buttonCancel.Size = new Size(75, 23);
        buttonCancel.TabIndex = 1;
        buttonCancel.Text = "Отмена";
        buttonCancel.UseVisualStyleBackColor = true;
        // 
        // textBoxUserName
        // 
        textBoxUserName.Location = new Point(12, 27);
        textBoxUserName.Name = "textBoxUserName";
        textBoxUserName.PlaceholderText = "Empty";
        textBoxUserName.Size = new Size(156, 23);
        textBoxUserName.TabIndex = 2;
        // 
        // textBoxPassword
        // 
        textBoxPassword.Location = new Point(12, 72);
        textBoxPassword.Name = "textBoxPassword";
        textBoxPassword.PasswordChar = '*';
        textBoxPassword.PlaceholderText = "Empty";
        textBoxPassword.Size = new Size(156, 23);
        textBoxPassword.TabIndex = 3;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(60, 15);
        label1.TabIndex = 4;
        label1.Text = "Username";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(12, 54);
        label2.Name = "label2";
        label2.Size = new Size(57, 15);
        label2.TabIndex = 5;
        label2.Text = "Password";
        // 
        // PromptUserPasswordForm
        // 
        AcceptButton = buttonApply;
        AccessibleRole = AccessibleRole.Dialog;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = buttonCancel;
        ClientSize = new Size(220, 175);
        ControlBox = false;
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(textBoxPassword);
        Controls.Add(textBoxUserName);
        Controls.Add(buttonCancel);
        Controls.Add(buttonApply);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Name = "PromptUserPasswordForm";
        ShowIcon = false;
        ShowInTaskbar = false;
        Text = "Prompt user password";
        FormClosing += PromptUserPasswordForm_FormClosing;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button buttonApply;
    private Button buttonCancel;
    private TextBox textBoxUserName;
    private TextBox textBoxPassword;
    private Label label1;
    private Label label2;
}