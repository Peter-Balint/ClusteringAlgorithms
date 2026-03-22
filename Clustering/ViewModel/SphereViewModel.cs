
using System.Drawing;
using System.Windows.Media.Media3D;

namespace Clustering.ViewModel
{
    public class SphereViewModel : ViewModelBase
    {
        private Point3D _center;
        private double _radius;
        private Color _color;

        public Point3D Center
        {
            get => _center;
            set 
            { 
                _center = value; 
                OnPropertyChanged(nameof(Center)); 
            }
        }

        public double Radius
        {
            get => _radius;
            set 
            { 
                _radius = value; 
                OnPropertyChanged(nameof(Radius)); 
            }
        }

        public Color Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(nameof(Color)); }
        }

        public SphereViewModel(Point3D center, double radius, Color color)
        {
            Center = center;
            Radius = radius;
            Color = color;
        }
    }
}
