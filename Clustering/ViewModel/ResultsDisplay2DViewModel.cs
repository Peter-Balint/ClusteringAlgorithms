
using Clustering.Model;
using System.Collections.ObjectModel;

namespace Clustering.ViewModel
{
    public class ResultsDisplay2DViewModel : ViewModelBase
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
                if (value >= 0 || value < _model.Results!.GetStepCount()) return;
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }

        public ObservableCollection<IShape> Points;

        public ResultsDisplay2DViewModel(MainModel model)
        {
            _model = model;

            Points = new ObservableCollection<IShape>();

            if (_model.ResultsAvailable)
            {
                
            }
        }

        private void GoToStep(int stepNumber)
        {
            Points.Clear();

        }
    }
}
