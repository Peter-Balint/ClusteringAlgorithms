
using Clustering.Model.DataRepresentation;

namespace Clustering.ViewModel
{
    public class SquareViewModel : ViewModelBase, IShape
    {
        private double _offsetX;
        private double _offsetY;

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
        public double PutCenterToX => X + _offsetX - SideLength / 2;

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
        public double PutCenterToY => Y + _offsetY - SideLength / 2;

        public SquareViewModel(IDataPoint point, double sideLength, double offsetX = 0, double offsetY = 0)
        {
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            SideLength = sideLength;
            _offsetX = offsetX;
            _offsetY = offsetY;
        }

        public void ScaleOffset(double offsetX, double offsetY)
        {
            _offsetX += offsetX;
            _offsetY += offsetY;
            OnPropertyChanged();
        }
    }
}
