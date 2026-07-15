
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Modeling.Diagrams;
using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace Clustering.ViewModel
{
    class CircleViewModel : ViewModelBase, IShape
    {
        private double _offsetX;
        private double _offsetY;
        private double _zoomFactor;

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
        public double PutCenterToX => X*_zoomFactor - _offsetX*_zoomFactor - Diameter / 2;

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
        public double PutCenterToY => Y*_zoomFactor - _offsetY*_zoomFactor - Diameter/2;

        public int Id { get; }
        public int _clusterId;

        public Brush Color => new SolidColorBrush(GetColorFromId(_clusterId));

        private Color GetColorFromId(int id)
        {
            int hue = (id * 137) %240;
            DrawingColor dColor = new HslColor(hue, 120, 120).ToRgbColor();
            return MediaColor.FromArgb(dColor.A,dColor.R,dColor.G,dColor.B);
        }

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

        public CircleViewModel(double  x, double y, double baseDiameter, int id, double zoomFactor, double offsetX, double offsetY)
        {
            SetShared(baseDiameter, zoomFactor, offsetX, offsetY);
            X = x;
            Y = y;
            Id = id;
        }
        public CircleViewModel(IDataPoint point, double baseDiameter, double zoomFactor, double offsetX, double offsetY)
        {
            SetShared(baseDiameter, zoomFactor, offsetX, offsetY);
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            Id = point.Id;
            _clusterId = point.ClusterId;
        }

        private void SetShared(double baseDiameter, double zoomfactor, double offsetX, double offsetY)
        {
            Diameter = baseDiameter*zoomfactor;
            _zoomFactor = zoomfactor;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _isSelected = false;
            OnClickCommand = new RelayCommand(() => PointClicked?.Invoke(this, EventArgs.Empty));
        }

        public void Scale(double offsetX, double offsetY, double zoomFactor, int baseDiameter)
        {
            _offsetX = offsetX;
            _offsetY = offsetY;
            _zoomFactor = zoomFactor;
            Diameter = baseDiameter * zoomFactor;
            OnPropertyChanged();
        }
    }
}
