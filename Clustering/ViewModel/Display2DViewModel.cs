
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

        private bool _isSelectMode;
        public bool IsSelectMode
        {
            get => _isSelectMode;
            set
            {
                _isSelectMode = value;
                OnPropertyChanged(nameof(IsSelectMode));
                OnPropertyChanged(nameof(IsDragMode));
            }
        }
        private double _dragStartX;
        private double _dragStartY;
        public bool IsDragMode => !IsSelectMode;


        private double _offsetX = 0;
        private double _offsetY = 0;

        private double _baseDiameter = 50;
        private double _currentDiameter => _baseDiameter / ZoomFactor; //will have some zoom factor

        private double _zoomFactor;
        public double ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                _zoomFactor = value;
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
                return Math.Round(_clickedX.Value+_offsetX, 2);
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
                return Math.Round(_clickedY.Value+_offsetY,2);
            }
            set
            {
                _clickedY = value;
                OnPropertyChanged(nameof(ClickedY));
            }
        }

        public bool IsRunnable => _mainModel.IsRunnable;

        public ObservableCollection<CircleViewModel> Points { get; set; }

        public ICommand CanvasClickedCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand CanvasReleasedCommand { get; }
        public ICommand AddPointCommand { get; }
        public ICommand RemovePointCommand { get; }
        public ICommand ZoomCommand { get; }
        public ICommand SwitchToSelectModeCommand {  get; }
        public ICommand SwitchToDragModeCommand { get; }
        public ICommand RunCommand { get; }

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

            Points.Add(new CircleViewModel(100, 100, _currentDiameter, 100));
            Points.Add(new CircleViewModel(200, 300, _currentDiameter, 200));
            Points.Add(new CircleViewModel(-5, -5, _currentDiameter, 300));

            foreach (CircleViewModel point in Points)
            {
                point.PointClicked += OnPointClicked;
            }

            IsSelectMode = false;

            CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClickedDrag);
            CanvasReleasedCommand = new RelayCommand(OnDragReleased);

            //ZoomCommand = new RelayCommand<MouseWheelEventArgs>(OnCanvasScrolling);

            AddPointCommand = new RelayCommand(AddPoint);
            RemovePointCommand = new RelayCommand(RemovePoint);

            _mainModel.PointCreated += OnPointCreated;

            SwitchToSelectModeCommand = new RelayCommand(() => 
                { 
                    CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClickedSelect);
                    IsSelectMode = true;
                    OnPropertyChanged(nameof(CanvasClickedCommand));
                });
            SwitchToDragModeCommand = new RelayCommand(() => 
                { 
                    CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClickedDrag);
                    IsSelectMode = false;
                    OnPropertyChanged(nameof(CanvasClickedCommand));
                });

            RunCommand = new RelayCommand(RunClustering);
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

        private void OnCanvasClickedSelect(MouseButtonEventArgs? m)
        {
            if (m is null || m.Source is not Canvas) return;
            SelectedPoint?.IsSelected = false;
            SelectedPoint = null;

            var position = m.GetPosition((IInputElement)m.Source);

            ClickedX = position.X;
            ClickedY = position.Y;
        }
        private void OnCanvasClickedDrag(MouseButtonEventArgs? m)
        {
            if (m is null || m.Source is not Canvas) return;

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseDrag);
            OnPropertyChanged(nameof(MouseMoveCommand));

            SelectedPoint?.IsSelected = false;
            SelectedPoint = null;
            ClickedX = ClickedY = null;

            _dragStartX = m.GetPosition((IInputElement)m.Source).X;
            _dragStartY = m.GetPosition((IInputElement)m.Source).Y;
        }
        private void OnMouseDrag(MouseEventArgs? m)
        {
            if (m is null) return;

            var currentLocation = m.GetPosition((IInputElement)m.Source);

            double offsetIncrementX = currentLocation.X - _dragStartX;
            double offsetIncrementY = currentLocation.Y - _dragStartY;
            _offsetX -= offsetIncrementX;
            _offsetY -= offsetIncrementY;
            ScalePointsOffset(offsetIncrementX,offsetIncrementY);
            _dragStartX = currentLocation.X;
            _dragStartY = currentLocation.Y;
        }
        private void OnDragReleased()
        {
            MouseMoveCommand = null;
            OnPropertyChanged(nameof(MouseMoveCommand));
        }

        private void OnCanvasScrolling(MouseWheelEventArgs? m)
        {
            if (m is null) return;

            double zoomDelta;
            if (m.Delta > 0)
            {
                zoomDelta = -0.1;
                ZoomFactor += zoomDelta;
            }
            else
            {
                zoomDelta = 0.1;
                ZoomFactor += zoomDelta;
            }

            if (ZoomFactor < 0.1)
            {
                zoomDelta = -ZoomFactor;
                ZoomFactor = 0.1;
            }
            else if(ZoomFactor > 3)
            {
                zoomDelta = 3 - (ZoomFactor - 0.1);
                ZoomFactor = 3;
            }

            var currentLocation = m.GetPosition((IInputElement)m.Source);
            ScalePointsZoom(currentLocation.X + _offsetX, currentLocation.Y + _offsetY, zoomDelta);
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
                OnPropertyChanged(nameof(IsRunnable));
            }
        }

        private void OnPointCreated(object? sender, IDataPoint point)
        {
            CircleViewModel pointVM = new CircleViewModel(point, _currentDiameter, _offsetX,_offsetY);
            pointVM.PointClicked += OnPointClicked;
            Points.Add(pointVM);
            OnPropertyChanged(nameof(IsRunnable));
        }

        private void ScalePointsOffset(double offsetIncrementX, double offsetIncrementY)
        {
            foreach (CircleViewModel point in Points)
            {
                point.ScaleOffset(offsetIncrementX, offsetIncrementY);
            }
        }
        private void ScalePointsZoom(double aroundX, double aroundY, double zoomDelta)
        {
            foreach (CircleViewModel point in Points)
            {
                point.ScaleZoom(aroundX, aroundY, _currentDiameter, zoomDelta);
            }
        }

        //alg futtatása előtt ellenőrizni hogy K<points.length

        private void RunClustering()
        {
            _mainModel.RunClustering();
        }
    }
}
