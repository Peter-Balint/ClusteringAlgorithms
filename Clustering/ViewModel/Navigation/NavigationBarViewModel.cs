using System.Windows.Input;

namespace Clustering.ViewModel.Navigation
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand NavigateToParameters { get; }
        public ICommand NavigateToSetup { get; }
        public ICommand NavigateToResult { get; }

        public NavigationBarViewModel(
            INavigationService parametersNavigationService,
            INavigationService setupNavigationService,
            INavigationService resultNavigationService)
        {
            NavigateToParameters = new NavigateCommand(parametersNavigationService);
            NavigateToSetup = new NavigateCommand(setupNavigationService);
            NavigateToResult = new NavigateCommand(resultNavigationService);
        }
    }
}
