
using Clustering.Model.DataRepresentation;

namespace Clustering.WPF.ViewModel
{
    internal class SphereViewModel : Shape3DViewModelBase
    {
        public int ClusterId { get; }

        public SphereViewModel(IDataPoint point)
        {
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            Z = point.GetCoordinateAt(2);
            Id = point.Id;
            ClusterId = point.ClusterId;
        }
    }
}
