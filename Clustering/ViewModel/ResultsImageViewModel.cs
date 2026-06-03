
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using Colourful;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Clustering.ViewModel
{
    public class ResultsImageViewModel : ViewModelBase
    {
        private MainModel _model;

        private ImageViewModel[] _images;
        public BitmapSource CurrentImage => _images[CurrentStep].Source;

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

        public ICommand StepBackCommand { get; }
        public ICommand StepForwardsCommand { get; }

        public ResultsImageViewModel(MainModel model)
        {
            _model = model;

            //conversion logic might get moved to the model layer later
            IColorConverter<LabColor, RGBColor> converter = new ConverterBuilder().FromLab().ToRGB().Build();

            int stepCount = _model.Results!.GetStepCount();
            _images = new ImageViewModel[stepCount];
            for(int i = 0; i < stepCount; i++)
            {
                (IDataPoint[] points, Cluster[] clusters) = _model.Results.GetStepAt(i);
                _images[i] = new ImageViewModel(points, clusters,_model.ImageWidth,_model.ImageHeight,converter);
            }

            StepBackCommand = new RelayCommand(() => GoToStep(CurrentStep - 1));
            StepForwardsCommand = new RelayCommand(() => GoToStep(CurrentStep + 1));
        }

        private void GoToStep(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= _model.Results!.GetStepCount()) return;
            CurrentStep = stepIndex;
            OnPropertyChanged(nameof(CurrentImage));
        }
    }
}
