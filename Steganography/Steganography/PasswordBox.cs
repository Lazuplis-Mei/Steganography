using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography;
public partial class PasswordBox : Form
{
    public PasswordBox()
    {
        InitializeComponent();
    }
    public string Password = string.Empty;

    private void CB_Visible_CheckedChanged(object sender, EventArgs e)
    {
        if (CB_Visible.Checked)
            TB_Password.PasswordChar = '\0';
        else
            TB_Password.PasswordChar = '*';

    }

    private void TB_Password_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            Close();
    }

    private void PasswordBox_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Encoding.Default.GetByteCount(TB_Password.Text) > 32)
        {
            ToolExtensions.MBoxInfo("密码长度过长，请重新输入");
            e.Cancel = true;
        }
        Password = TB_Password.Text;
    }
}
