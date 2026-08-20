using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.ViewModels;
using Telerik.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Design;
using System.ComponentModel;


namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for ImageEditor.xaml
    /// </summary>
    public partial class ImageEditor : Window
    {
        public ImageEditor()
        {


            
                InitializeComponent();

                var vm = new ImageEditorViewModel();
                DataContext = vm;

                vm.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(ImageEditorViewModel.HistogramValues))
                {
                    var vm = (ImageEditorViewModel)sender;
                    if (vm.HistogramValues == null) return;

                    var hist = ScottPlot.Statistics.Histogram.WithBinSize(1, vm.HistogramValues);
                    HistogramPlot.Plot.Clear();
                    HistogramPlot.Plot.Add.Bars(hist.Bins, hist.Counts);
                    HistogramPlot.Refresh();
                }
            }
        }
    
}
   

    

