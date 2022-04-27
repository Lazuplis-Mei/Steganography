using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Steganography
{
    static class ToolExtensions
    {
        public static readonly byte[] EmptyData = { 212, 29, 140, 217, 143, 0, 178, 4, 233, 128, 9, 152, 236, 248, 66, 126 };
        private static readonly MD5 md5 = MD5.Create();
        private static readonly Aes aes = Aes.Create();

        public static byte[] GetMD5(string str)
        {
            var bytes = Encoding.Default.GetBytes(str);
            return md5.ComputeHash(bytes);
        }

        public static byte[] Encrypt(byte[] bytes, string password)
        {
            if (string.IsNullOrEmpty(password))
                return bytes;

            var key = new byte[32];
            Encoding.Default.GetBytes(password).CopyTo(key, 0);
            aes.Key = key;
            return aes.EncryptCbc(bytes, EmptyData);
        }

        public static byte[] Decrypt(byte[] bytes, string password)
        {
            if (string.IsNullOrEmpty(password))
                return bytes;

            var key = new byte[32];
            Encoding.Default.GetBytes(password).CopyTo(key, 0);
            aes.Key = key;
            return aes.DecryptCbc(bytes, EmptyData);
        }

        public static bool MBoxQuestion(string text, string title)
        {
            return MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static void MBoxError(string text)
        {
            MessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MBoxInfo(string text)
        {
            MessageBox.Show(text, "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static Bitmap LoadImage(string filePath)
        {
            var stream = new MemoryStream(File.ReadAllBytes(filePath));
            var img = new Bitmap(stream);
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            var image = img.Clone(rect, PixelFormat.Format32bppArgb);
            img.Dispose();
            stream.Dispose();
            return image;
        }

        public static string BytesToKB(int n)
        {
            return (n / 1024D).ToString("F2") + "KB";
        }

        public static int FunHead(int n)
        {
            return (int)Math.Floor(Math.Pow(n, 0.8) * Math.Log(n));
        }

        public static int GetContentSize(this Bitmap self)
        {
            return (self.Width * self.Height) / 2 - 24;
        }

        public static void SaveToFile(this Bitmap self, string filepath)
        {
            filepath = Path.Combine(Path.GetDirectoryName(filepath),
                Path.GetFileNameWithoutExtension(filepath) + ".png");
            self.Save(filepath, ImageFormat.Png);
        }

        private static byte ReadByte(this Bitmap self, int pos)
        {
            pos *= 2;

            int x = pos % self.Width;
            int y = pos / self.Width;
            var color = self.GetPixel(x, y);
            var bx = (color.A & 1) |
                ((color.R & 1) << 1) |
                ((color.G & 1) << 2) |
                ((color.B & 1) << 3);

            x = (pos + 1) % self.Width;
            y = (pos + 1) / self.Width;
            color = self.GetPixel(x, y);
            bx |= ((color.A & 1) << 4) |
                ((color.R & 1) << 5) |
                ((color.G & 1) << 6) |
                ((color.B & 1) << 7);

            return (byte)bx;
        }

        public static byte[] ReadBytes(this Bitmap self, ref int pos, int len)
        {
            var bytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                bytes[i] = self.ReadByte(pos + i);
            }
            pos += len;
            return bytes;
        }

        public static int ReadInt32(this Bitmap self, ref int pos)
        {
            return BitConverter.ToInt32(self.ReadBytes(ref pos, 4));
        }

        private static void WriteByte(this Bitmap self, int pos, byte value)
        {
            pos *= 2;

            int x = pos % self.Width;
            int y = pos / self.Width;
            var color = self.GetPixel(x, y);
            self.SetPixel(x, y, WriteLowToColor(color, value));

            x = (pos + 1) % self.Width;
            y = (pos + 1) / self.Width;
            color = self.GetPixel(x, y);
            self.SetPixel(x, y, WriteHighToColor(color, value));
        }

        private static Color WriteLowToColor(Color color, byte value)
        {
            var a = ((color.A >> 1) << 1) | (value & 1);
            var r = ((color.R >> 1) << 1) | ((value >> 1) & 1);
            var g = ((color.G >> 1) << 1) | ((value >> 2) & 1);
            var b = ((color.B >> 1) << 1) | ((value >> 3) & 1);
            return Color.FromArgb(a, r, g, b);
        }

        private static Color WriteHighToColor(Color color, byte value)
        {
            var a = ((color.A >> 1) << 1) | ((value >> 4) & 1);
            var r = ((color.R >> 1) << 1) | ((value >> 5) & 1);
            var g = ((color.G >> 1) << 1) | ((value >> 6) & 1);
            var b = ((color.B >> 1) << 1) | ((value >> 7) & 1);
            return Color.FromArgb(a, r, g, b);
        }

        public static void WriteBytes(this Bitmap self, ref int pos, byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                self.WriteByte(pos + i, bytes[i]);
            }
            pos += bytes.Length;
        }

        public static void WriteInt32(this Bitmap self, ref int pos, int value)
        {
            self.WriteBytes(ref pos, BitConverter.GetBytes(value));
        }

        public static byte[] Compress(byte[] bytes)
        {
            using var memory = new MemoryStream();
            using var gZip = new GZipStream(memory, CompressionMode.Compress);
            gZip.Write(bytes, 0, bytes.Length);
            gZip.Close();
            return memory.ToArray();
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using var gZip = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress);
            using var memory = new MemoryStream();
            gZip.CopyTo(memory);
            gZip.Close();
            return memory.ToArray();
        }
    }
}
