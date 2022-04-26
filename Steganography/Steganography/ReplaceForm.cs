using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    public partial class ReplaceForm : Form
    {
        public MainForm mainForm;
        public ReplaceForm(MainForm form)
        {
            InitializeComponent();
            mainForm = form;
            Owner = form;
        }

        private void ReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                Hide();
                mainForm.BringToFront();
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public void ShowFind()
        {
            if (!Visible)
            {
                Text = "查找";
                label1.Top = 39;
                TB_FindText.Top = 34;
                Btn_FindNext.Top = 32;
                label2.Visible = false;
                TB_ReplaceText.Visible = false;
                Btn_Replace.Visible = false;
                Btn_ReplaceAll.Visible = false;
                Show();
            }
        }

        public void ShowReplace()
        {
            if (!Visible)
            {
                Text = "替换";
                label1.Top = 19;
                TB_FindText.Top = 14;
                Btn_FindNext.Top = 12;
                label2.Visible = true;
                TB_ReplaceText.Visible = true;
                Btn_Replace.Visible = true;
                Btn_ReplaceAll.Visible = true;
                Show();
            }
        }
        private void Btn_FindNext_Click(object sender, EventArgs e)
        {
            var ss = mainForm.TB_InnerText.SelectionStart;
            var slen = mainForm.TB_InnerText.SelectionLength;
            var text = mainForm.TB_InnerText.Text[(ss + slen)..];
            var str = TB_FindText.Text;
            if (str.Length > 0)
            {
                if (!CB_Regex.Checked)
                {
                    int si;
                    if (CB_Case.Checked)
                    {
                        si = text.IndexOf(str);
                    }
                    else
                    {
                        si = text.ToLower().IndexOf(str.ToLower());
                    }
                    if (si == -1)
                    {
                        ToolExtensions.MBoxInfo($"找不到：{str}");
                    }
                    else
                    {
                        mainForm.TB_InnerText.Select(ss + slen + si, str.Length);
                        mainForm.BringToFront();
                    }
                }
                else
                {
                    var match = Regex.Match(text, str, CB_Case.Checked ?
                     RegexOptions.None : RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        ToolExtensions.MBoxInfo($"找不到：{str}");
                    }
                    else
                    {
                        mainForm.TB_InnerText.Select(ss + slen + match.Index, match.Length);
                        mainForm.BringToFront();
                    }
                }
            }
        }

        private void Btn_Replace_Click(object sender, EventArgs e)
        {
            var ss = mainForm.TB_InnerText.SelectionStart;
            var slen = mainForm.TB_InnerText.SelectionLength;
            if (slen == 0)
            {
                Btn_FindNext.PerformClick();
                return;
            }

            if (CB_Regex.Checked)
            {
                if (!Regex.IsMatch(mainForm.TB_InnerText.SelectedText, 
                    TB_FindText.Text, CB_Case.Checked ? 
                    RegexOptions.None : RegexOptions.IgnoreCase))
                {
                    Btn_FindNext.PerformClick();
                    return;
                }
            }
            else
            {
                if (CB_Case.Checked)
                {
                    if (mainForm.TB_InnerText.SelectedText!= TB_FindText.Text)
                    {
                        Btn_FindNext.PerformClick();
                        return;
                    }
                }
                else
                {
                    if (mainForm.TB_InnerText.SelectedText.ToLower() != TB_FindText.Text.ToLower())
                    {
                        Btn_FindNext.PerformClick();
                        return;
                    }
                }
            }
            mainForm.TB_InnerText.Text = 
                mainForm.TB_InnerText.Text[..ss]
                + TB_ReplaceText.Text
                + mainForm.TB_InnerText.Text[(ss + slen)..];
            mainForm.TB_InnerText.Select(ss, TB_ReplaceText.TextLength);
            mainForm.BringToFront();
        }

        private void Btn_ReplaceAll_Click(object sender, EventArgs e)
        {
            var text = mainForm.TB_InnerText.Text;
            var str = TB_FindText.Text;
            var rep = TB_ReplaceText.Text;
            if (str.Length > 0)
            {
                if (!CB_Regex.Checked)
                {
                    mainForm.TB_InnerText.Text = text.Replace(str, rep,
                        CB_Case.Checked ? StringComparison.Ordinal :
                        StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    mainForm.TB_InnerText.Text = Regex.Replace(text, str, rep,
                        CB_Case.Checked ? RegexOptions.None : RegexOptions.IgnoreCase);
                }
                mainForm.BringToFront();
            }
        }
    }
}
