
namespace Clustering.ViewModel.Navigation
{
    //originally there is a CloseModalNavigationService, but I would like to do it in a more integrated way
    //without composite nav services
    //idea: viewmodels have a disposed event, which ModalNavigationService subsribes to, and calls ModalNavigationStore.Close()
    //with the way ModalNavigationStore is set up, this probably won't work

    public class ModalNavigationService<TViewModel> : INavigationService
        where TViewModel : ViewModelBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public ModalNavigationService(ModalNavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _modalNavigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _modalNavigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
