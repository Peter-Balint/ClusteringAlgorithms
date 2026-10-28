
namespace Clustering.Model
{
    public class ParameterSet
    {
        public DisplayMode DisplayMode { get; set; }
        public SpatialDistanceMetric SpatialDistanceMetric { get; set; }
        public CenterType CenterType { get; set; }
        public TerminateCondition TerminateCondition { get; set; }
        public int IterationNumber { get; set; }
        public double MinimalDelta { get; set; }
        public ClusterInitializationMethod ClusterInitializationMethod { get; set; }
        public RandomizationType RandomizationType { get; set; }
        public int Seed { get; set; }
        public int InitialClusterNumber{ get; set; }

        public Random GetRandomInstance()
        {
            if(RandomizationType == RandomizationType.Random) return new Random();
            else return new Random(Seed);
        }

        public bool HasBreakingChanges(ParameterSet? previous)
        {
            if (previous == null) return true;
            if (DisplayMode != previous.DisplayMode || ClusterInitializationMethod != previous.ClusterInitializationMethod) return true;
            return false;
        }

        public ParameterSet Copy()
        {
            ParameterSet ps = new ParameterSet();
            ps.DisplayMode = DisplayMode;
            ps.SpatialDistanceMetric = SpatialDistanceMetric;
            ps.CenterType = CenterType;
            ps.TerminateCondition = TerminateCondition;
            ps.IterationNumber = IterationNumber;
            ps.MinimalDelta = MinimalDelta;
            ps.ClusterInitializationMethod = ClusterInitializationMethod;
            ps.RandomizationType = RandomizationType;
            ps.Seed = Seed;
            ps.InitialClusterNumber = InitialClusterNumber;
            return ps;
        }
    }

    //add unset and description?
    public enum DisplayMode { Spatial2D, Spatial3D, Image }
    public enum SpatialDistanceMetric { Manhattan, Euclidean, Sup }
    public enum CenterType { Mean, Medoid }
    public enum TerminateCondition { IterationNumber, MinimalDelta }
    public enum ClusterInitializationMethod { RandomDataPoint, UserDefined }
    public enum RandomizationType { Random, Seeded } //techincally random is also seeded, but with very little chance of producing repeats

}
