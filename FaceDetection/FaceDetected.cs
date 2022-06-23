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
        public FaceDetected(FaceRecognizerType type)
        {
            FaceCascadeClassifier = new CascadeClassifier("haarcascade_frontalface_default.xml");
            FaceRecognizer = new FaceRecognizerFromImage();
            switch (type)
            {
                case FaceRecognizerType.EigenFaceRecognizer:
                    FaceRecognizer.faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
                    break;

                case FaceRecognizerType.FisherFaceRecognizer:
                    FaceRecognizer.faceRecognizer = new FisherFaceRecognizer(80, 3500);
                    break;

                case FaceRecognizerType.LBPHFFaceRecognizer:
                    FaceRecognizer.faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
                    break;
            }


            FaceRecognizer.TrainedFileList = new FaceRecognizerListFromImage();
            CurrentFaceList = new List<Mat>();
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
        public List<Mat> CurrentFaceList;
        public Mat _CurrentFace;
        private CascadeClassifier FaceCascadeClassifier;

        public Mat DetectedFace(Mat ScrMat,Size size,int distance =500)
        {
            faceDetectedObj faceDetectedObj = GetFaceRectangle(ScrMat);
            Mat drawMat = ScrMat.Clone();
            Mat currentFaceMat = new Mat();
            CurrentFaceList.Clear();
            foreach (Rectangle rectangle in faceDetectedObj.FaceRectangles)
            {
                CvInvoke.Rectangle(drawMat, rectangle, new MCvScalar(0, 0, 255), 2);
                if (currentFaceMat != null)
                    currentFaceMat.Dispose();
                currentFaceMat = new Mat(ScrMat, rectangle);
                CvInvoke.CvtColor(currentFaceMat, currentFaceMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);//轉灰階
                CvInvoke.EqualizeHist(currentFaceMat, currentFaceMat);// 均衡灰度
                CvInvoke.Resize(currentFaceMat, currentFaceMat, size);
                CurrentFaceList.Add(currentFaceMat.Clone());
                double PassDistance = Int32.MaxValue;
                int predictionResultLab = -1;
                _CurrentFace = currentFaceMat.Clone();
                if (FaceRecognizer.faceRecognizer.Ptr.ToInt32() != 0)
                {
                    FaceRecognizer.PredictionResult predictionResult = FaceRecognizer.faceRecognizer.Predict(currentFaceMat);
                    PassDistance = predictionResult.Distance;
                    predictionResultLab = predictionResult.Label;
                }
                if(PassDistance < distance)
                    CvInvoke.PutText(drawMat, FaceRecognizer.TrainedFileList.trainedFileName[predictionResultLab], new Point(rectangle.X, rectangle.Y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3, new MCvScalar(0, 0, 255), 5);
                else
                    CvInvoke.PutText(drawMat,"UnKnow", new Point(rectangle.X, rectangle.Y), Emgu.CV.CvEnum.FontFace.HersheyPlain, 3, new MCvScalar(0, 0, 255), 5);
            }
            GC.Collect();
            return drawMat;
        }

        public List<Mat> SearchFace(Mat ScrMat,Size size)
        {
            faceDetectedObj faceDetectedObj = GetFaceRectangle(ScrMat);
            Mat drawMat = ScrMat.Clone();
            Mat currentFaceMat = new Mat();
            CurrentFaceList.Clear();
            foreach (Rectangle rectangle in faceDetectedObj.FaceRectangles)
            {
                CvInvoke.Rectangle(drawMat, rectangle, new MCvScalar(0, 0, 255), 2);
                if (currentFaceMat != null)
                    currentFaceMat.Dispose();
                currentFaceMat = new Mat(ScrMat, rectangle);
                CvInvoke.CvtColor(currentFaceMat, currentFaceMat, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);//轉灰階
                CvInvoke.EqualizeHist(currentFaceMat, currentFaceMat);// 均衡灰度
                CvInvoke.Resize(currentFaceMat, currentFaceMat, size);
                CurrentFaceList.Add(currentFaceMat.Clone());
            }
            GC.Collect();
            return CurrentFaceList;
        }
 

        public FaceRecognizerListFromImage SetImageFacesList()
        {
            FaceRecognizerListFromImage tf = new FaceRecognizerListFromImage();
            DirectoryInfo di = new DirectoryInfo("TrainedFace");
            int i = 0;
            foreach (FileInfo fi in di.GetFiles())
            {
                if(Path.GetExtension(fi.Name)==".bmp")
                {
                    tf.trainedImage.Add(new Image<Gray, byte>(fi.FullName));
                    tf.trainedLabelOrder.Add(i);
                    tf.trainedFileName.Add(fi.Name.Split('_')[1].Split('.')[0]);
                    i++;
                }             
            }
            return tf;
        }

        public void SetTrainedFaceRecognizerFromImage(FaceRecognizerType type, Mat srcMat, string name)
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
            int[] singalListLable =new int[] { tfList.trainedLabelOrder.Count()-1 };
            tfr.faceRecognizer.Train(tfr.TrainedFileList.trainedImage.ToArray(), tfr.TrainedFileList.trainedLabelOrder.ToArray());
            FaceRecognizer = tfr;
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
            CreateFolder();
            if (FaceRecognizer.faceRecognizer.Ptr.ToInt32() != 0)
            {
                FaceRecognizer.faceRecognizer.Save("TrainedFace\\Recognizer");
                //StreamWriter sr = new StreamWriter("TrainedFace\\RecognizerList.txt");

                for (int i = 0; i < FaceRecognizer.TrainedFileList.trainedLabelOrder.Count; i++)
                {
                    //sr.WriteLine(FaceRecognizer.TrainedFileList.trainedLabelOrder[i] + "," + FaceRecognizer.TrainedFileList.trainedFileName[i]);
                    CvInvoke.Imwrite("TrainedFace\\"+i.ToString() + "_" + FaceRecognizer.TrainedFileList.trainedFileName[i]+".bmp", FaceRecognizer.TrainedFileList.trainedImage[i]);
                }
                //sr.Close();
                //FaceRecognizer.TrainedFileList.trainedImage
                    
            }
            else
            {
                File.Delete("TrainedFace\\Recognizer");
                File.Delete("TrainedFace\\RecognizerList.txt");
            }
            
        }
        public void LoadTrainedFaceRecognizer()
        {
            FaceRecognizer.TrainedFileList = SetImageFacesList();
            CreateFolder();
            if (!File.Exists("TrainedFace\\Recognizer")|| !File.Exists("TrainedFace\\RecognizerList.txt"))
            {
                FaceRecognizer.faceRecognizer.Dispose();
                return;
            }
            FaceRecognizer.faceRecognizer.Load("TrainedFace\\Recognizer");
            //StreamReader sr = new StreamReader("TrainedFace\\RecognizerList.txt");
            //string[] str;
            //while (!sr.EndOfStream)
            //{
            //    str = sr.ReadLine().Split(',');
            //    FaceRecognizer.TrainedFileList.trainedLabelOrder.Add(Convert.ToInt32(str[0]));
            //    FaceRecognizer.TrainedFileList.trainedFileName.Add(str[1]);
            //}
            //sr.Close();
        }

        public void ClearTrainedFaceRecognizer()
        {
            FaceRecognizer.faceRecognizer.Dispose();
            //FaceRecognizer.faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            FaceRecognizer.TrainedFileList.trainedFileName.Clear();
            FaceRecognizer.TrainedFileList.trainedImage.Clear();
            FaceRecognizer.TrainedFileList.trainedLabelOrder.Clear();
        }

        private void CreateFolder()
        {
            if(!Directory.Exists("TrainedFace"))
                Directory.CreateDirectory("TrainedFace");
        }
    }
}
