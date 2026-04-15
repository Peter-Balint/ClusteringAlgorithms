

using Clustering.Model.DataRepresentation;
using System.Windows.Markup.Localizer;

namespace Clustering.Model.Algorithm
{
    internal class KMeansAlgorithm : IAlgorithm
    {
        private readonly ParameterSet _parameters;
        private IDistanceStrategy _distanceStrategy;

        public KMeansAlgorithm(ParameterSet parameters)
        {
            _parameters = parameters;
            if (parameters.DisplayMode == DisplayMode.Spatial2D || parameters.DisplayMode == DisplayMode.Spatial3D)
            {
                switch (parameters.SpatialDistanceMetric)
                {
                    case SpatialDistanceMetric.Euclidean:
                        {
                            _distanceStrategy = new EuclideanDistance();
                            break;
                        }
                    case SpatialDistanceMetric.Manhattan:
                        {
                            _distanceStrategy = new ManhattanDistance();
                            break;
                        }
                    case SpatialDistanceMetric.Sup:
                        {
                            _distanceStrategy = new SupDistance();
                            break;
                        }
                }
            }
            else _distanceStrategy = new RGBADistance();
        }

        public StepSequence Run(PointCloud pointCloud)
        {
            throw new NotImplementedException();
        }
    }
}
