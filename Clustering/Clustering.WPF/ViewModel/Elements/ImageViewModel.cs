
using Clustering.Model.DataRepresentation;
using Colourful;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clustering.ViewModel
{
    public class ImageViewModel
    {
        public BitmapSource Source { get; }

        public ImageViewModel(int imageWidth, int imageHeight, IColorConverter<LabColor, RGBColor> converter, IDataPoint[] points, Cluster[]? clusters = null)
        {
            byte[] pixels = new byte[points.Length*3];

            if (clusters is null)
            {
                SetPixelsConvertOnly(pixels, points, imageWidth, imageHeight, converter);
            }
            else
            {
                SetPixelsWithClusters(pixels, points, clusters, imageWidth, imageHeight, converter);
            }

            Source = CreateBitmapSource(imageWidth, imageHeight, pixels);
        }
        public ImageViewModel(BitmapSource ImageSource)
        {
            Source = ImageSource;
        }

        private BitmapSource CreateBitmapSource(int imageWidth, int imageHeight, byte[] pixels)
        {
            return BitmapSource.Create(
                imageWidth,
                imageHeight,
                96,
                96,
                PixelFormats.Rgb24,
                null,
                pixels,
                imageWidth * 3);
        }

        private void SetPixelsWithClusters(byte[] pixels, IDataPoint[] points, Cluster[] clusters, int imageWidth, int imageHeight, IColorConverter<LabColor, RGBColor> converter)
        {

            RGBColor[] colors = new RGBColor[clusters.Length];
            clusters = clusters.OrderBy(c => c.Id).ToArray();

            for (int i = 0; i < clusters.Length; i++)
            {
                LabColor labColor = new LabColor(clusters[i].CenterPoint.GetAllCoordinates());
                colors[i] = converter.Convert(labColor);
            }

            int index = 0;
            foreach (IDataPoint point in points)
            {

                RGBColor rgbColor = colors[point.ClusterId];
                pixels[index] = (byte)Math.Clamp(rgbColor.R * 255, 0, 255);
                pixels[index + 1] = (byte)Math.Clamp(rgbColor.G * 255, 0, 255);
                pixels[index + 2] = (byte)Math.Clamp(rgbColor.B * 255, 0, 255);
                index += 3;
            }
        }
        private void SetPixelsConvertOnly(byte[] pixels, IDataPoint[] points, int imageWidth, int imageHeight, IColorConverter<LabColor, RGBColor> converter)
        {
            int index = 0;
            foreach (IDataPoint point in points)
            {
                LabColor labColor = new LabColor(point.GetAllCoordinates());
                RGBColor rgbColor = converter.Convert(labColor);
                pixels[index] = (byte)Math.Clamp(rgbColor.R * 255, 0, 255);
                pixels[index + 1] = (byte)Math.Clamp(rgbColor.G * 255, 0, 255);
                pixels[index + 2] = (byte)Math.Clamp(rgbColor.B * 255, 0, 255);
                index += 3;
            }
        }

    }
}
