
namespace Clustering.ViewModel
{
    internal abstract class Shape2DViewModelBase : ViewModelBase
    {
        protected double _offsetX;
        protected double _offsetY;
        protected double _zoomFactor;

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
        public abstract double PutCenterToX {  get; }

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
        public abstract double PutCenterToY { get; }

        public abstract void Scale(double offsetX, double offsetY, double zoomFactor, int baseSize);
    }
}
