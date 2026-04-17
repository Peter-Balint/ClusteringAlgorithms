
using Clustering.Model.DataRepresentation;

namespace Clustering.Model
{
    public class StepSequence
    {
        // storage for each snapshot (static pointcloud) between steps
        private List<IDataPoint[]> _points;
        private List<Cluster[]> _clusters;

        public StepSequence()
        {
            _points = new List<IDataPoint[]>();
            _clusters = new List<Cluster[]>();
        }

        public void SaveStep(IDataPoint[] points, Cluster[] clusters)
        {
            _points.Add(points);
            _clusters.Add(clusters);
        }

        public int GetStepCount()
        {
            return _points.Count;
        }
    }
}
