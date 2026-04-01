
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    class Display2DViewModel : ViewModelBase
    {
        public double CanvasHeight { get; set; }
        public double CanvasWidth { get; set; }

        private double _currentDiameter;

        private CircleViewModel? _selectedPoint = null;
        private double? _clickedX = null;
        public double? ClickedX 
        {
            get
            {
                if (_clickedX == null) return null;
                return Math.Round(_clickedX.Value, 2);
            }
            set
            {
                _clickedX = value;
                OnPropertyChanged(nameof(ClickedX));
            }
        }
        private double? _clickedY = null;
        public double? ClickedY
        {
            get
            {
                if (_clickedY == null) return null;
                return Math.Round(_clickedY.Value,2);
            }
            set
            {
                _clickedY = value;
                OnPropertyChanged(nameof(ClickedY));
            }
        }

        //mivel observable collection nem adhatja vissza egyből a model egy collectionjét
        //akkor a model pont létrehozásáról és törléséről jelez eventtel? in the alg
        public ObservableCollection<CircleViewModel> Points { get; set; }

        public ICommand AddPoint { get; }
        public ICommand CanvasClicked { get; }

        public Display2DViewModel()
        {
            Points = new ObservableCollection<CircleViewModel>();
            //link up points from the model here, dummy for now
            Points.Add(new CircleViewModel(100, 100, 30, 1));
            Points.Add(new CircleViewModel(200, 300, 30, 2));

            foreach (CircleViewModel point in Points)
            {
                point.PointClicked += OnPointClicked;
            }

            CanvasClicked = new RelayCommand<MouseButtonEventArgs>(OnCanvasClicked);

            //AddPoint = new RelayCommand();
        }

        private void OnPointClicked(object? sender, EventArgs e)
        {
            if(sender is not null && sender is CircleViewModel point)
            {
                _selectedPoint?.IsSelected = false;
                ClickedX = ClickedY = null;
                _selectedPoint = point;
                _selectedPoint.IsSelected = true;
            }
        }

        private void OnCanvasClicked(MouseButtonEventArgs? m)
        {
            if (m is null) return;
            _selectedPoint?.IsSelected = false;
            _selectedPoint = null;

            var position = m.GetPosition((IInputElement)m.Source);

            ClickedX = position.X;
            ClickedY = position.Y;
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
