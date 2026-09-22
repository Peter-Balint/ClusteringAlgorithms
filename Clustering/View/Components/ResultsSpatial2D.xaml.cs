
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Clustering.View.Components
{
    /// <summary>
    /// Interaction logic for ResultsSpatial2D.xaml
    /// </summary>
    public partial class ResultsSpatial2D : UserControl
    {
        public ResultsSpatial2D()
        {
            InitializeComponent();
        }

        private void SaveAsImageButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save image",
                Filter = "PNG Image (*.png)|*.png",
                DefaultExt = ".png",
                AddExtension = true
            };
            if (dialog.ShowDialog() == true)
            {
                /*Canvas canvas = (Canvas)PointsItemsControl.ItemsPanel;
                Rect rect = new Rect(MyCanvas.RenderSize);
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
                  (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                rtb.Render(MyCanvas);
                //endcode as PNG
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using var stream = File.Create(dialog.FileName);
                encoder.Save(stream);*/
            }



        }
    }
}
