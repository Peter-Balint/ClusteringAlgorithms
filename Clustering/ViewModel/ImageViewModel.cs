
using Clustering.Model.DataRepresentation;
using Colourful;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clustering.ViewModel
{
    public class ImageViewModel
    {
        public BitmapSource Source { get; }

        public ImageViewModel(IDataPoint[] points, Cluster[] clusters, int imageWidth, int imageHeight, IColorConverter<LabColor, RGBColor> converter)
        {
            byte[] pixels = new byte[points.Length*3];
            RGBColor[] colors = new RGBColor[clusters.Length];
            int index = 0;
            clusters = clusters.OrderBy(c => c.Id).ToArray();
            for(int i=0;i<clusters.Length;i++)
            {
                LabColor labColor = new LabColor(clusters[i].CenterPoint.GetAllCoordinates());
                colors[i] = converter.Convert(labColor);
            }
            foreach(IDataPoint point in points)
            {
                /*LabColor labColor = new LabColor(point.GetAllCoordinates());
                RGBColor rgbColor = converter.Convert(labColor);
                pixels[index] = (byte)Math.Clamp(rgbColor.R*255,0,255);
                pixels[index+1] = (byte)Math.Clamp(rgbColor.G*255, 0, 255);
                pixels[index+2] = (byte)Math.Clamp(rgbColor.B*255, 0, 255);*/
                RGBColor rgbColor = colors[point.ClusterId];
                pixels[index] = (byte)Math.Clamp(rgbColor.R * 255, 0, 255);
                pixels[index + 1] = (byte)Math.Clamp(rgbColor.G * 255, 0, 255);
                pixels[index + 2] = (byte)Math.Clamp(rgbColor.B * 255, 0, 255);
                index += 3;
            }

            Source = BitmapSource.Create(
                imageWidth,
                imageHeight,
                96,
                96,
                PixelFormats.Rgb24,
                null,
                pixels,
                imageWidth * 3);

        }


    }
}
