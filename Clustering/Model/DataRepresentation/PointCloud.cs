
namespace Clustering.Model.DataRepresentation
{
    public class PointCloud
    {
        // pointId -> point (1to1)
        private Dictionary<int, IDataPoint> _points;

        //clusters still kept in pointcloud for future user interaction
        private List<Cluster> _clusters;


        private Func<double[], int, IDataPoint> CreatePoint;

        private int _newId = 0;

        public int PointCount => _points.Count;

        public PointCloud(Func<double[], int, IDataPoint> createPoint)
        {
            _points = new Dictionary<int, IDataPoint>(256); //to skip frequent early capacity increases, without too much space wasted for smaller dicts
            CreatePoint = createPoint;
            _clusters = new List<Cluster>();
        }
        //dummy constructor for testing
        public PointCloud(IDataPoint[] initalPoints, Func<double[], int, IDataPoint> createPoint)
        {
            _points = new Dictionary<int, IDataPoint>();
            foreach (IDataPoint point in initalPoints)
            {
                _points.Add(point.Id, point);
            }
            _clusters = new List<Cluster>();
            CreatePoint = createPoint;
        }

        public IDataPoint AddPoint(double[] coordinates, int clusterId = 0)
        {
            IDataPoint point = CreatePoint(coordinates, GetNewId());
            _points.Add(point.Id, point);
            return point;
        }
        public void RemovePoint(int PointId) 
        { 
            _points.Remove(PointId);
        }

        public void AddCluster(int id, IDataPoint? centerPoint = null)
        {
            _clusters.Add(new Cluster(id,centerPoint));
        }

        public Func<double[], int, IDataPoint> GetPointFactoryMethod()
        {
            return CreatePoint;
        }

        //might need to return the points that were left in the cluster and need to be redistributed (which should probably be done in the alg)
        //or have a constraint that only empty clusters can be deleted and redistribution has to happen beforehand
        //public void RemoveCluster(int clusterId) { }

        public void ChangeDimensions(int to) { }

        private int GetNewId()
        {
            int newId = _newId;
            _newId++;
            return newId;
        }
        public int[] GetIds()
        {
            return _points.Keys.ToArray();
        }

        public IEnumerable<IDataPoint> GetAllPoints()
        {
            return _points.Values;
        }
        public IDataPoint GetPoint(int id)
        {
            return _points[id];
        }
        public IEnumerable<Cluster> GetClusters()
        {
            return _clusters;
        }
    }
}
