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

        bool IsRuncap;
        private void Form1_Load(object sender, EventArgs e)
        {
            FaceDetected = new FaceDetected(FaceDetected.FaceRecognizerType.LBPHFFaceRecognizer);
            FaceDetected.LoadTrainedFaceRecognizer();
            _Size = new Size(100, 100);
        }
        private void button1_Click(object sender, EventArgs e)
        {
             FaceDetected.SetTrainedFaceRecognizerFromImage(FaceDetected.FaceRecognizerType.LBPHFFaceRecognizer, FaceDetected.CurrentFaceList[0], textBox1.Text);
            //AutoTrained = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FaceDetected.SaveTrainedFaceRecognizer();
        }
        private void ShowImage()
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
                    if(FaceDetected._CurrentFace!=null)
                        pictureBox2.Image = FaceDetected._CurrentFace.Bitmap;
                    Thread.Sleep(20);
                }
            }

        }
        private void btnOpenCam_Click(object sender, EventArgs e)
        {
            if (!IsRuncap)
            {
                IsRuncap = true;
                Cap = new Capture();
                _SrcImage = new Mat();
                Cap.Start();
                Thread T1 = new Thread(ShowImage);
                T1.Start();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cap.Stop();
            IsRuncap = false;
        }
    }
}
