
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using Clustering.ViewModel.Navigation;

namespace Clustering.ViewModel
{
    internal class SetupSpatial3DViewModel : ViewModelBase
    {
        private MainModel _mainModel;
        private INavigationService _progressNavigationService;

        public bool IsRunnable => _mainModel.IsRunnable;


        public SetupSpatial3DViewModel(MainModel model, INavigationService progressNavigationService)
        {
            _mainModel = model;
            _progressNavigationService = progressNavigationService;
        }

        public void AddPoint(double x, double y, double z)
        {
            _mainModel.AddPoint([x, y, z]);
        }
        //might work on id, might work on a shared selected point thing, will depend on the type of data structures and the connection the view+vm will have
        private void RemovePoint()
        {
        }

        private void OnPointCreated(object? sender, IDataPoint point)
        {
        }
        private void OnPointsCreated(object? sender, IDataPoint[] points)
        {
        }

        private void LoadPoints()
        {
        }

        private void RunClustering()
        {
        }

    }
}
