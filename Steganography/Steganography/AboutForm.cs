using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Steganography
{
    public partial class AboutForm : Form
    {
        public int nCount;
        public string HiddenText;

        private int clickCount = 0;
        private readonly Random random = new();
        public AboutForm()
        {
            InitializeComponent();
            nCount = random.Next(10, 30);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = "https://github.com/Lazuplis-Mei/Steganography";
            try
            {
                Process.Start(url);
            }
            catch (Win32Exception)
            {
                Clipboard.SetText(url);
            }
        }

        private void AboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HiddenText = textBox1.Text;
        }
        
        private void Label2_Click(object sender, EventArgs e)
        {
            clickCount++;
            if (clickCount == nCount)
            {
                textBox1.Visible = true;
            }
        }
    }
}
