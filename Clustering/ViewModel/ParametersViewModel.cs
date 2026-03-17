
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public ParametersViewModel(MainModel model)
        {
            _model = model;
        }
    }
}
