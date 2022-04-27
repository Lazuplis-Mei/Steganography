namespace Steganography;

partial class PasswordBox
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
            this.TB_Password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CB_Visible = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TB_Password
            // 
            this.TB_Password.Location = new System.Drawing.Point(12, 44);
            this.TB_Password.MaxLength = 16;
            this.TB_Password.Name = "TB_Password";
            this.TB_Password.PasswordChar = '*';
            this.TB_Password.PlaceholderText = "请输入密码";
            this.TB_Password.Size = new System.Drawing.Size(259, 27);
            this.TB_Password.TabIndex = 0;
            this.TB_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_Password_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "请输入密码：";
            // 
            // CB_Visible
            // 
            this.CB_Visible.AutoSize = true;
            this.CB_Visible.Location = new System.Drawing.Point(277, 46);
            this.CB_Visible.Name = "CB_Visible";
            this.CB_Visible.Size = new System.Drawing.Size(61, 24);
            this.CB_Visible.TabIndex = 2;
            this.CB_Visible.Text = "可见";
            this.CB_Visible.UseVisualStyleBackColor = true;
            this.CB_Visible.CheckedChanged += new System.EventHandler(this.CB_Visible_CheckedChanged);
            // 
            // PasswordBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 96);
            this.Controls.Add(this.CB_Visible);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_Password);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入密码";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PasswordBox_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TB_Password;
    private System.Windows.Forms.CheckBox CB_Visible;
}
