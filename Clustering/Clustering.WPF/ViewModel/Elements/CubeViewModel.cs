
using Clustering.Model.DataRepresentation;

namespace Clustering.WPF.ViewModel
{
    internal class CubeViewModel : Shape3DViewModelBase
    {
        public CubeViewModel(IDataPoint point)
        {
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            Z = point.GetCoordinateAt(2);
            Id = point.Id;
        }
    }
}
