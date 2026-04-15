using Clustering.Model.DataRepresentation;

namespace Clustering.Model.Algorithm
{
    internal interface IAlgorithm
    {
        public StepSequence Run(PointCloud pointCloud);
    }
}
