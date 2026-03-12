using Clustering.Model.DataRepresentation;
using Clustering.Model.Algorithm;

namespace Clustering.Model
{
    public class MainModel
    {
        private PointCloud _pointCloud;
        private IDistanceStrategy _distanceStrategy;
        private ParameterSet _parameterSet;
        private IAlgorithm _algorithm;

        public MainModel() 
        {
        }

        public void ResetParameters() { }
    }
}
