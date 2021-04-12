using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Emgu.Util;
using DirectShowLib;
using ZXing;
namespace test_w_cam
{
    public partial class Form1 : Form
    {
        private System.ComponentModel.IContainer components = null;
        private VideoCapture capture = null;
        private DsDevice[] web_cam = null;
        private int select = 0;
        private IBarcodeReader reader = new BarcodeReader();

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            web_cam = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            for(int i =0; i< web_cam.Length; i++)
            {
                toolStripComboBox1.Items.Add(web_cam[i].Name);
            }
            try
            {
                if(web_cam.Length == 0)
                {
                    throw new Exception("У вас нет камер");
                }
                else if(web_cam == null)
                {
                    Console.WriteLine("lol");
                }
            }
            catch
            {
                Console.WriteLine("het");
            }
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if(web_cam.Length == 0)
                {
                    throw new Exception("НЕТ КАМЕР");
                }
                else if (toolStripComboBox1.SelectedItem ==null)
                {
                    throw new Exception("НЕТ ВЫБОРА");
                }
                else if(capture != null)
                {
                    capture.Start();
                }
                else
                {
                    capture = new VideoCapture(select);
                    capture.ImageGrabbed += Capture_ImageGrabbed;
                    capture.Start();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {

                Mat new_mat = new Mat();
                capture.Retrieve(new_mat);
                var barcodeBitmap = new_mat.ToImage<Bgr, byte>().Flip(Emgu.CV.CvEnum.FlipType.Horizontal).ToBitmap();
                var result = reader.Decode(barcodeBitmap);
                pictureBox1.Image = barcodeBitmap;
                // do something with the result
                if (result != null)
                {
                    Console.WriteLine(result.Text);
                }
            }
            catch
            {
                Console.WriteLine("ОШИБКА");
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            select = toolStripComboBox1.SelectedIndex;
        }
    }
}
