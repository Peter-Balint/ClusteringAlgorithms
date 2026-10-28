using Clustering.Model;
using Clustering.ViewModel.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    internal class ProgressViewModel : ViewModelBase
    {
        private readonly MainModel _mainModel;
        private readonly INavigationService _resultsNavigationService;

        private int _currentStep = 1;
        public string StepText => $"Current step: {_currentStep}";
        public bool IsFinished {  get; private set; }

        public ICommand FinishCommand { get; }
        public ICommand NavigateToResultsCommand { get; }

        public ProgressViewModel(MainModel model, INavigationService resultsNavigationService)
        {
            _mainModel = model;
            _mainModel.StepFinished += OnStepFinished;
            //on really fast clusterings can the event try to fire and fizzle before this constructor?
            _mainModel.ClusteringFinished += OnClusteringFinished;

            FinishCommand = new RelayCommand(() => _mainModel.StopClustering());

            _resultsNavigationService = resultsNavigationService;
            NavigateToResultsCommand = new RelayCommand(() => _resultsNavigationService.Navigate());
            //two buttons and commands: one to stop after current step
            //one to go to the results page, whether the process was finished naturally or prompted to
        }

        private void OnStepFinished(object? sender, int finishedStep)
        {
            _currentStep = finishedStep + 1;
            OnPropertyChanged(nameof(StepText));
        }
        private void OnClusteringFinished(object? sender, EventArgs e)
        {
            IsFinished = true;
            OnPropertyChanged(nameof(IsFinished));
        }
    }
}
