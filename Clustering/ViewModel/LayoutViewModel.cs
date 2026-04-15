using Clustering.ViewModel.Navigation;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class LayoutViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public ViewModelBase ContentViewModel { get; }

        private bool isNavigationEnabled;
        public bool IsNavigationEnabled
        { 
            get => isNavigationEnabled;
            private set
            {
                isNavigationEnabled = value;
                OnPropertyChanged(nameof(IsNavigationEnabled));
            } 
        }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, ViewModelBase contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;

            ContentViewModel.NavigatabilityChanged += OnContentNavigatabilityChanged;
        }

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
