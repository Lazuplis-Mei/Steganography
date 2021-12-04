
namespace Steganography
{
    partial class ReplaceForm
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
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_FindNext = new System.Windows.Forms.Button();
            this.TB_FindText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_Replace = new System.Windows.Forms.Button();
            this.Btn_ReplaceAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_ReplaceText = new System.Windows.Forms.TextBox();
            this.CB_Case = new System.Windows.Forms.CheckBox();
            this.CB_Regex = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(450, 135);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(120, 35);
            this.Btn_Cancel.TabIndex = 3;
            this.Btn_Cancel.Text = "取消";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_FindNext
            // 
            this.Btn_FindNext.Location = new System.Drawing.Point(450, 12);
            this.Btn_FindNext.Name = "Btn_FindNext";
            this.Btn_FindNext.Size = new System.Drawing.Size(120, 35);
            this.Btn_FindNext.TabIndex = 0;
            this.Btn_FindNext.Text = "查找下一个";
            this.Btn_FindNext.UseVisualStyleBackColor = true;
            this.Btn_FindNext.Click += new System.EventHandler(this.Btn_FindNext_Click);
            // 
            // TB_FindText
            // 
            this.TB_FindText.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TB_FindText.Location = new System.Drawing.Point(102, 14);
            this.TB_FindText.Name = "TB_FindText";
            this.TB_FindText.Size = new System.Drawing.Size(342, 29);
            this.TB_FindText.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "查找内容：";
            // 
            // Btn_Replace
            // 
            this.Btn_Replace.Location = new System.Drawing.Point(450, 53);
            this.Btn_Replace.Name = "Btn_Replace";
            this.Btn_Replace.Size = new System.Drawing.Size(120, 35);
            this.Btn_Replace.TabIndex = 1;
            this.Btn_Replace.Text = "替换";
            this.Btn_Replace.UseVisualStyleBackColor = true;
            this.Btn_Replace.Click += new System.EventHandler(this.Btn_Replace_Click);
            // 
            // Btn_ReplaceAll
            // 
            this.Btn_ReplaceAll.Location = new System.Drawing.Point(450, 94);
            this.Btn_ReplaceAll.Name = "Btn_ReplaceAll";
            this.Btn_ReplaceAll.Size = new System.Drawing.Size(120, 35);
            this.Btn_ReplaceAll.TabIndex = 2;
            this.Btn_ReplaceAll.Text = "全部替换";
            this.Btn_ReplaceAll.UseVisualStyleBackColor = true;
            this.Btn_ReplaceAll.Click += new System.EventHandler(this.Btn_ReplaceAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "替换为：";
            // 
            // TB_ReplaceText
            // 
            this.TB_ReplaceText.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TB_ReplaceText.Location = new System.Drawing.Point(102, 55);
            this.TB_ReplaceText.Name = "TB_ReplaceText";
            this.TB_ReplaceText.Size = new System.Drawing.Size(342, 29);
            this.TB_ReplaceText.TabIndex = 5;
            // 
            // CB_Case
            // 
            this.CB_Case.AutoSize = true;
            this.CB_Case.Location = new System.Drawing.Point(12, 135);
            this.CB_Case.Name = "CB_Case";
            this.CB_Case.Size = new System.Drawing.Size(106, 24);
            this.CB_Case.TabIndex = 6;
            this.CB_Case.Text = "区分大小写";
            this.CB_Case.UseVisualStyleBackColor = true;
            // 
            // CB_Regex
            // 
            this.CB_Regex.AutoSize = true;
            this.CB_Regex.Location = new System.Drawing.Point(12, 167);
            this.CB_Regex.Name = "CB_Regex";
            this.CB_Regex.Size = new System.Drawing.Size(106, 24);
            this.CB_Regex.TabIndex = 7;
            this.CB_Regex.Text = "正则表达式";
            this.CB_Regex.UseVisualStyleBackColor = true;
            // 
            // ReplaceForm
            // 
            this.AcceptButton = this.Btn_FindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Btn_Cancel;
            this.ClientSize = new System.Drawing.Size(582, 203);
            this.Controls.Add(this.CB_Regex);
            this.Controls.Add(this.CB_Case);
            this.Controls.Add(this.TB_ReplaceText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Btn_ReplaceAll);
            this.Controls.Add(this.Btn_Replace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_FindText);
            this.Controls.Add(this.Btn_FindNext);
            this.Controls.Add(this.Btn_Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReplaceForm";
            this.ShowInTaskbar = false;
            this.Text = "替换";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReplaceForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_FindNext;
        private System.Windows.Forms.TextBox TB_FindText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CB_Case;
        private System.Windows.Forms.CheckBox CB_Regex;
        private System.Windows.Forms.Button Btn_Replace;
        private System.Windows.Forms.Button Btn_ReplaceAll;
        private System.Windows.Forms.TextBox TB_ReplaceText;
    }
}