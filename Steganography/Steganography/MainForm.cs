using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Steganography.ToolExtensions;

namespace Steganography
{
    public partial class MainForm : Form
    {
        private Bitmap image;

        private bool saved = true;
        private bool userc = true;

        private string password = string.Empty;

        private readonly ReplaceForm replaceForm;

        public MainForm()
        {
            InitializeComponent();
            replaceForm = new ReplaceForm(this);
        }

        private void SetTitle()
        {
            Text = Path.GetFileName(ofd_Picture.FileName) + " - Steganography";
            Text = saved ? Text : "*" + Text;
        }

        private void OpenPicture()
        {
            if (ofd_Picture.ShowDialog() == DialogResult.OK)
            {
                InternalOpenPicture(ofd_Picture.FileName);
            }
        }

        private void InternalOpenPicture(string path)
        {
            ofd_Picture.FileName = path;
            image?.Dispose();
            image = LoadImage(path);
            saved = true;
            SetTitle();
            ReadContent();
            lable_contentSize.Text = BytesToKB(image.GetContentSize());
            TB_InnerText.ReadOnly = false;
            保存ToolStripMenuItem.Enabled = true;
            另存为ToolStripMenuItem.Enabled = true;
            插入文件ToolStripMenuItem.Enabled = true;
            加密内容ToolStripMenuItem.Enabled = true;
        }

