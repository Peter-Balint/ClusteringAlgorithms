
using CommunityToolkit.Mvvm.Input;
using SharpDX.Direct3D11;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    class CircleViewModel : ViewModelBase
    {
        //itt sem direkt referencia lesz valszeg a modellbeli pontra
        //lehet itt nem kell notification, mert mindig változáskor új pont lesz: csak a modellből érkező változásra igaz, mert az új állapot
        //viewből is jöhet: zoom, scroll

        public double Diameter { get; set; }
        private double _x;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }
        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }

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

        public ICommand OnClick { get; }
        public event EventHandler PointClicked;

        public CircleViewModel(double  x, double y, double diameter, int id)
        {
            X = x;
            Y = y;
            Diameter = diameter;
            Id = id;
            _isSelected = false;
            OnClick = new RelayCommand(() => PointClicked?.Invoke(this, EventArgs.Empty));
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
