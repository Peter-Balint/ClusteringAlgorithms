
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
        public double ICWidth {  get; set; }
        //private double _canvasWidth;
        public double CanvasWidth
        {
            get => ICWidth/ZoomFactor;
        }

        private double _currentDiameter = 50; //will have some zoom factor

        private double _zoomFactor;
        public double ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                _zoomFactor = value;
                OnPropertyChanged(nameof(ZoomFactor));
                OnPropertyChanged(nameof(CanvasWidth));
            }
        }

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
        public ICommand ZoomCommand { get; }

        public Display2DViewModel(MainModel model)
        {
            _mainModel = model;

            Points = new ObservableCollection<CircleViewModel>();

            ZoomFactor = 1;

            IEnumerable<IDataPoint> modelPoints = _mainModel.GetAllPoints();
            foreach(IDataPoint point in modelPoints)
            {
                Points.Add(new CircleViewModel(point,_currentDiameter));
            }

            Points.Add(new CircleViewModel(100, 100, 30, 100));
            Points.Add(new CircleViewModel(200, 300, 30, 200));
            Points.Add(new CircleViewModel(-5, -5, 30, 300));

            foreach (CircleViewModel point in Points)
            {
                point.PointClicked += OnPointClicked;
            }

            CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClicked);
            ZoomCommand = new RelayCommand<MouseWheelEventArgs>(OnCanvasScrolling);

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

        private void OnCanvasScrolling(MouseWheelEventArgs? m)
        {
            if (m is null) return;

            if (m.Delta > 0)
                ZoomFactor += 0.1;
            else
                ZoomFactor -= 0.1;

            // Clamp zoom
            ZoomFactor = Math.Max(0.1, Math.Min(ZoomFactor, 5.0));
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
                SelectedPoint.PointClicked -= OnPointClicked;
                SelectedPoint = null;
            }
        }

        private void OnPointCreated(object? sender, IDataPoint point)
        {
            CircleViewModel pointVM = new CircleViewModel(point, _currentDiameter);
            pointVM.PointClicked += OnPointClicked;
            Points.Add(pointVM);
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