        private void ReadContent()
        {
            int len = image.ReadInt32(0);

            if (FunHead(len) == image.ReadInt32(4))
            {
                byte[] head = image.ReadBytes(8, 16);
                password = string.Empty;
                if (!EmptyData.SequenceEqual(head))
                {
                    var passwordBox = new PasswordBox();
                    passwordBox.ShowDialog();
                    password = passwordBox.Password;
                    if (!head.SequenceEqual(GetMD5(password)))
                    {
                        MBoxError("密码不正确");
                        Environment.Exit(-1);
                    }
                    加密内容ToolStripMenuItem.Checked = true;
                }
                var bytes = image.ReadBytes(16 + 8, len);
                if (password != string.Empty)
                    bytes = Decrypt(bytes, password);

                userc = false;
                TB_InnerText.Text = Encoding.Default.GetString(bytes);
                userc = true;
                int fnLen = image.ReadInt32(16 + 8 + len);
                if (FunHead(fnLen) == image.ReadInt32(16 + 12 + len))
                {
                    var fnbytes = image.ReadBytes(16 + 16 + len, fnLen);
                    sfd_File.FileName = Encoding.Default.GetString(fnbytes);
                    提取文件ToolStripMenuItem.ToolTipText = $"待提取文件：[{sfd_File.FileName}]";
                    提取文件ToolStripMenuItem.CheckState = CheckState.Indeterminate;
                    提取文件ToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void SavePicture(string filepath)
        {
            if (image != null)
            {
                string password = string.Empty;
                if (加密内容ToolStripMenuItem.Checked)
                {
                    var passwordBox = new PasswordBox();
                    passwordBox.ShowDialog();
                    password = passwordBox.Password;
                }
                image.WriteBytes(8, GetMD5(password));

                var bytes = Encoding.Default.GetBytes(TB_InnerText.Text);
                if (password != string.Empty)
                    bytes = Encrypt(bytes, password);

                var size = image.GetContentSize();
                if (bytes.Length > size)
                {
                    MBoxError("内容超出图片承载的容量");
                    return;
                }
                提取文件ToolStripMenuItem.ToolTipText = null;
                提取文件ToolStripMenuItem.CheckState = CheckState.Unchecked;
                提取文件ToolStripMenuItem.Enabled = false;
                image.WriteInt32(0, bytes.Length);
                image.WriteInt32(4, FunHead(bytes.Length));
                image.WriteBytes(16 + 8, bytes);
                size -= bytes.Length;

                if (插入文件ToolStripMenuItem.CheckState == CheckState.Indeterminate
                    && File.Exists(ofd_File.FileName))
                {
                    var fnbytes = Encoding.Default.GetBytes(ofd_File.FileName);
                    var filebytes = File.ReadAllBytes(ofd_File.FileName);
                    if (password != string.Empty)
                        filebytes = Encrypt(filebytes, password);
                    size -= 8;
                    if (fnbytes.Length + filebytes.Length > size)
                    {
                        MBoxError("插入的文件超出图片承载的容量");
                        return;
                    }
                    image.WriteInt32(16 + 8 + bytes.Length, fnbytes.Length);
                    image.WriteInt32(16 + 12 + bytes.Length, FunHead(fnbytes.Length));
                    image.WriteBytes(16 + 16 + bytes.Length, fnbytes);
                    image.WriteInt32(16 + 16 + bytes.Length + fnbytes.Length, filebytes.Length);
                    image.WriteBytes(16 + 20 + bytes.Length + fnbytes.Length, filebytes);
                }

                image.SaveToFile(filepath);
                saved = true;
                SetTitle();
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image == null)
            {
                OpenPicture();
            }
            else
            {
                if (saved)
                {
                    OpenPicture();
                }
                else
                {
                    if (MBoxQuestion("内容未保存，确定要打开新文件吗？", "未保存！"))
                    {
                        OpenPicture();
                    }
                }
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePicture(ofd_Picture.FileName);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saved)
                return;
            if (MBoxQuestion("文件未保存，确定要退出吗？", "退出？"))
                return;
            e.Cancel = true;
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null && sfd_Picture.ShowDialog() == DialogResult.OK)
            {
                SavePicture(sfd_Picture.FileName);
            }
        }

        private void TB_InnerText_TextChanged(object sender, EventArgs e)
        {
            if (userc && image != null && saved)
            {
                saved = false;
                SetTitle();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            TB_InnerText.Height = Height - 107;
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            replaceForm.ShowFind();
        }

        private void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            replaceForm.ShowReplace();
        }

        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDlg.ShowDialog() == DialogResult.OK)
            {
                TB_InnerText.Font = fontDlg.Font;
                TB_InnerText.ForeColor = fontDlg.Color;
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void TB_InnerText_DragDrop(object sender, DragEventArgs e)
        {
            string localFilePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (image == null)
            {
                InternalOpenPicture(localFilePath);
            }
            else
            {
                if (saved)
                {
                    InternalOpenPicture(localFilePath);
                }
                else
                {
                    if (MBoxQuestion("内容未保存，确定要打开新文件吗？", "未保存！"))
                    {
                        InternalOpenPicture(localFilePath);
                    }
                }
            }
        }

        private void TB_InnerText_DragEnter(object sender, DragEventArgs e)
        {
            string localFilePath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            string extension = Path.GetExtension(localFilePath).ToLower();
            e.Effect = extension is ".png" or ".jpg" or ".jpeg" or ".bmp"
                ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void 插入文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null && ofd_File.ShowDialog() == DialogResult.OK)
            {
                插入文件ToolStripMenuItem.ToolTipText = $"已选择文件：[{ofd_File.FileName}]";
                插入文件ToolStripMenuItem.CheckState = CheckState.Indeterminate;
                saved = false;
                SetTitle();
            }
        }

        private void 提取文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null && sfd_File.ShowDialog() == DialogResult.OK)
            {
                int len = image.ReadInt32(0);
                int fnLen = image.ReadInt32(16 + 8 + len);
                int fsize = image.ReadInt32(16 + 16 + len + fnLen);
                var fbytes = image.ReadBytes(16 + 20 + len + fnLen, fsize);
                if (password != string.Empty)
                    fbytes = Decrypt(fbytes, password);
                File.WriteAllBytes(sfd_File.FileName, fbytes);
            }
        }

        private void 加密内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saved = false;
            SetTitle();
        }
    }
}
