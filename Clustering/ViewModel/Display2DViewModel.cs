
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    class Display2DViewModel : ViewModelBase
    {
        private MainModel _mainModel;

        public double CanvasHeight { get; set; }
        public double CanvasWidth { get; set; }

        private double _currentDiameter = 50; //will have some zoom factor

        private CircleViewModel? _selectedPoint = null;
        public CircleViewModel? SelectedPoint
        { 
            get => _selectedPoint;
            set
            {
                if (_selectedPoint == value) return;
                _selectedPoint = value;
                OnPropertyChanged(nameof(SelectedPoint));
            }
        }

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

        public ICommand CanvasClickedCommand { get; }
        public ICommand AddPointCommand { get; }
        public ICommand RemovePointCommand { get; }

        public Display2DViewModel(MainModel model)
        {
            _mainModel = model;

            Points = new ObservableCollection<CircleViewModel>();

            IEnumerable<IDataPoint> modelPoints = _mainModel.GetAllPoints();
            foreach(IDataPoint point in modelPoints)
            {
                Points.Add(new CircleViewModel(point,_currentDiameter));
            }

            Points.Add(new CircleViewModel(100, 100, 30, 100));
            Points.Add(new CircleViewModel(200, 300, 30, 200));
            Points.Add(new CircleViewModel(-10, -10, 30, 300));

            foreach (CircleViewModel point in Points)
            {
                point.PointClicked += OnPointClicked;
            }

            CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClicked);

            AddPointCommand = new RelayCommand(AddPoint);
            RemovePointCommand = new RelayCommand(RemovePoint);

            _mainModel.PointCreated += OnPointCreated;
        }

        private void OnPointClicked(object? sender, EventArgs e)
        {
            if(sender is not null && sender is CircleViewModel point && point!= SelectedPoint)
            {
                SelectedPoint?.IsSelected = false;
                ClickedX = ClickedY = null;
                SelectedPoint = point;
                SelectedPoint.IsSelected = true;
            }
        }

        private void OnCanvasClicked(MouseButtonEventArgs? m)
        {
            if (m is null || m.Source is not Canvas) return;
            SelectedPoint?.IsSelected = false;
            SelectedPoint = null;

            var position = m.GetPosition((IInputElement)m.Source);

            ClickedX = position.X;
            ClickedY = position.Y;
        }

        private void AddPoint()
        {
            if (ClickedX is not null && ClickedY is not null)
            {
                _mainModel.AddPoint([ClickedX.Value, ClickedY.Value]);
            }
        }
        private void RemovePoint()
        {
            if (SelectedPoint is not null)
            {
                _mainModel.RemovePoint(SelectedPoint!.Id);
                Points.Remove(SelectedPoint);
                SelectedPoint = null;
            }
        }

        private void OnPointCreated(object? sender, IDataPoint point)
        {
            Points.Add(new CircleViewModel(point, _currentDiameter));
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
