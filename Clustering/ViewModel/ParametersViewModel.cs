
using Clustering.Model;

namespace Clustering.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        public DisplayMode DisplayMode
        {
            get => _model.ParameterSet.DisplayMode;
            set
            {
                _model.ParameterSet.SetDisplayMode(value);
                OnPropertyChanged(nameof(DisplayMode));
            }
        }

        public ParametersViewModel(MainModel model)
        {
            _model = model;
        }
    }
}
