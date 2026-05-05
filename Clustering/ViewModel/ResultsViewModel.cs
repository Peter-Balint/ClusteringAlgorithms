
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class ResultsViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public ViewModelBase DisplayViewModel { get; private set; }

        public ResultsViewModel(MainModel model)
        {
            _model = model;
            DisplayViewModel = new ResultsSpatial2DViewModel(model);
        }
    }
}
