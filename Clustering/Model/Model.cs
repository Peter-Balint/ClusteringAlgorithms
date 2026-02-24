using Clustering.Model.DataRepresentation;
using Clustering.Model.Algorithm;

namespace Clustering.Model
{
    internal class Model
    {
        private PointCloud pointCloud;
        private IDistanceStrategy distanceStrategy;
        private ParameterSet parameterSet;
        private IAlgorithm algorithm;

        public Model() { }

        public void ResetParameters() { }
    }
}
