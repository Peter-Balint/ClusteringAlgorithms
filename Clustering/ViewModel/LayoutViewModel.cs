
namespace Clustering.ViewModel
{
    public class LayoutViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ViewModelBase ContentViewModel { get; }

        private bool _isNavigationEnabled;
        public bool IsNavigationEnabled
        { 
            get => _isNavigationEnabled;
            private set
            {
                _isNavigationEnabled = value;
                OnPropertyChanged(nameof(IsNavigationEnabled));
            } 
        }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;

            if (ContentViewModel is ParametersViewModel _) _isNavigationEnabled = false;
            else _isNavigationEnabled = true;

            ContentViewModel.NavigatabilityChanged += OnContentNavigatabilityChanged;
        }

        //these events might have become obsolete
        private void OnContentNavigatabilityChanged(object? sender, bool isEnabled)
        {
            IsNavigationEnabled = isEnabled;
        }

        public override void Dispose()
        {
            ContentViewModel.NavigatabilityChanged -= OnContentNavigatabilityChanged;

            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}
