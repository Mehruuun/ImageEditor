using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp1.Interfaces;
using WpfApp1.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Linq;
using ScottPlot;
using ScottPlot.WPF;
using SixLabors.ImageSharp.Formats.Png;
using System.Windows;

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

        private BitmapImage _previewImage;
        public BitmapImage PreviewImage
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

        public ImageEditorViewModel()
        {
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
        }

        private void ResetAdjustments()
        {
            _brightness = 1.0f;
            _contrast = 1.0f;
            _sharpness = 0f;
            ApplyAdjustments();
        }

        private void IncreaseBrightness(object parameter)
        {
            _brightness += 0.1f;
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

            try
            {
                using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(SelectedImage.FullPath))
                {
                    image.Mutate(x =>
                    {
                        x.Brightness(_brightness);
                        x.Contrast(_contrast);
                        if (_sharpness > 0) x.GaussianSharpen(_sharpness);
                    });

                    // استفاده از MemoryStream به جای فایل موقت
                    using (var ms = new MemoryStream())
                    {
                        image.Save(ms, new PngEncoder());
                        ms.Position = 0;

                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        bitmap.Freeze();

                        PreviewImage = bitmap;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        
            }
        }
    


    



   

