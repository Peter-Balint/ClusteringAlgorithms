
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clustering.View.Components
{
    /// <summary>
    /// Interaction logic for ResultsSpatial2D.xaml
    /// </summary>
    public partial class ResultsSpatial2D : UserControl
    {
        private Canvas canvas;
        public ResultsSpatial2D()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            canvas = FindVisualChild<Canvas>(PointsItemsControl);
        }

        private static childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
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
                canvas.Background = Brushes.AliceBlue;

                Rect rect = new Rect(canvas.RenderSize);
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
                  (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                rtb.Render(canvas);

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using var stream = File.Create(dialog.FileName);
                encoder.Save(stream);
            }
        }
    }
}
