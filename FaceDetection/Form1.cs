using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using System.Threading;

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FaceDetected FaceDetected;
        Capture Cap;
        Mat _SrcImage;
        Size _Size;
        object key = new object();
        FaceDetected.FaceRecognizerType RecognizerType = FaceDetected.FaceRecognizerType.LBPHFFaceRecognizer;
        bool IsRuncap;
        private void Form1_Load(object sender, EventArgs e)
        {
            FaceDetected = new FaceDetected(RecognizerType);
            FaceDetected.LoadTrainedFaceRecognizer();
            _Size = new Size(100, 100);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (TrainNameBox.Text.Trim() == "" || FaceDetected.CurrentFaceList.Count < 1)
                return;
            FaceDetected.SetTrainedFaceRecognizerFromImage(RecognizerType, FaceDetected.CurrentFaceList[0], TrainNameBox.Text);
            //AutoTrained = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FaceDetected.SaveTrainedFaceRecognizer();
        }
        private void ShowImage()
        {
            try
            {
                while (IsRuncap)
                {
                    _SrcImage = Cap.QueryFrame();
                    lock (key)
                    {
                        if (pictureBox1.Image != null)
                        {
                            _SrcImage = FaceDetected.DetectedFace(_SrcImage, _Size, 100);
                        }

                        CvInvoke.Resize(_SrcImage, _SrcImage, pictureBox1.Size);
                        Bitmap oldBitmap = pictureBox1.Image as Bitmap;
                        pictureBox1.Image = _SrcImage.Bitmap;
                        if (oldBitmap != null)
                        {
                            oldBitmap.Dispose();
                        }
                        if (FaceDetected._CurrentFace != null)
                            pictureBox2.Image = FaceDetected._CurrentFace.Bitmap;
                        Thread.Sleep(20);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                IsRuncap = false;
            }

        }
        private void btnOpenCam_Click(object sender, EventArgs e)
        {
            try 
            {
                if (!IsRuncap)
                {
                    Cap = new Capture(0);
                    if (Cap.Width == 0 || Cap.Height == 0)
                        throw new Exception("Can't Find Camera");
                    _SrcImage = new Mat();
                    Cap.Start();
                    IsRuncap = true;
                    Thread T1 = new Thread(ShowImage);
                    T1.Start();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Cap!=null)
                Cap.Stop();
            IsRuncap = false;
        }
    }
}
