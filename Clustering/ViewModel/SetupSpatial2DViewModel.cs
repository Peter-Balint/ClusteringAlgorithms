
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using Clustering.ViewModel.FileHandling;
using Clustering.ViewModel.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    internal class SetupSpatial2DViewModel : Spatial2DViewModelBase
    {
        private MainModel _mainModel;
        private INavigationService _progressNavigationService;

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
        public bool IsDragMode => !IsSelectMode;


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
                return Math.Round(_clickedX.Value / _zoomFactor + _offsetX, 2);
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
                return Math.Round(_clickedY.Value / _zoomFactor + _offsetY, 2);
            }
            set
            {
                _clickedY = value;
                OnPropertyChanged(nameof(ClickedY));
            }
        }

        public bool IsRunnable => _mainModel.IsRunnable;

        public ObservableCollection<CircleViewModel> Points { get; set; }

        public ICommand AddPointCommand { get; }
        public ICommand RemovePointCommand { get; }
        public ICommand SwitchToSelectModeCommand {  get; }
        public ICommand SwitchToDragModeCommand { get; }
        public ICommand RunCommand { get; }
        public ICommand LoadPointsCommand { get; }

        public SetupSpatial2DViewModel(MainModel model, INavigationService progressNavigationService)
        {
            _mainModel = model;
            _progressNavigationService = progressNavigationService;

            Points = new ObservableCollection<CircleViewModel>();

            IEnumerable<IDataPoint> modelPoints = _mainModel.GetAllPoints();
            foreach(IDataPoint point in modelPoints)
            {
                Points.Add(new CircleViewModel(point,_baseDiameter,_zoomFactor,_offsetX,_offsetY));
            }

            foreach (CircleViewModel point in Points)
            {
                point.PointClicked += OnPointClicked;
            }

            IsSelectMode = false;

            CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClickedDrag);
            CanvasReleasedCommand = new RelayCommand(OnDragReleased);

            ZoomCommand = new RelayCommand<MouseWheelEventArgs>(OnCanvasScrolling);

            AddPointCommand = new RelayCommand(AddPoint);
            RemovePointCommand = new RelayCommand(RemovePoint);

            _mainModel.PointCreated += OnPointCreated;
            _mainModel.PointsCreated += OnPointsCreated;

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

            LoadPointsCommand = new RelayCommand(LoadPoints);
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
        protected override void OnCanvasClickedDrag(MouseButtonEventArgs? m)
        {
            base.OnCanvasClickedDrag(m);

            SelectedPoint?.IsSelected = false;
            SelectedPoint = null;
            ClickedX = ClickedY = null;
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
            CircleViewModel pointVM = new CircleViewModel(point, _baseDiameter, _zoomFactor, _offsetX, _offsetY);
            pointVM.PointClicked += OnPointClicked;
            Points.Add(pointVM);
            OnPropertyChanged(nameof(IsRunnable));
        }
        private void OnPointsCreated(object? sender, IDataPoint[] points)
        {
            foreach(IDataPoint point in points)
            {
                CircleViewModel pointVM = new CircleViewModel(point, _baseDiameter, _zoomFactor, _offsetX, _offsetY);
                pointVM.PointClicked += OnPointClicked;
                Points.Add(pointVM);
            }
            OnPropertyChanged(nameof(IsRunnable));
        }

        protected override void ScalePoints()
        {
            foreach (CircleViewModel point in Points)
            {
                point.Scale(_offsetX, _offsetY, _zoomFactor, _baseDiameter);
            }
        }


        private void LoadPoints()
        {
            var pointsFromFile = PointsLoader.Load(2);
            if (pointsFromFile.Count == 0) return;
            //do I need to unsubscribe from the events of points?
            Points.Clear();
            //this small bit is a little inelegant after the refactor but it should be fine overall
            //adding a new return value or making a whole unique structure because of this seems overkill
            if (pointsFromFile.First().Length > 2)
            {
                _mainModel.NewPointSetFromHigherDimension(pointsFromFile.ToArray(),2);
            }
            else
            {
                _mainModel.NewPointSet(pointsFromFile);
            }
        }

        private void RunClustering()
        {
            _mainModel.RunClustering();
            _progressNavigationService.Navigate();
        }
    }
}
