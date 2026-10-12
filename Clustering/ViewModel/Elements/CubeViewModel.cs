
using Clustering.Model.DataRepresentation;

namespace Clustering.ViewModel
{
    internal class CubeViewModel
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }
        public int Id { get; }

        public CubeViewModel(IDataPoint point)
        {
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            Z = point.GetCoordinateAt(2);
            Id = point.Id;
        }
    }
}
