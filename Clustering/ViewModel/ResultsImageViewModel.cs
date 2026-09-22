
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using Colourful;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;

namespace Clustering.ViewModel
{
    public class ResultsImageViewModel : ViewModelBase
    {
        private MainModel _model;
        private ImageHandler _imageHandler => _model.ImageHandler;

        private ImageViewModel[] _images;
        public BitmapSource CurrentImage => _images[CurrentStep].Source;

        public bool ResultsAvailable => _model.ResultsAvailable;

        private int _currentStep = 0;
        public int CurrentStep
        {
            get => _currentStep;
            set
            {
                if (!_model.ResultsAvailable) return;
                if (value < 0 || value >= _model.Results!.GetStepCount()) return;
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }

        public ICommand StepBackCommand { get; }
        public ICommand StepForwardsCommand { get; }
        public ICommand SaveImageCommand { get; }
        public ICommand SaveImageSequenceCommand { get; }

        public ResultsImageViewModel(MainModel model)
        {
            _model = model;

            //conversion logic might get moved to the model layer later
            IColorConverter<LabColor, RGBColor> converter = new ConverterBuilder().FromLab().ToRGB().Build();

            int stepCount = _model.Results!.GetStepCount();
            _images = new ImageViewModel[stepCount];

            (IDataPoint[] pointsAtZero, _) = _model.Results.GetStepAt(0);
            _images[0] = new ImageViewModel(_imageHandler.ImageSource!);
            for(int i = 1; i < stepCount; i++)
            {
                (IDataPoint[] points, Cluster[] clusters) = _model.Results.GetStepAt(i);
                _images[i] = new ImageViewModel(_imageHandler.ImageWidth, _imageHandler.ImageHeight, converter, points, clusters);
            }

            StepBackCommand = new RelayCommand(() => GoToStep(CurrentStep - 1));
            StepForwardsCommand = new RelayCommand(() => GoToStep(CurrentStep + 1));
            SaveImageCommand = new RelayCommand(SaveCurrentImage);
            SaveImageSequenceCommand = new RelayCommand(SaveImageSequence);
        }

        private void GoToStep(int stepIndex)
        {
            if (stepIndex < 0 || stepIndex >= _model.Results!.GetStepCount()) return;
            CurrentStep = stepIndex;
            OnPropertyChanged(nameof(CurrentImage));
        }

        private void SaveCurrentImage()
        {
            if (_images is null || _images.Length == 0) return;
            var dialog = new SaveFileDialog
            {
                Title = "Save image",
                Filter = "PNG Image (*.png)|*.png",
                DefaultExt = ".png",
                AddExtension = true
            };
            if (dialog.ShowDialog() == true)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(CurrentImage));

                using var stream = File.Create(dialog.FileName);
                encoder.Save(stream);
            }
        }
        private void SaveImageSequence()
        {
            if (_images is null || _images.Length == 0) return;

            var dialog = new SaveFileDialog
            {
                Title = "Save images",
                Filter = "PNG Image (*.png)|*.png",
                AddExtension = false
            };
            if (dialog.ShowDialog() == true)
            {
                string directory = Path.GetDirectoryName(dialog.FileName)!;
                string baseName = Path.GetFileNameWithoutExtension(dialog.FileName);

                for (int i = 0; i < _images.Length; i++)
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(_images[i].Source));

                    string fileName = Path.Combine(directory, $"{baseName}{i:D3}.png");

                    using var stream = File.Create(fileName);
                    encoder.Save(stream);
                }
            }
        }
    }
}
