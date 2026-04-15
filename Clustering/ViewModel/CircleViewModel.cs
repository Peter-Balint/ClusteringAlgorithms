
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    class CircleViewModel : ViewModelBase
    {
        private double _offsetX;
        private double _offsetY;

        private double _diameter;
        public double Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value;
                OnPropertyChanged(nameof(Diameter));
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
        public double PutCenterToX => X + _offsetX - Diameter / 2;

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
        public double PutCenterToY => Y + _offsetY- Diameter/2;

        public int Id { get; }

        private bool _isSelected;
        public bool IsSelected 
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public ICommand OnClickCommand { get; private set; } = null!;
        public event EventHandler? PointClicked;

        public CircleViewModel(double  x, double y, double diameter, int id, double offsetX = 0, double offsetY = 0)
        {
            SetShared(diameter, offsetX, offsetY);
            X = x;
            Y = y;
            Id = id;
        }
        public CircleViewModel(IDataPoint point, double diameter, double offsetX = 0, double offsetY = 0)
        {
            SetShared(diameter, offsetX, offsetY);
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            Id = point.Id;
        }

        private void SetShared(double diameter, double offsetX, double offsetY)
        {
            Diameter = diameter;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _isSelected = false;
            OnClickCommand = new RelayCommand(() => PointClicked?.Invoke(this, EventArgs.Empty));
        }

        public void ScaleOffset(double offsetX, double offsetY)
        {
            _offsetX += offsetX;
            _offsetY += offsetY;
            OnPropertyChanged();
        }
        public void ScaleZoom(double centerX, double centerY, double newDiameter, double zoomDelta)
        {
            //not correct yet
            //offset changes have happened in displayvm since
            double _deltaX = ((centerX - X) * zoomDelta)/3;
            double _deltaY = ((centerY - Y) * zoomDelta)/3;
            _offsetX += _deltaX;
            _offsetY += _deltaY;
            Diameter = newDiameter;
            OnPropertyChanged();
        }
    }
}
