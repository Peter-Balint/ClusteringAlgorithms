
using Clustering.Model.DataRepresentation;
using System.Collections.Immutable;

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

        public void SaveStepCopy(IDataPoint[] points, Cluster[] clusters)
        {
            IDataPoint[] copiedPoints = new IDataPoint[points.Length];
            for(int i=0; i<points.Length; i++)
            {
                copiedPoints[i] = points[i].DeepCopy();
            }
            _points.Add(copiedPoints);

            Cluster[] copiedClusters = new Cluster[clusters.Length];
            for(int i=0; i<clusters.Length; i++)
            {
                copiedClusters[i] = new Cluster(clusters[i].Id, clusters[i].CenterPoint.DeepCopy());
            }
            _clusters.Add(copiedClusters);
        }

        public int GetStepCount()
        {
            return _points.Count;
        }

        public (IDataPoint[],Cluster[]) GetStepAt(int stepIndex)
        {
            return (_points.ElementAt(stepIndex),_clusters.ElementAt(stepIndex));
        }
    }
}
