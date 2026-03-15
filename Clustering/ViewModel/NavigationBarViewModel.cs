
using Clustering.ViewModel.Navigation;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand NavigateToParameters { get; }
        public ICommand NavigateToSetup { get; }
        public ICommand NavigateToWalkthrough { get; }

        public NavigationBarViewModel(
            INavigationService parametersNavigationService,
            INavigationService setupNavigationService,
            INavigationService walkthroughNavigationService)
        {
            NavigateToParameters = new NavigateCommand(parametersNavigationService);
            NavigateToSetup = new NavigateCommand(setupNavigationService);
            NavigateToWalkthrough = new NavigateCommand(walkthroughNavigationService);
        }
    }
}
