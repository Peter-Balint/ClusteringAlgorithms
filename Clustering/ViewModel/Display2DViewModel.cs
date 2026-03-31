
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    class Display2DViewModel : ViewModelBase
    {
        public double CanvasHeight { get; set; }
        public double CanvasWidth { get; set; }

        private double _currentDiameter;

        private CircleViewModel? _selectedPoint = null;

        //mivel observable collection nem adhatja vissza egyből a model egy collectionjét
        //akkor a model pont létrehozásáról és törléséről jelez eventtel?
        public ObservableCollection<CircleViewModel> Points { get; set; }

        public ICommand AddPoint { get; }

        public Display2DViewModel()
        {
            Points = new ObservableCollection<CircleViewModel>();
            //link up points from the model here, dummy for now
            Points.Add(new CircleViewModel(100, 100, 30, 1));
            Points.Add(new CircleViewModel(200, 300, 30, 2));

            foreach (CircleViewModel point in Points)
            {
                point.PointClicked += PointClicked;
            }

            //AddPoint = new RelayCommand();
        }

        private void PointClicked(object? sender, EventArgs e)
        {
            if(sender is not null && sender is CircleViewModel point)
            {
                _selectedPoint?.IsSelected = false;
                _selectedPoint = point;
                _selectedPoint.IsSelected = true;
            }
        }

        private void ScalePoints(double deltaX, double deltaY, double? newDiameter = null)
        {
            foreach (CircleViewModel point in Points)
            {
                point.Scale(deltaX, deltaY, newDiameter);
            }
        }

    }
}
