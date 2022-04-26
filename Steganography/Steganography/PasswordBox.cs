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

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1.Checked)
            tb_Password.PasswordChar = '\0';
        else
            tb_Password.PasswordChar = '*';

    }

    private void tb_Password_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            Close();
    }

    private void PasswordBox_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Encoding.Default.GetByteCount(tb_Password.Text) > 32)
        {
            ToolExtensions.MBoxInfo("密码长度过长，请重新输入");
            e.Cancel = true;
        }
        Password = tb_Password.Text;
    }
}
