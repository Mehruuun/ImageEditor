using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace WpfApp1.Interfaces
{
    public interface IImageProcessor
    {
        Mat AdjustBrightness(Mat source, double brightness);
        Mat AdjustContrast(Mat source, double contrast);
        Mat Sharpness(Mat source, double amount);
        double[] GetGrayscaleValues(Mat source);


    }
}
