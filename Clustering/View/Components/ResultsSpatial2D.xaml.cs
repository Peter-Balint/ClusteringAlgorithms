
using Clustering.ViewModel;
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
        private ResultsSpatial2DViewModel _viewModel => (ResultsSpatial2DViewModel)DataContext;
        private Canvas canvas;
        public ResultsSpatial2D()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not ResultsSpatial2DViewModel vm)
                return;

            DataContextChanged -= OnDataContextChanged;
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
                Rect rect = new Rect(canvas.RenderSize);
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
                  (int)rect.Bottom, 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas);

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(rtb));

                using var stream = File.Create(dialog.FileName);
                encoder.Save(stream);
            }
        }

        private void SaveAsImageSequenceButton_Click(object? sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save image",
                Filter = "PNG Image (*.png)|*.png",
                AddExtension = false
            };
            if(dialog.ShowDialog() == true)
            {
                _viewModel.IsInteractible = false;
                int originalStep = _viewModel.CurrentStep;

                string directory = Path.GetDirectoryName(dialog.FileName)!;
                string baseName = Path.GetFileNameWithoutExtension(dialog.FileName);

                for (int i = 0; i < _viewModel.StepCount; i++)
                {
                    _viewModel.GoToStep(i);
                    Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
                    Rect rect = new Rect(canvas.RenderSize);
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
                      (int)rect.Bottom, 96d, 96d, PixelFormats.Default);
                    rtb.Render(canvas);

                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));

                    string fileName = Path.Combine(directory, $"{baseName}{i:D3}.png");

                    using var stream = File.Create(fileName);
                    encoder.Save(stream);
                }

                _viewModel.GoToStep(originalStep);
                _viewModel.IsInteractible = true;
            }

        }
    }
}
