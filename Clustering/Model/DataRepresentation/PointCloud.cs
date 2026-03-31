using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.DataRepresentation
{
    public class PointCloud
    {
        public int Dimension { get; private set; }

        // pointId -> point (1to1)
        private Dictionary<int, IDataPoint> _points;

        // pointId -> cluster (nto1)
        private Dictionary<int, Cluster> _pointToClusterMapping;

        //public Type PointType;

        private Func<IDataPoint> CreatePoint;

        public PointCloud(int dimension /*might be unneccessary with the factory function*/,
            Func<IDataPoint> createPoint, IDataPoint[]? initalPoints = null)
        {
            CreatePoint = createPoint;
        }

        public virtual void AddPoint(IDataPoint point, int clusterId = 0)
        {
            //itt is a validation a kérdés, kell-e ellenőrizni a pont dimenzióját?
        }
        public void RemovePoint(int PointId) { }

        public void AddCluster() { }

        //might need to return the points that were left in the cluster and need to be redistributed (which should probably be done in the alg)
        //or have a constraint that only empty clusters can be deleted and redistribution has to happen beforehand
        public void RemoveCluster(int clusterId) { }

        public void ChangeDimensions(int to) { }
    }
}
