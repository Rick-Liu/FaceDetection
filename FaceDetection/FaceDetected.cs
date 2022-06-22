using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDetection
{
    class FaceDetected
    {
        public FaceDetected()
        {
            FaceCascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            FaceRecognizer = new FaceRecognizerFromImage();
            FaceRecognizer.faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            FaceRecognizer.TrainedFileList = new FaceRecognizerListFromImage();
        }
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
        public class FaceRecognizerListFromImage
        {
            public List<Image<Gray, byte>> trainedImage = new List<Image<Gray, byte>>();
            public List<int> trainedLabelOrder = new List<int>();
            public List<string> trainedFileName = new List<string>();
        }
        public class FaceRecognizerFromImage
        {
            public FaceRecognizer faceRecognizer;
            public FaceRecognizerListFromImage TrainedFileList;
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


        public FaceRecognizerFromImage FaceRecognizer;
        private CascadeClassifier FaceCascadeClassifier;

        public Mat DetectedFace(Mat ScrMat)
        {
            faceDetectedObj faceDetectedObj = GetFaceRectangle(ScrMat);
            Mat drawMat = ScrMat.Clone();
            Mat currentFaceMat = new Mat(); 
            foreach (Rectangle rectangle in faceDetectedObj.FaceRectangles)
            {
                CvInvoke.Rectangle(drawMat, rectangle, new MCvScalar(0, 0, 255), 2);
                if (currentFaceMat != null)
                    currentFaceMat.Dispose();
                currentFaceMat = new Mat(OrgMat, rectangle);
                CvInvoke.CvtColor(currentFaceMat, currentFaceMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);//轉灰階
                CvInvoke.EqualizeHist(currentFaceMat, currentFaceMat);// 均衡灰度
                CvInvoke.Resize(currentFaceMat, currentFaceMat, new Size(100, 100));
                FaceRecognizer.PredictionResult rrr = FaceRecognizer.faceRecognizer.Predict(currentFaceMat);
                if (rrr.Distance < 500)
                    CvInvoke.PutText(drawMat, FaceRecognizer.TrainedFileList.trainedFileName[rrr.Label], new Point(rectangle.X, rectangle.Y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3, new MCvScalar(0, 0, 255), 5);
                else
                {
                    CvInvoke.PutText(drawMat, "UnKnow", new Point(rectangle.X, rectangle.Y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3, new MCvScalar(0, 0, 255), 5);
                }
            }
            return 
        }

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

        public FaceRecognizerFromImage SetTrainedFaceRecognizerFromImage(FaceRecognizerType type, Mat srcMat, string name)
        {

            FaceRecognizerFromImage tfr = new FaceRecognizerFromImage();
            FaceRecognizerListFromImage tfList = new FaceRecognizerListFromImage();
            if (FaceRecognizer.TrainedFileList == null || FaceRecognizer.TrainedFileList.trainedLabelOrder.Count == 0)
            {
                tfList.trainedFileName.Add(name);
                tfList.trainedImage.Add(srcMat.ToImage<Gray, byte>());
                tfList.trainedLabelOrder.Add(0);
            }
            else
            {
                tfList = FaceRecognizer.TrainedFileList;
                tfList.trainedFileName.Add(name);
                tfList.trainedImage.Add(srcMat.ToImage<Gray, byte>());
                tfList.trainedLabelOrder.Add(tfList.trainedLabelOrder.Count());
            }
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

            tfr.faceRecognizer.Train(tfr.TrainedFileList.trainedImage.ToArray(), tfr.TrainedFileList.trainedLabelOrder.ToArray());
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
                CvInvoke.EqualizeHist(ugray, ugray);// 均衡灰度
                Rectangle[] faceDetected = FaceCascadeClassifier.DetectMultiScale(ugray, 1.1, 10, new Size(20, 20));
                face.AddRange(faceDetected);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            fdo.FaceRectangles = face;
            return fdo;
        }

        public void SaveTrainedFaceRecognizer()
        {
            FaceRecognizer.faceRecognizer.Save("POIUY");
            StreamWriter sr = new StreamWriter("Test.txt");

            for (int i = 0; i < FaceRecognizer.TrainedFileList.trainedLabelOrder.Count; i++)
            {
                sr.WriteLine(FaceRecognizer.TrainedFileList.trainedLabelOrder[i] + "," + FaceRecognizer.TrainedFileList.trainedFileName[i]);
            }
            sr.Close();
        }
        public void LoadTrainedFaceRecognizer()
        {
            FaceRecognizer.faceRecognizer.Load("POIUY");
            StreamReader sr = new StreamReader("Test.txt");
            string[] str;
            while (!sr.EndOfStream)
            {
                str = sr.ReadLine().Split(',');
                FaceRecognizer.TrainedFileList.trainedLabelOrder.Add(Convert.ToInt32(str[0]));
                FaceRecognizer.TrainedFileList.trainedFileName.Add(str[1]);
            }
            sr.Close();
        }
    }
}
