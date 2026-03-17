
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class ResultViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public ResultViewModel(MainModel model)
        {
            _model = model;
        }
    }
}
