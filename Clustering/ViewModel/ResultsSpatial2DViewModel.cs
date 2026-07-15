
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class ResultsSpatial2DViewModel : ViewModelBase
    {
        //both in the shapevms and the displays many things could be moved to a parent class
        private MainModel _model;

        private double _offsetX = 0;
        private double _offsetY = 0;

        private double _dragStartX;
        private double _dragStartY;

        private int _baseDiameter = 30;

        private double _zoomFactor = 1;
        public double ZoomFactor
        {
            get => _zoomFactor;
            set
            {
                _zoomFactor = value;
                OnPropertyChanged(nameof(ZoomFactor));
            }
        }

        public bool ResultsAvailable => _model.ResultsAvailable;

        private int _currentStep = 0;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (!_model.ResultsAvailable) return;
                if (value < 0 || value >= _model.Results!.GetStepCount()) return;
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }

        public ObservableCollection<IShape> Points { get; }

        public ICommand CanvasClickedCommand { get; private set; }
        public ICommand MouseMoveCommand { get; private set; }
        public ICommand CanvasReleasedCommand { get; }
        public ICommand ZoomCommand { get; }
        public ICommand StepBackCommand { get; }
        public ICommand StepForwardsCommand { get; }

        public ResultsSpatial2DViewModel(MainModel model)
        {
            _model = model;

            Points = new ObservableCollection<IShape>();

            if (_model.ResultsAvailable)
            {
                CurrentStep = _model.Results!.GetStepCount();
                GoToStep(CurrentStep);
            }

            CanvasClickedCommand = new RelayCommand<MouseButtonEventArgs>(OnCanvasClickedDrag);
            CanvasReleasedCommand = new RelayCommand(OnDragReleased);

            ZoomCommand = new RelayCommand<MouseWheelEventArgs>(OnCanvasScrolling);

            StepBackCommand = new RelayCommand(()=>GoToStep(CurrentStep-1));
            StepForwardsCommand = new RelayCommand(() => GoToStep(CurrentStep+1));
        }

        private void OnCanvasClickedDrag(MouseButtonEventArgs? m)
        {
            if (m is null || m.Source is not Canvas) return;

            MouseMoveCommand = new RelayCommand<MouseEventArgs>(OnMouseDrag);
            OnPropertyChanged(nameof(MouseMoveCommand));

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
            ScalePoints();
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

            if (m.Delta > 0 && ZoomFactor <= 2.9)
            {
                ZoomFactor += 0.1;
                ScalePoints();
            }
            else if (m.Delta < 0 && ZoomFactor >= 0.2)
            {
                ZoomFactor -= 0.1;
                ScalePoints();
            }
        }

        private void ScalePoints()
        {
            foreach (IShape point in Points)
            {
                point.Scale(_offsetX, _offsetY, ZoomFactor, _baseDiameter);
            }
        }

        private void GoToStep(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= _model.Results!.GetStepCount()) return;
            CurrentStep = stepIndex;
            Points.Clear();
            (IDataPoint[] points, Cluster[] clusters) = _model.Results!.GetStepAt(stepIndex);
            foreach(IDataPoint point in points)
            {
                //todo: implement zoom here as well
                Points.Add(new CircleViewModel(point, _baseDiameter, ZoomFactor, _offsetX, _offsetY));
            }
            foreach(Cluster cluster in clusters)
            {
                Points.Add(new SquareViewModel(cluster.CenterPoint,_baseDiameter, ZoomFactor, _offsetX, _offsetY));
            }
        }
    }
}
