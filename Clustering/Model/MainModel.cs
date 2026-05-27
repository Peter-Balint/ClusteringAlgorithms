using Clustering.Model.Algorithm;
using Clustering.Model.DataRepresentation;
using Clustering.Model.DimensionalityReduction;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Colourful;
using SharpDX.Mathematics.Interop;


namespace Clustering.Model
{
    public class MainModel
    {
        private PointCloud _pointCloud;
        //may be removed, only to be initialized in the algorithm
        private IDistanceStrategy _distanceStrategy;
        public ParameterSet ParameterSet { get; private set; }
        public ParameterSet? PreviousParameterSet { get; set; }
        private IAlgorithm _algorithm;

        public StepSequence? Results { get; private set; }

        public Action ClusteringFinished;

        public event EventHandler<IDataPoint>? PointCreated;
        public event EventHandler<IDataPoint[]>? PointsCreated;

        public bool ResultsAvailable => Results != null;
        public bool IsRunnable => ParameterSet.InitialClusterNumber !=0 && _pointCloud.PointCount > ParameterSet.InitialClusterNumber;
        private bool _firstParameterSet = true;

        public MainModel() 
        {
            ParameterSet = new ParameterSet();
            PreviousParameterSet = null;
        }

        private Func<double[],int,IDataPoint> CreatePointFactory()
        {
            switch (ParameterSet.DisplayMode)
            {
                case DisplayMode.Spatial2D:
                    {
                        return (coordinates, id) => { return new SpatialPoint(coordinates, 2, id); };
                    }
                case DisplayMode.Spatial3D:
                    {
                        return (coordinates, id) => { return new SpatialPoint(coordinates, 3, id); };
                    }
                case DisplayMode.Image:
                    {
                        return (coordinates, id) => { return new LabColorPoint(coordinates); };
                    }
            }
            return null!;
        }


        public IEnumerable<IDataPoint> GetAllPoints()
        {
            return _pointCloud.GetAllPoints();
        }

        public void AddPoint(double[] coordinates, int clusterId = 0)
        {
            IDataPoint point = _pointCloud.AddPoint(coordinates);
            PointCreated?.Invoke(this,point);
        }
        public void RemovePoint(int id)
        {
            _pointCloud.RemovePoint(id);
        }

        public void NewPointSet(IEnumerable<double[]> pointCoordinates)
        {
            _pointCloud.Reset();
            foreach (double[] point in pointCoordinates)
            {
                _pointCloud.AddPoint(point);
            }
            PointsCreated?.Invoke(this, _pointCloud.GetAllPoints().ToArray());
        }
        public void NewPointSetFromHigherDimension(double[][] pointCoordinates, int toDim)
        {
            PCA.ReduceDimension(pointCoordinates, 2);
            _pointCloud.Reset();
            NewPointSet(pointCoordinates);
        }

        public void NewParameterSet()
        {
            if (_firstParameterSet){
                _firstParameterSet = false;
                PreviousParameterSet = null;
            }
            else
            {
                PreviousParameterSet = ParameterSet.Copy();
            }
        }

        public void Initialize()
        {
            if (ParameterSet.HasBreakingChanges(PreviousParameterSet))
            {
                _pointCloud = new PointCloud(CreatePointFactory());
                _algorithm = new KMeansAlgorithm(ParameterSet);
                if(PreviousParameterSet is null) { PreviousParameterSet = ParameterSet.Copy(); }
            }
        }

        public void RevertParameters()
        {
            if (PreviousParameterSet is null) return;
            ParameterSet = PreviousParameterSet;
            PreviousParameterSet = null;
            _firstParameterSet = true;
        }

        public void RunClustering()
        {
            Results = _algorithm.Run(_pointCloud);
        }

        //todo: move image stuff to its own class when done
        public void LoadImage(string filePath)
        {
            BitmapImage image = new BitmapImage();

            image.BeginInit();
            image.UriSource = new Uri(filePath);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            BitmapSource bitmap = image;

            if (bitmap.Format != PixelFormats.Bgr24)
            {
                bitmap = new FormatConvertedBitmap(
                    bitmap,
                    PixelFormats.Bgr24,
                    null,
                    0);
            }

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;

            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;

            byte[] pixels = new byte[height * stride];
            bitmap.CopyPixels(pixels, stride, 0);

            IColorConverter<RGBColor,LabColor> converter = new ConverterBuilder().FromRGB().ToLab().Build();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * stride + x * bytesPerPixel;

                    byte blue = pixels[index];
                    byte green = pixels[index + 1];
                    byte red = pixels[index + 2];

                    LabColorPoint point = new LabColorPoint(converter.Convert(RGBColor.FromRGB8Bit(red, green, blue)));
                }
            }

        }
    }


}
