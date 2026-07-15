
using Clustering.Model.DataRepresentation;

namespace Clustering.ViewModel
{
    public class SquareViewModel : ViewModelBase, IShape
    {
        private double _offsetX;
        private double _offsetY;
        private double _zoomFactor;

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

        private double _x;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(PutCenterToX));
            }
        }
        public double PutCenterToX => X * _zoomFactor - _offsetX * _zoomFactor - SideLength / 2;

        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(PutCenterToY));
            }
        }
        public double PutCenterToY => Y*_zoomFactor - _offsetY*_zoomFactor - SideLength / 2;

        public SquareViewModel(IDataPoint point, double baseSize, double zoomFactor, double offsetX, double offsetY)
        {
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            SideLength = baseSize/2*zoomFactor;
            _zoomFactor = zoomFactor;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        public void Scale(double offsetX, double offsetY, double zoomFactor, int baseSize)
        {
            _offsetX = offsetX;
            _offsetY = offsetY;
            _zoomFactor = zoomFactor;
            SideLength = baseSize/2 * zoomFactor;
            OnPropertyChanged();
        }
    }
}
