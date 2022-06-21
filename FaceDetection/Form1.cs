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

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        public class TrainedFileList
        {
            public List<Image<Gray, byte>> trainedImage = new List<Image<Gray, byte>>();
            public List<int> trainedLabelOrder = new List<int>();
            public List<string> trainedFileName = new List<string>();
        }
        public class TrainedFaceRecognizer
        {
            public FaceRecognizer faceRecognizer;
            public TrainedFileList TrainedFileList;
        }

        public class TrainedFileListFromImage
        {
            public List<Mat> trainedImage = new List<Mat>();
            public List<int> trainedLabelOrder = new List<int>();
            public List<string> trainedFileName = new List<string>();
        }
        public class TrainedFaceRecognizerFromImage
        {
            public FaceRecognizer faceRecognizer;
            public TrainedFileListFromImage TrainedFileList;
        }
        public class faceDetectedObj
        {
            public Mat originalImg;
            public List<Rectangle> FaceRectangles;
            public List<string> name = new List<string>();
        }
        public enum FaceRecognizerType
        {
            EigenFaceRecognizer = 0,
            FisherFaceRecognizer = 1,
            LBPHFFaceRecognizer = 2,
        };
        
        public Form1()
        {
            InitializeComponent();
        }

        CascadeClassifier FaceCascadeClassifier;
        Mat OrgMat = new Mat();

        public TrainedFileList SetSampleFacesList()
        {
            TrainedFileList tf = new TrainedFileList();
            DirectoryInfo di = new DirectoryInfo("TrainedFace");
            int i = 0;
            foreach (FileInfo fi in di.GetFiles())
            {
                tf.trainedImage.Add(new Image<Gray, byte>(fi.FullName));
                tf.trainedLabelOrder.Add(i);
                tf.trainedFileName.Add(fi.Name.Split('_')[0]);
                i++;
            }
            return tf;
        }
        public TrainedFaceRecognizer SetTrainedFaceRecognizer(FaceRecognizerType type)
        {
            TrainedFaceRecognizer tfr = new TrainedFaceRecognizer();
            tfr.TrainedFileList = SetSampleFacesList();
            switch (type)
            {
                case FaceRecognizerType.EigenFaceRecognizer:
                    tfr.faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
                    break;

                case FaceRecognizerType.FisherFaceRecognizer:
                    tfr.faceRecognizer = new FisherFaceRecognizer(80, 3500);
                    break;

                case FaceRecognizerType.LBPHFFaceRecognizer:
                    tfr.faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
                    break; 
            }
            tfr.faceRecognizer.Train(tfr.TrainedFileList.trainedImage.ToArray(), tfr.TrainedFileList.trainedLabelOrder.ToArray());
            return tfr;
        }


        public TrainedFaceRecognizerFromImage SetTrainedFaceRecognizerFromImage(FaceRecognizerType type)
        {

            TrainedFaceRecognizerFromImage tfr = new TrainedFaceRecognizerFromImage();
            TrainedFileListFromImage tfList = new TrainedFileListFromImage();
            tfList.trainedFileName.Add("NA");
            tfList.trainedImage.Add(new Mat());
            tfList.trainedLabelOrder.Add(1);
            tfr.TrainedFileList = tfList;
            switch (type)
            {
                case FaceRecognizerType.EigenFaceRecognizer:
                    tfr.faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
                    break;

                case FaceRecognizerType.FisherFaceRecognizer:
                    tfr.faceRecognizer = new FisherFaceRecognizer(80, 3500);
                    break;

                case FaceRecognizerType.LBPHFFaceRecognizer:
                    tfr.faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
                    break;
            }
            Mat[] ff = new Mat[] {new Mat(),new Mat() };
            Image<Gray, byte> pp = new Image<Gray, byte>(new Size(5,5));
            Image<Gray,byte>[] ppp = new Image<Gray, byte>[] { pp, pp };
            int[] hh = new int[] { 1, 2 };
            List<Mat> eee = new List<Mat>();



            //tfr.faceRecognizer.Train(new Mat(), hh);
            return tfr;
        }





        public faceDetectedObj GetFaceRectangle(Mat emguImage)
        {
            faceDetectedObj fdo = new faceDetectedObj();
            fdo.originalImg = emguImage;
            List<Rectangle> face = new List<Rectangle>();
            
            try
            {
                Mat ugray = new Mat();
                CvInvoke.CvtColor(emguImage, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);//轉灰階
                CvInvoke.EqualizeHist(ugray,ugray);// 均衡灰度
                Rectangle[] faceDetected = FaceCascadeClassifier.DetectMultiScale(ugray, 1.1, 10, new Size(20, 20));
                face.AddRange(faceDetected);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            fdo.FaceRectangles = face;
            return fdo;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FaceCascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
        }

        private void BtnInPut_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog path = new OpenFileDialog();
                if (path.ShowDialog() != DialogResult.OK)
                    return;
                OrgMat = CvInvoke.Imread(path.FileName, Emgu.CV.CvEnum.LoadImageType.AnyColor);
                pictureBox1.Image = OrgMat.Bitmap;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GC.Collect();
        }

        private void BtnDetect_Click(object sender, EventArgs e)
        {
            faceDetectedObj faceDetectedObj = GetFaceRectangle(OrgMat);
            Mat drawMat = OrgMat.Clone();
            foreach (Rectangle rectangle in faceDetectedObj.FaceRectangles)
            {
                CvInvoke.Rectangle(drawMat, rectangle,new MCvScalar(0,0,255),2);
            }
            pictureBox1.Image = drawMat.Bitmap;
            GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           TrainedFaceRecognizer ggg = SetTrainedFaceRecognizer(FaceRecognizerType.EigenFaceRecognizer);
        }
    }
    

 

}
