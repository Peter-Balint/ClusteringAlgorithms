
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class ResultsViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public DisplayMode DisplayMode => _model.ParameterSet.DisplayMode;
        public ViewModelBase DisplayViewModel { get; private set; }

        public ResultsViewModel(MainModel model)
        {
            _model = model;
            switch (this.DisplayMode)
            {
                case DisplayMode.Spatial2D: { DisplayViewModel = new ResultsSpatial2DViewModel(_model); break; }
                case DisplayMode.Image: { DisplayViewModel = new ResultsImageViewModel(_model); break; }
            }
        }
    }
}
