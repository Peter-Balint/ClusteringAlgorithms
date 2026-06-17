
using Clustering.Model.DataRepresentation;
using Colourful;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clustering.Model
{
    //handles the loading, conversion, and properties of the initial image, but doesn't oversee the clustering process
    public class ImageHandler
    {
        public BitmapSource? ImageSource { get; private set; }
        public int ImageHeight { get; private set; }
        public int ImageWidth { get; private set; }

        public event EventHandler? InitialImageLoaded;

        internal void LoadImage(string filePath, PointCloud pointCloud)
        {
            BitmapImage _image = new BitmapImage();

            _image.BeginInit();
            _image.UriSource = new Uri(filePath);
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _image.EndInit();
            _image.Freeze();

            ImageSource = _image;

            if (ImageSource.Format != PixelFormats.Rgb24)
            {
                ImageSource = new FormatConvertedBitmap(
                    ImageSource,
                    PixelFormats.Rgb24,
                    null,
                    0);
            }

            InitialImageLoaded?.Invoke(this, EventArgs.Empty);

            int width = ImageSource.PixelWidth; ImageWidth = width;
            int height = ImageSource.PixelHeight; ImageHeight = height;

            int bytesPerPixel = (ImageSource.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;

            byte[] pixels = new byte[height * stride];
            ImageSource.CopyPixels(pixels, stride, 0);

            IColorConverter<RGBColor, LabColor> converter = new ConverterBuilder().FromRGB().ToLab().Build();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x * bytesPerPixel;

                    byte red = pixels[index];
                    byte green = pixels[index + 1];
                    byte blue = pixels[index + 2];

                    pointCloud.AddPoint(converter.Convert(RGBColor.FromRGB8Bit(red, green, blue)).Vector);
                }
            }
        }
    }
}
