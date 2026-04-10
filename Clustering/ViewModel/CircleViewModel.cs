
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    class CircleViewModel : ViewModelBase
    {
        //itt sem direkt referencia lesz valszeg a modellbeli pontra
        //lehet itt nem kell notification, mert mindig változáskor új pont lesz: csak a modellből érkező változásra igaz, mert az új állapot
        //viewből is jöhet: zoom, scroll

        //mégegy dolog amit figyelembe kell venni: a position binding nem a kör közepét, hanem a bal felső részét állítja
        public double Diameter { get; set; }
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
        public double PutCenterToX => X - Diameter / 2;

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
        public double PutCenterToY => Y - Diameter/2;

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

        public CircleViewModel(double  x, double y, double diameter, int id)
        {
            SetShared(diameter);
            X = x;
            Y = y;
            Id = id;
        }
        public CircleViewModel(IDataPoint point, double diameter)
        {
            SetShared(diameter);
            X = point.GetCoordinateAt(0);
            Y = point.GetCoordinateAt(1);
            Id = point.Id;
        }

        private void SetShared(double diameter)
        {
            Diameter = diameter;
            _isSelected = false;
            OnClickCommand = new RelayCommand(() => PointClicked?.Invoke(this, EventArgs.Empty));
        }

        public void Scale(double deltaX, double deltaY, double? newDiameter = null)
        {
            //math it out how to translate the model's coordinate to the canvases, including zooming
            //otherwise simply offsetting it would work
            if(newDiameter is null)
            {
                X += deltaX;
                Y += deltaY;
                return;
            }
        }
    }
}
