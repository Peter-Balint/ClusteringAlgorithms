using Clustering.Model.DataRepresentation;
using Clustering.Model.Algorithm;

namespace Clustering.Model
{
    public class MainModel
    {
        private PointCloud _pointCloud;
        private IDistanceStrategy _distanceStrategy;
        public ParameterSet ParameterSet { get; }
        private IAlgorithm _algorithm;

        public Action ClusteringFinished;

        public event EventHandler<IDataPoint>? PointCreated;

        public bool Visualizable {  get; private set; }

        public MainModel() 
        {
            ParameterSet = new ParameterSet();

            //dummy
            ParameterSet.DisplayMode = DisplayMode.Spatial2D;
            SpatialPoint p = new SpatialPoint([30, 60], 2, 10);
            _pointCloud = new PointCloud([p], CreatePointFactory());
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

        public void ResetParameters() { }


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
    }


}
