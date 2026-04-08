
using Clustering.Model;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        private readonly MainModel _model;
        //private readonly ErrorRelay _errorRelay;

        public ObservableCollection<PropertyError> Errors { get; }

        public ICommand PreviewPositiveIntegerInputCommand { get; }

        #region properties

        public DisplayMode DisplayMode
        {
            get => _model.ParameterSet.DisplayMode;
            set
            {
                _model.ParameterSet.DisplayMode = value;
                OnPropertyChanged(nameof(DisplayMode));
            }
        }        
        public DisplayMode[] DisplayModeValues => Enum.GetValues<DisplayMode>();

        public SpatialDistanceMetric SpatialDistanceMetric
        {
            get => _model.ParameterSet.SpatialDistanceMetric;
            set
            {
                _model.ParameterSet.SpatialDistanceMetric = value;
                OnPropertyChanged(nameof(SpatialDistanceMetric));
            }
        }
        public SpatialDistanceMetric[] SpatialDistanceMetricValues => Enum.GetValues<SpatialDistanceMetric>();

        public CenterType CenterType
        {
            get => _model.ParameterSet.CenterType;
            set
            {
                _model.ParameterSet.CenterType = value;
                OnPropertyChanged(nameof(CenterType));
            }
        }
        public CenterType[] CenterTypeValues => Enum.GetValues<CenterType>();

        public TerminateCondition TerminateCondition
        {
            get => _model.ParameterSet.TerminateCondition;
            set
            {
                _model.ParameterSet.TerminateCondition = value;
                OnPropertyChanged(nameof(TerminateCondition));
            }
        }
        public TerminateCondition[] TerminateConditionValues => Enum.GetValues<TerminateCondition>();

        public int IterationNumber
        {
            get => _model.ParameterSet.IterationNumber;
            set
            {
                _model.ParameterSet.IterationNumber = value;
            }
        }
        public string IterationNumberDisplay
        {
            get => IterationNumber.ToString();
            set
            {
                if(value is not null && int.TryParse(value, out _))
                {
                    IterationNumber = int.Parse(value);
                    ClearErrors(nameof(IterationNumber));
                }//flaw:textbox now can't be empty, maybe onewaytosource and more manual handling?
                else
                {
                    IterationNumber = 0;
                    ClearErrors(nameof(IterationNumber));
                    AddError(nameof(IterationNumber), "Iteration Number field should contain a positive integer and should not be empty");
                }
                //this can work if im careful
                //do I even need the IterationNumber property as a proxy
            }
        }
        public double MinimalDelta
        {
            get => _model.ParameterSet.MinimalDelta;
            set
            {
                _model.ParameterSet.MinimalDelta = value;
                OnPropertyChanged(nameof(MinimalDelta));
            }
        }
        public ClusterInitializationMethod ClusterInitializationMethod
        {
            get => _model.ParameterSet.ClusterInitializationMethod;
            set
            {
                _model.ParameterSet.ClusterInitializationMethod = value;
                OnPropertyChanged(nameof(ClusterInitializationMethod));
            }
        }
        public ClusterInitializationMethod[] ClusterInitializationMethodValues => Enum.GetValues<ClusterInitializationMethod>();

        public int InitialClusterNumber
        {
            get => _model.ParameterSet.InitialClusterNumber;
            set
            {
                _model.ParameterSet.InitialClusterNumber = value;
                OnPropertyChanged(nameof(InitialClusterNumber));
            }
        }


        #endregion


        public ParametersViewModel(MainModel model)
        {
            _model = model;
            //_errorRelay = new ErrorRelay();
            Errors = new ObservableCollection<PropertyError>();
            PreviewPositiveIntegerInputCommand = new RelayCommand<TextCompositionEventArgs>(ValidatePositiveIntegerInputFromText);
        }


        private void ValidatePositiveIntegerInputFromText(TextCompositionEventArgs? args)
        {
            args!.Handled = !args.Text.All(char.IsDigit) || args.Text == string.Empty;
        }

        private void AddError(string propertyName, string message)
        {
            for (int i = 0; i < Errors.Count; i++)
            {
                if (Errors[i].PropertyName == propertyName)
                {
                    Errors[i].ErrorMessages.Add(message);
                    return;
                }
            }
            Errors.Add(new PropertyError(propertyName, message));
        }
        private void ClearErrors(string? propertyName = null)
        {
            if(propertyName is null)
            {
                Errors.Clear();
            }
            else
            {
                for(int i = 0; i < Errors.Count; i++)
                {
                    if (Errors[i].PropertyName == propertyName)
                    {
                        Errors.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
