
using Clustering.Model.DataRepresentation;

namespace Clustering.ViewModel
{
    internal class SquareViewModel : Shape2DViewModelBase
    {
        private double _sideLength;
        public double SideLength
        {
            get => _sideLength;
            set
            {
                _sideLength = value;
                OnPropertyChanged(nameof(SideLength));
            }
        }

        public override double PutCenterToX => X * _zoomFactor - _offsetX * _zoomFactor - SideLength / 2;
        public override double PutCenterToY => Y*_zoomFactor - _offsetY*_zoomFactor - SideLength / 2;

        public SquareViewModel(IDataPoint point, double baseSize, double zoomFactor, double offsetX, double offsetY)
        {
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            SideLength = baseSize/2*zoomFactor;
            _zoomFactor = zoomFactor;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        public override void Scale(double offsetX, double offsetY, double zoomFactor, int baseSize)
        {
            _offsetX = offsetX;
            _offsetY = offsetY;
            _zoomFactor = zoomFactor;
            SideLength = baseSize/2 * zoomFactor;
            OnPropertyChanged();
        }
    }
}
