using System.Windows.Input;

namespace Clustering.WPF.ViewModel.Navigation
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public NavigateCommand NavigateToParameters { get; }
        public NavigateCommand NavigateToSetup { get; }
        public NavigateCommand NavigateToResult { get; }

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
