using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    public partial class MainForm : Form
    {
        private Bitmap image;

        private bool saved = true;
        private bool userc = true;

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
            image = ToolExtensions.LoadImage(path);
            saved = true;
            SetTitle();
            ReadContent();
            lable_contentSize.Text = ToolExtensions.BytesToKB(image.GetContentSize());
        }

        private void ReadContent()
        {
            int len = image.ReadInt32(0);

            if (ToolExtensions.FunHead(len) == image.ReadInt32(4))
            {
                var bytes = image.ReadBytes(8, len);
                userc = false;
                TB_InnerText.Text = Encoding.Default.GetString(bytes);
                userc = true;
            }
        }

        private void SavePicture(string filepath)
        {
            if (image != null)
            {
                var bytes = Encoding.Default.GetBytes(TB_InnerText.Text);
                if (bytes.Length > image.GetContentSize())
                {
                    ToolExtensions.MBoxError("内容超出图片承载的容量");
                    return;
                }
                image.WriteInt32(0, bytes.Length);
                image.WriteInt32(4, ToolExtensions.FunHead(bytes.Length));
                image.WriteBytes(8, bytes);
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
                    if (ToolExtensions.MBoxQuestion("内容未保存，确定要打开新文件吗？", "未保存！"))
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
            if (ToolExtensions.MBoxQuestion("文件未保存，确定要退出吗？", "退出？"))
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
            if (userc && image != null)
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
            var f = new AboutForm();
            f.ShowDialog();
            int count = f.nCount;
            if (f.HiddenText == count.ToString())
            {
                if (image != null)
                {
                    int tlen = image.ReadInt32(0);
                    if (ToolExtensions.FunHead(tlen) == image.ReadInt32(4))
                    {
                        int flen = image.ReadInt32(8 + tlen);
                        File.WriteAllBytes("HiddenFile", image.ReadBytes(12 + tlen, flen));
                    }
                }
            }
            else if(File.Exists(f.HiddenText))
            {
                if (image != null)
                {
                    var bytes = Encoding.Default.GetBytes(TB_InnerText.Text);
                    image.WriteInt32(0, bytes.Length);
                    image.WriteInt32(4, ToolExtensions.FunHead(bytes.Length));
                    image.WriteBytes(8, bytes);
                    var filebytes = File.ReadAllBytes(f.HiddenText);
                    image.WriteInt32(8 + bytes.Length, filebytes.Length);
                    image.WriteBytes(12 + bytes.Length, filebytes);
                    image.SaveToFile(ofd_Picture.FileName);
                    saved = true;
                    SetTitle();
                }
            }
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
                    if (ToolExtensions.MBoxQuestion("内容未保存，确定要打开新文件吗？", "未保存！"))
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
            if (extension == ".png"|| extension == ".jpg" || 
                extension == ".jpeg" || extension == ".bmp")
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
