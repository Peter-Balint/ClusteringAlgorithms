using Clustering.Model.Algorithm;
using Clustering.Model.DataRepresentation;
using Clustering.Model.DimensionalityReduction;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Colourful;


namespace Clustering.Model
{
    public class MainModel
    {
        private PointCloud _pointCloud;
        public ImageHandler ImageHandler { get; }
        public ParameterSet ParameterSet { get; private set; }
        public ParameterSet? PreviousParameterSet { get; set; }

        private IAlgorithm _algorithm;

        private CancellationTokenSource _tokenSource;

        public StepSequence? Results { get; private set; }

        public event EventHandler ClusteringFinished;
        public event EventHandler<int>? StepFinished;

        public event EventHandler<IDataPoint>? PointCreated;
        public event EventHandler<IDataPoint[]>? PointsCreated;
        public event EventHandler<IDataPoint>? CenterCreated;

        public bool ResultsAvailable => Results != null;

        private List<IDataPoint> _handPickedInitialCenters;
        public bool IsRunnable => ParameterSet.InitialClusterNumber !=0 && _pointCloud.PointCount > ParameterSet.InitialClusterNumber 
            && (ParameterSet.ClusterInitializationMethod == ClusterInitializationMethod.RandomDataPoint || _handPickedInitialCenters.Count >= ParameterSet.InitialClusterNumber);
        
        private bool _firstParameterSet = true;

        public MainModel() 
        {
            ParameterSet = new ParameterSet();
            PreviousParameterSet = null;
            ImageHandler = new ImageHandler();
            _tokenSource = new CancellationTokenSource();
            _handPickedInitialCenters = [];
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
                        return (coordinates, id) => { return new LabColorPoint(coordinates, id); };
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

        public void AddCenter(double[] coordinates)
        {
            _handPickedInitialCenters.Add(CreatePointFactory().Invoke(coordinates, -1));
            CenterCreated?.Invoke(this, _handPickedInitialCenters.Last());
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
                _algorithm.StepFinished += (_,progress) => StepFinished?.Invoke(this,progress);
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

        public async void RunClustering()
        {
            Results = await Task.Run(()=>_algorithm.Run(_pointCloud, _tokenSource.Token, _handPickedInitialCenters));
            ClusteringFinished?.Invoke(this, EventArgs.Empty);
        }

        public void StopClustering()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }
 
        public void LoadImage(string filePath)
        {
            ImageHandler.LoadImage(filePath, _pointCloud);
        }

    }


}
