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

            ContentViewModel.NavigatabilityChanged += OnContentNaviagatabilityChanged;
        }

        private void OnContentNaviagatabilityChanged(object? sender, bool isEnabled)
        {
            IsNavigationEnabled = isEnabled;
        }

        public override void Dispose()
        {
            ContentViewModel.NavigatabilityChanged -= OnContentNaviagatabilityChanged;

            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}
