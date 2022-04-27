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
        public AboutForm()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var url = "https://github.com/Lazuplis-Mei/Steganography";
            try
            {
                Process.Start("explorer.exe", url);
            }
            catch (Win32Exception)
            {
                Clipboard.SetText(url);
            }
        }

    }
}
