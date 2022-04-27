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

        private bool imageSaved = true;
        private bool userChange = true;

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
            Text = imageSaved ? Text : "*" + Text;
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
            imageSaved = true;
            SetTitle();
            ReadContent();
            lable_contentSize.Text = BytesToKB(image.GetContentSize());
            TB_InnerText.ReadOnly = false;
            保存ToolStripMenuItem.Enabled = true;
            另存为ToolStripMenuItem.Enabled = true;
            插入文件ToolStripMenuItem.Enabled = true;
            加密内容ToolStripMenuItem.Enabled = true;
            压缩数据ToolStripMenuItem.Enabled = true;
        }

        private void ReadContent()
        {
            int pos = 0;
            int textLen = image.ReadInt32(ref pos);

            if (FunHead(textLen) == image.ReadInt32(ref pos))
            {
                var hash = image.ReadBytes(ref pos, 16);
                password = string.Empty;
                if (!EmptyData.SequenceEqual(hash))
                {
                    var passwordBox = new PasswordBox();
                    passwordBox.ShowDialog();
                    password = passwordBox.Password;
                    if (!GetMD5(password).SequenceEqual(hash))
                    {
                        MBoxError("密码不正确");
                        Environment.Exit(-1);
                    }
                    加密内容ToolStripMenuItem.Checked = true;
                }
                var textBytes = image.ReadBytes(ref pos, textLen);
                textBytes = Decrypt(textBytes, password);
                if (textBytes.Length > 0)
                {
                    try
                    {
                        textBytes = Decompress(textBytes);
                        压缩数据ToolStripMenuItem.Checked = true;
                    }
                    catch (InvalidDataException)
                    {
                        //数据无法解压
                    }
                }

                userChange = false;
                TB_InnerText.Text = Encoding.Default.GetString(textBytes);
                userChange = true;
                int filenameLen = image.ReadInt32(ref pos);
                if (FunHead(filenameLen) == image.ReadInt32(ref pos))
                {
                    var filenameBytes = image.ReadBytes(ref pos, filenameLen);
                    sfd_File.FileName = Encoding.Default.GetString(filenameBytes);
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
                var textBytes = Encoding.Default.GetBytes(TB_InnerText.Text);
                if (压缩数据ToolStripMenuItem.Checked)
                    textBytes = Compress(textBytes);
                textBytes = Encrypt(textBytes, password);


                var size = image.GetContentSize();
                if (textBytes.Length > size)
                {
                    MBoxError("内容超出图片承载的容量");
                    return;
                }
                提取文件ToolStripMenuItem.ToolTipText = null;
                提取文件ToolStripMenuItem.CheckState = CheckState.Unchecked;
                提取文件ToolStripMenuItem.Enabled = false;
                int pos = 0;
                image.WriteInt32(ref pos, textBytes.Length);
                image.WriteInt32(ref pos, FunHead(textBytes.Length));
                image.WriteBytes(ref pos, GetMD5(password));
                image.WriteBytes(ref pos, textBytes);
                size -= textBytes.Length;

                if (插入文件ToolStripMenuItem.CheckState == CheckState.Indeterminate
                    && File.Exists(ofd_File.FileName))
                {
                    var filenameBytes = Encoding.Default.GetBytes(ofd_File.FileName);
                    var fileBytes = File.ReadAllBytes(ofd_File.FileName);
                    if (压缩数据ToolStripMenuItem.Checked)
                        fileBytes = Compress(fileBytes);
                    fileBytes = Encrypt(fileBytes, password);
                    size -= 12;
                    if (filenameBytes.Length + fileBytes.Length > size)
                    {
                        MBoxError("插入的文件超出图片承载的容量");
                        return;
                    }
                    image.WriteInt32(ref pos, filenameBytes.Length);
                    image.WriteInt32(ref pos, FunHead(filenameBytes.Length));
                    image.WriteBytes(ref pos, filenameBytes);
                    image.WriteInt32(ref pos, fileBytes.Length);
                    image.WriteBytes(ref pos, fileBytes);
                }

                image.SaveToFile(filepath);
                imageSaved = true;
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
                if (imageSaved)
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
            if (imageSaved)
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
            if (userChange && image != null && imageSaved)
            {
                imageSaved = false;
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
                if (imageSaved)
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
                imageSaved = false;
                SetTitle();
            }
        }

        private void 提取文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null && sfd_File.ShowDialog() == DialogResult.OK)
            {
                int pos = 0;
                int textLen = image.ReadInt32(ref pos);
                pos = 24 + textLen;
                int filenameLen = image.ReadInt32(ref pos);
                pos = 32 + textLen + filenameLen;
                int fileSize = image.ReadInt32(ref pos);
                var fileBytes = image.ReadBytes(ref pos, fileSize);
                fileBytes = Decrypt(fileBytes, password);
                if (压缩数据ToolStripMenuItem.Checked)
                {
                    try
                    {
                        fileBytes = Decompress(fileBytes);
                    }
                    catch (InvalidDataException)
                    {
                        压缩数据ToolStripMenuItem.Checked = false;
                    }
                }
                File.WriteAllBytes(sfd_File.FileName, fileBytes);
            }
        }

        private void 加密内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageSaved = false;
            SetTitle();
        }

        private void 压缩数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageSaved = false;
            SetTitle();
        }
    }
}
