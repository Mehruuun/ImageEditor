using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.Interfaces;
using WpfApp1.Models;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using ScottPlot;
namespace WpfApp1.ViewModels
  
{
    public class ImageEditorViewModel : INotifyPropertyChanged
    {
     
        public ObservableCollection<ImageItem> Images { get; set; }

        private ImageItem _selectedImage;
        public ImageItem SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
                ResetAdjustments();
            }
        }

        private float _brightness = 1.0f;
        private float _contrast = 1.0f;
        private float _sharpness = 0f;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _previewImage;
        public ImageSource PreviewImage
        {
            get => _previewImage;
            set
            {
                _previewImage = value;
                OnPropertyChanged();
            }
        }

        public ICommand BrightnessCommand { get; set; }
        public ICommand ContrastCommand { get; set; }
        public ICommand SharpeningCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly IImageProcessor _imageProcessor;
        public ImageEditorViewModel()
        {
            _imageProcessor = new OpenCvImageProcessor();

            Images = new ObservableCollection<ImageItem>();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleImages");
            LoadImagesFromFolder(folderPath);

            BrightnessCommand = new RelayCommand(IncreaseBrightness);
            ContrastCommand = new RelayCommand(IncreaseContrast);
            SharpeningCommand = new RelayCommand(IncreaseSharpness);
        }

        private void LoadImagesFromFolder(string folderPath)
        {
            Images.Clear();
            string[] files = Directory.GetFiles(folderPath, "*.png")
                .Concat(Directory.GetFiles(folderPath, "*.PNG"))
                .ToArray();

            foreach (string file in files)
            {
                Images.Add(new ImageItem
                {
                    FileName = Path.GetFileName(file),
                    FullPath = file
                });
            }

            // 🔽 انتخاب اولین تصویر به‌عنوان پیش‌فرض
            if (Images.Any())
            {
                SelectedImage = Images.First();
            }

            
        }

        private void ResetAdjustments()
        {
            _brightness = 0f;
            _contrast = 1.0f;
            _sharpness = 0f;
            ApplyAdjustments();
        }

        private void IncreaseBrightness(object parameter)
        {
            _brightness += 10f;
            ApplyAdjustments();
        }

        private void IncreaseContrast(object parameter)
        {
            _contrast += 0.1f;
            ApplyAdjustments();
        }

        private void IncreaseSharpness(object parameter)
        {
            _sharpness += 1f;
            ApplyAdjustments();
        }

        private void ApplyAdjustments()
        {
            if (SelectedImage == null) return;
            using (var org = Cv2.ImRead(SelectedImage.FullPath))
            {
                Mat bright = _imageProcessor.AdjustBrightness(org, _brightness);
                Mat cont=_imageProcessor.AdjustContrast(bright, _contrast);
                Mat sharp =  _imageProcessor.Sharpness(cont, _sharpness);
                var bitmap = sharp.ToBitmapSource();
                bitmap.Freeze();
                PreviewImage = bitmap;
                HistogramValues = _imageProcessor.GetGrayscaleValues(sharp);

                bright.Dispose();
                cont.Dispose();
                sharp.Dispose();
            }
           
        }
        private double[] _histogramValues;
        public double[] HistogramValues
        {
            get => _histogramValues;
            set
            {
                _histogramValues = value;
                OnPropertyChanged();
            }
        }

    }
}