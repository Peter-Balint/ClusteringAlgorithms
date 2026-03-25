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

        public bool Visualizable {  get; private set; }

        public MainModel() 
        {
            ParameterSet = new ParameterSet();
        }

        public void ResetParameters() { }
    }
}
