
using Clustering.Model;
using Clustering.ViewModel.Navigation;

namespace Clustering.ViewModel
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        //in the lifecycle of this viewmodel the displaymode cannot change
        public DisplayMode DisplayMode => _model.ParameterSet.DisplayMode;

        public ViewModelBase DisplayViewModel { get; private set; }

        public SetupViewModel(MainModel model, INavigationService progressNavigationService)
        {
            _model = model;
            _model.Initialize();

            switch (this.DisplayMode)
            {
                case DisplayMode.Spatial2D: { DisplayViewModel = new SetupSpatial2DViewModel(_model, progressNavigationService); break; }
                case DisplayMode.Spatial3D: { DisplayViewModel = new SetupSpatial3DViewModel(_model, progressNavigationService); break; }
                case DisplayMode.Image: { DisplayViewModel = new SetupImageViewModel(_model, progressNavigationService); break; }
            }
        }
    }
}
