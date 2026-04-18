
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class ResultsDisplay2DViewModel : ViewModelBase
    {
        //both in the shapevms and the displays many things could be moved to a parent class
        private MainModel _model;

        private double _offsetX = 0;
        private double _offsetY = 0;

        private double _dragStartX;
        private double _dragStartY;

        private double _baseDiameter = 50;

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
        public ICommand StepBackCommand { get; }
        public ICommand StepForwardsCommand { get; }

        public ResultsDisplay2DViewModel(MainModel model)
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
            if (m is null) return;

            var currentLocation = m.GetPosition((IInputElement)m.Source);

            double offsetIncrementX = currentLocation.X - _dragStartX;
            double offsetIncrementY = currentLocation.Y - _dragStartY;
            _offsetX -= offsetIncrementX;
            _offsetY -= offsetIncrementY;
            ScalePointsOffset(offsetIncrementX, offsetIncrementY);
            _dragStartX = currentLocation.X;
            _dragStartY = currentLocation.Y;
        }
        private void OnDragReleased()
        {
            MouseMoveCommand = null;
            OnPropertyChanged(nameof(MouseMoveCommand));
        }

        private void ScalePointsOffset(double offsetIncrementX, double offsetIncrementY)
        {
            foreach (IShape point in Points)
            {
                point.ScaleOffset(offsetIncrementX, offsetIncrementY);
            }
        }

        private void GoToStep(int stepIndex)
        {
            //validated twice
            if (stepIndex < 0 || stepIndex >= _model.Results!.GetStepCount()) return;
            CurrentStep = stepIndex;
            Points.Clear();
            (IDataPoint[] points, Cluster[] clusters) = _model.Results!.GetStepAt(stepIndex);
            foreach(IDataPoint point in points)
            {
                Points.Add(new CircleViewModel(point, _baseDiameter, -_offsetX, -_offsetY));
            }
            foreach(Cluster cluster in clusters)
            {
                Points.Add(new SquareViewModel(cluster.CenterPoint,_baseDiameter/2, -_offsetX, -_offsetY));
            }
        }
    }
}
