
using Clustering.Model;
using System.Windows.Controls;

namespace Clustering.ViewModel
{
    public class ParametersViewModel : ViewModelBase
    {
        private readonly MainModel _model;


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
                OnPropertyChanged(nameof(IterationNumber));
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
            DisplayMode = DisplayMode.Spatial3D;
        }
    }
}
