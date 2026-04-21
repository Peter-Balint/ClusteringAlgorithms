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
        public ParameterSet ParameterSet { get; }
        private IAlgorithm _algorithm;

        public StepSequence? Results { get; private set; }

        public Action ClusteringFinished;

        public event EventHandler<IDataPoint>? PointCreated;
        public event EventHandler<IDataPoint[]>? PointsCreated;

        public bool ResultsAvailable => Results != null;
        public bool IsRunnable => ParameterSet.InitialClusterNumber !=0 && _pointCloud.PointCount > ParameterSet.InitialClusterNumber;

        public MainModel() 
        {
            ParameterSet = new ParameterSet();
        }

        //return the factory method that will be passed to pointcloud
        //if needs be, can rework to assign dimension from parameter as well, unifying spatial point creation, and allowing higher dimensions
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
            //pca, then
            PCA.ReduceDimension(pointCoordinates, 2);
            _pointCloud.Reset();
            NewPointSet(pointCoordinates);
        }

        public void Initialize()
        {
            _pointCloud = new PointCloud(CreatePointFactory());
            _algorithm = new KMeansAlgorithm(ParameterSet);
        }
        public void RunClustering()
        {
            Results = _algorithm.Run(_pointCloud);
        }
    }


}
