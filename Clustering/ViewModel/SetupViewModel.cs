
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        //in the lifecycle of this viewmodel the displaymode cannot change
        public DisplayMode DisplayMode => _model.ParameterSet.DisplayMode;

        public ViewModelBase DisplayViewModel { get; private set; }

        public SetupViewModel(MainModel model)
        {
            _model = model;
            _model.Initialize();
            //later initilaize based on displaymode
            DisplayViewModel = new Display2DViewModel(_model);
            DisplayViewModel.NavigatabilityChanged += BubbleDisplayNavigatabilityChanged;
        }

        private void BubbleDisplayNavigatabilityChanged(object? sender, bool IsEnabled)
        {
            if (IsEnabled) EnableNavigation();
            else DisableNavigation();
        }
    }
}
