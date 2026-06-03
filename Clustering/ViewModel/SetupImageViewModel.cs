
using Clustering.Model;
using Clustering.ViewModel.Navigation;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Clustering.ViewModel
{
    public class SetupImageViewModel : ViewModelBase
    {
        private MainModel _mainModel;
        private INavigationService _resultsNavigationService;

        private bool _isImageLoaded = false;
        public bool IsRunnable => _mainModel.IsRunnable && _isImageLoaded;

        private BitmapImage? _image;
        public BitmapImage? Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public ICommand LoadImageCommand { get; }
        public ICommand RunCommand { get; }

        public SetupImageViewModel(MainModel model, INavigationService resultsNavigationService)
        {
            _mainModel = model;
            _resultsNavigationService = resultsNavigationService;

            _mainModel.InitialImageLoaded += OnInitialImageLoaded;

            LoadImageCommand = new RelayCommand(OpenImageDialog);
            RunCommand = new RelayCommand(RunClustering);
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
                _mainModel.LoadImage(dialog.FileName);
                _isImageLoaded = true;
                OnPropertyChanged(nameof(IsRunnable));
            }
        }

        private void OnInitialImageLoaded(object? sender, BitmapImage e)
        {
            Image = e;
        }

        private void RunClustering()
        {
            _mainModel.RunClustering();
            _resultsNavigationService.Navigate();
        }
    }
}
