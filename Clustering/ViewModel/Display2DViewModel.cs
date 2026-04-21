
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
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

        private double _baseDiameter = 30;
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
        public ICommand LoadPointsCommand { get; }

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
            if (m is null || m.Source is not Canvas) return;

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
            CircleViewModel pointVM = new CircleViewModel(point, _currentDiameter, -_offsetX,-_offsetY);
            pointVM.PointClicked += OnPointClicked;
            Points.Add(pointVM);
            OnPropertyChanged(nameof(IsRunnable));
        }
        private void OnPointsCreated(object? sender, IDataPoint[] points)
        {
            foreach(IDataPoint point in points)
            {
                CircleViewModel pointVM = new CircleViewModel(point, _currentDiameter, -_offsetX, -_offsetY);
                pointVM.PointClicked += OnPointClicked;
                Points.Add(pointVM);
            }
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

        private void LoadPoints()
        {
            FileDialog fd = new OpenFileDialog();
            fd.Filter = "Setup files (*.sup)|*.sup";
            if (fd.ShowDialog() == false)
            {
                return;
            }
            using var reader = new StreamReader(fd.FileName);
            List<double[]> pointsFromFile = new List<double[]>();
            int lineCounter = 1;
            string? line = reader.ReadLine();
            if (line is null)
            {
                MessageBox.Show($"Opened file was empty:\n{fd.FileName}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            line = line.Trim();
            if (int.TryParse(line, out int dimension) == false)
            {
                MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nDimension of points was not declared","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            if (dimension < 2)
            {
                MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nDimension of points must be at least 2", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            while ((line = reader.ReadLine()) is not null)
            {
                lineCounter++;
                string[] splits = line.Split(';');
                if (splits.Length != dimension)
                {
                    MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nNumber of coordinates of point didn't match the declared dimension","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }
                double[] coordinates = new double[dimension];
                for (int i = 0; i < dimension; i++)
                {
                    if (double.TryParse(splits[i], out double coordinate) == false)
                    {
                        MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nCoordinate could not be parsed: {splits[i]}","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                        return;
                    }
                    coordinates[i] = coordinate;
                }
                pointsFromFile.Add(coordinates);
            }
            //from here on out operating under the assumption that once parameters are set
            //setting them again, or at least the important ones, leads to a full reset
            //do I need to unsubscribe from the events of points?
            Points.Clear();
            if (dimension > 2)
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
            EnableNavigation();
        }
    }
}
