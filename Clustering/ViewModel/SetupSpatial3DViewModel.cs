
using Clustering.Model;
using Clustering.ViewModel.Navigation;

namespace Clustering.ViewModel
{
    internal class SetupSpatial3DViewModel : ViewModelBase
    {
        private MainModel _mainModel;
        private INavigationService _progressNavigationService;
        public SetupSpatial3DViewModel(INavigationService progressNavigationService)
        {

        }

        public void AddPoint(double x, double y, double z)
        {
            _mainModel.AddPoint([x, y, z]);
        }

    }
}
