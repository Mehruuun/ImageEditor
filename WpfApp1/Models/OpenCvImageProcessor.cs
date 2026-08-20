using OpenCvSharp;
using SixLabors.ImageSharp.Processing.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;

namespace WpfApp1.Models
{
    public class OpenCvImageProcessor : Interfaces.IImageProcessor
    {
        public Mat AdjustBrightness(Mat source, double brightness)
        {
            Mat resul = new Mat();
            Cv2.ConvertScaleAbs(source, resul, alpha: 1.0, beta: brightness);
            return resul;
        }
        public Mat AdjustContrast(Mat source, double contrast)
        {
            Mat result = new Mat();
            double beta = 128 * (1 - contrast);
            Cv2.ConvertScaleAbs(source, result, alpha: contrast, beta: beta);
            return result;
        }
        public Mat Sharpness(Mat source, double amount)
        {
            Mat blur = new Mat();
            Cv2.GaussianBlur(source, blur, new Size(0, 0), 3);
            Mat sharp = new Mat();
            Cv2.AddWeighted(source, 1 + amount, blur, -amount, 0, sharp);
            return sharp;

        }
       public double[] GetGrayscaleValues(Mat source)
        {
            Mat gray = new Mat();
            Cv2.CvtColor(source, gray, ColorConversionCodes.BGR2GRAY);

            gray.GetArray(out byte[] pixelData);
            double[] values = Array.ConvertAll(pixelData, b => (double)b);

            gray.Dispose();
            return values;
        }
    }
    
}
