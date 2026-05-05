using Clustering.Model.DataRepresentation;
using Clustering.Model.Algorithm;
using Clustering.Model.DimensionalityReduction;

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
                case DisplayMode.RGBA:
                    {
                        return (coordinates, id) => { return new RGBAPoint(coordinates); };
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

        public void RunClustering()
        {
            Results = _algorithm.Run(_pointCloud);
        }
    }


}
