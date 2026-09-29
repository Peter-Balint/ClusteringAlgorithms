
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
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}
