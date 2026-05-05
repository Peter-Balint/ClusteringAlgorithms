
using Clustering.Model;
using Clustering.ViewModel.Navigation;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clustering.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        #region properties

        public ObservableCollection<PropertyError> Errors { get; }
        public bool HasErrors => Errors.Any();
        public bool HasPreviousState => _model.PreviousParameterSet is not null;

        public ICommand ApplyCommand {  get; }
        public ICommand CancelCommand { get; }

        public DisplayMode DisplayMode
        {
            get => _model.ParameterSet.DisplayMode;
            set
            {
                _model.ParameterSet.DisplayMode = value;
                OnPropertyChanged(nameof(DisplayMode));
                OnPropertyChanged(nameof(IsSpatialDistanceMetricApplicable));
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
        public bool IsSpatialDistanceMetricApplicable => DisplayMode == DisplayMode.Spatial2D || DisplayMode == DisplayMode.Spatial3D;

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

                //dependent parameters
                OnPropertyChanged(nameof(IsIterationNumberApplicable));
                ClearErrors(nameof(IterationNumber));
                //maybe replace this with a method that checks for the errors the same way the setter does
                //since just the errors getting added is what we want and the binding isn't two way
                //find out if that works on start as well
                if(IsIterationNumberApplicable) IterationNumberDisplay = IterationNumber.ToString();
                OnPropertyChanged(nameof(IsMinimalDeltaApplicable));
                ClearErrors(nameof(MinimalDelta));
                if (IsMinimalDeltaApplicable) MinimalDeltaDisplay = MinimalDelta.ToString();
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
                ClearErrors(nameof(IterationNumber));

                if(value == string.Empty)
                {
                    IterationNumber = 0;
                    AddError(nameof(IterationNumber), "Iteration Number field should not be empty", IsIterationNumberApplicable);
                }
                else if(!int.TryParse(value, out int parsedValue) || parsedValue < 1)
                {
                    IterationNumber = 0;
                    AddError(nameof(IterationNumber), "Iteration Number field should contain a positive integer", IsIterationNumberApplicable);
                }
                else
                {
                    IterationNumber = parsedValue;
                    ClearErrors(nameof(IterationNumber));
                }
            }
        }
        public bool IsIterationNumberApplicable => TerminateCondition == TerminateCondition.IterationNumber;

        public double MinimalDelta
        {
            get => _model.ParameterSet.MinimalDelta;
            set
            {
                _model.ParameterSet.MinimalDelta = value;
            }
        }
        public string MinimalDeltaDisplay
        {
            get => MinimalDelta.ToString();
            set
            {
                ClearErrors(nameof(MinimalDelta));

                if (value == string.Empty)
                {
                    MinimalDelta = 0;
                    AddError(nameof(MinimalDelta), "Minimal Delta field should not be empty", IsMinimalDeltaApplicable);
                }
                else if (!double.TryParse(value, out double parsedValue) || parsedValue <= 0)
                {
                    IterationNumber = 0;
                    AddError(nameof(MinimalDelta), "Minimal Delta field should contain a positive real number, with comma separator character", IsMinimalDeltaApplicable);
                }
                else
                {
                    MinimalDelta = parsedValue;
                    ClearErrors(nameof(MinimalDelta));
                }
            }
        }
        public bool IsMinimalDeltaApplicable => TerminateCondition == TerminateCondition.MinimalDelta;

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

        public RandomizationType RandomizationType
        {
            get => _model.ParameterSet.RandomizationType;
            set
            {
                _model.ParameterSet.RandomizationType = value;
                OnPropertyChanged(nameof(RandomizationType));

                //dependent parameters
                OnPropertyChanged(nameof(IsSeedApplicable));
                ClearErrors(nameof(Seed));
                if (IsSeedApplicable) SeedDisplay = Seed.ToString();
            }
        }
        public RandomizationType[] RandomizationTypeValues => Enum.GetValues<RandomizationType>();
        public int Seed
        {
            get => _model.ParameterSet.Seed;
            set
            {
                _model.ParameterSet.Seed = value;
            }
        }
        public string SeedDisplay
        {
            get => Seed.ToString();
            set
            {
                ClearErrors(nameof(Seed));

                if (value == string.Empty)
                {
                    Seed = 0;
                    AddError(nameof(Seed), "Seed field should not be empty", IsSeedApplicable);
                }
                else if (!int.TryParse(value, out int parsedValue) || parsedValue <= 0)
                {
                    Seed = 0;
                    AddError(nameof(Seed), "Seed field should contain a whole number", IsSeedApplicable);
                }
                else
                {
                    Seed = parsedValue;
                    ClearErrors(nameof(Seed));
                }
            }
        }
        public bool IsSeedApplicable => RandomizationType == RandomizationType.Seeded;

        public int InitialClusterNumber
        {
            get => _model.ParameterSet.InitialClusterNumber;
            set
            {
                _model.ParameterSet.InitialClusterNumber = value;
                OnPropertyChanged(nameof(InitialClusterNumber));
            }
        }
        public string InitialClusterNumberDisplay
        {
            get => InitialClusterNumber.ToString();
            set
            {
                ClearErrors(nameof(InitialClusterNumber));

                if (value == string.Empty)
                {
                    InitialClusterNumber = 0;
                    AddError(nameof(InitialClusterNumber), "Number of Clusters field should not be empty");
                }
                else if (!int.TryParse(value, out int parsedValue) || parsedValue <= 0)
                {
                    InitialClusterNumber = 0;
                    AddError(nameof(InitialClusterNumber), "Number of Clusters field should contain a positive whole number");
                }
                else
                {
                    InitialClusterNumber = parsedValue;
                    ClearErrors(nameof(InitialClusterNumber));
                }
            }
        }


        #endregion


        public ParametersViewModel(MainModel model, INavigationService setupNavigationService)
        {
            _model = model;
            _model.NewParameterSet();

            Errors = new ObservableCollection<PropertyError>();

            ApplyCommand = new RelayCommand(setupNavigationService.Navigate);
        }


        private void AddError(string propertyName, string message, bool? isApplicable = null)
        {
            if (isApplicable is not null && !isApplicable.Value) return;
            for (int i = 0; i < Errors.Count; i++)
            {
                if (Errors[i].PropertyName == propertyName)
                {
                    Errors[i].ErrorMessages.Add(message);
                    return;
                }
            }
            Errors.Add(new PropertyError(propertyName, message));
            OnPropertyChanged(nameof(HasErrors));
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
            OnPropertyChanged(nameof(HasErrors));
        }
    }
}
