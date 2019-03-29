using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrumPNG
{
    public partial class Form1 : Form
    {
        string originalPath;
        string folderPath;
        int recursionLevel = 0;
        Image trump;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            originalPath = textBox3.Text;
            folderPath = textBox1.Text;

            //Data validation
            if (!Directory.Exists(folderPath)) return;
            if (!File.Exists(originalPath)) return;
            if (!Int32.TryParse(textBox2.Text, out recursionLevel)) return;

            trump = Image.FromFile(originalPath);

            ReplaceInFolder(folderPath, recursionLevel);

            MessageBox.Show("Gotowe!");
        }

        private void ReplaceInFolder(string path, int rec)
        {
            string[] files = Directory.GetFiles(path);

            foreach(string file in files)
            {
                if(file.Substring(file.Length - 4) == ".png")
                {
                    //File we have found may have different resolution, so we have to scale our original
                    //Donald Trump to make him look good and don't break up game or whatever we edit

                    Image img = Image.FromFile(file);
                    int w = img.Width;
                    int h = img.Height;

                    img.Dispose();
                    img = null;

                    Image trumpCopy = resizeImage(trump, w, h);
                    trumpCopy.Save(file, System.Drawing.Imaging.ImageFormat.Png);
                    trumpCopy.Dispose();
                    trumpCopy = null;
                }
            }


            if (rec > 0)
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    ReplaceInFolder(dir, rec - 1);
                }
            }
        }

        public static Image resizeImage(Image image, int new_height, int new_width)
        {
            Bitmap new_image = new Bitmap(new_width, new_height);
            Graphics g = Graphics.FromImage((Image)new_image);
            g.InterpolationMode = InterpolationMode.High;
            g.DrawImage(image, 0, 0, new_width, new_height);
            return new_image;
        }
    }
}
