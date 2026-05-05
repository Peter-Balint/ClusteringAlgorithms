
using Clustering.Model;
using Clustering.ViewModel.Navigation;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        //in the lifecycle of this viewmodel the displaymode cannot change
        public DisplayMode DisplayMode => _model.ParameterSet.DisplayMode;

        public ViewModelBase DisplayViewModel { get; private set; }

        public SetupViewModel(MainModel model, INavigationService resultsNavigationService)
        {
            _model = model;
            _model.Initialize(); //only reinitialize after breaking param changes

            //later initilaize vm based on displaymode
            DisplayViewModel = new SetupSpatial2DViewModel(_model, resultsNavigationService);
            DisplayViewModel.NavigatabilityChanged += BubbleDisplayNavigatabilityChanged;
        }

        //come back later to check the neccessity of this with the new navigation style
        private void BubbleDisplayNavigatabilityChanged(object? sender, bool IsEnabled)
        {
            if (IsEnabled) EnableNavigation();
            else DisableNavigation();
        }
    }
}
