using Clustering.Model.DataRepresentation;

namespace Clustering.Model.Algorithm
{
    internal interface IAlgorithm
    {
        public StepSequence Run(PointCloud pointCloud, CancellationToken ct);
        event EventHandler<int> StepFinished;
    }
}
