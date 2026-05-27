
using Clustering.Model;
using Clustering.ViewModel.Navigation;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class SetupImageViewModel : ViewModelBase
    {
        private MainModel _model;
        private INavigationService _resultsNavigationService;

        public ICommand LoadImageCommand { get; }

        public SetupImageViewModel(MainModel model, INavigationService resultsNavigationService)
        {
            _model = model;
            _resultsNavigationService = resultsNavigationService;

            LoadImageCommand = new RelayCommand(OpenImageDialog);
        }

        private void OpenImageDialog()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select a PNG image",
                Filter = "PNG Files (*.png)|*.png",
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                _model.LoadImage(dialog.FileName);
            }
        }
    }
}
