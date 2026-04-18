
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class ResultViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public ViewModelBase DisplayViewModel { get; private set; }

        public ResultViewModel(MainModel model)
        {
            _model = model;
            DisplayViewModel = new ResultsDisplay2DViewModel(model);
        }
    }
}
