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
            public Emgu.CV.Face.FaceRecognizer faceRecognizer;
            public TrainedFileList TrainedFilelist;
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
        public TrainedFileList SetSampleFacesList()
        {
            TrainedFileList tf = new TrainedFileList();
            DirectoryInfo di = new DirectoryInfo("hhhh");
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




    }
    

 

}
