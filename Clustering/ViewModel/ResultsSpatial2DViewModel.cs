
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    internal class ResultsSpatial2DViewModel : Spatial2DViewModelBase
    {
        private MainModel _model;

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

        public ObservableCollection<Shape2DViewModelBase> Points { get; }

        public ICommand StepBackCommand { get; }
        public ICommand StepForwardsCommand { get; }

        public ResultsSpatial2DViewModel(MainModel model)
        {
            _model = model;

            Points = new ObservableCollection<Shape2DViewModelBase>();

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


        protected override void ScalePoints()
        {
            foreach (Shape2DViewModelBase point in Points)
            {
                point.Scale(_offsetX, _offsetY, _zoomFactor, _baseDiameter);
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
                Points.Add(new CircleViewModel(point, _baseDiameter, _zoomFactor, _offsetX, _offsetY));
            }
            foreach(Cluster cluster in clusters)
            {
                Points.Add(new SquareViewModel(cluster.CenterPoint,_baseDiameter, _zoomFactor, _offsetX, _offsetY));
            }
        }
    }
}
