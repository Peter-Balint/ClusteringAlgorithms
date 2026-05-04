
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

    }

    //add unset and description?
    public enum DisplayMode { Spatial2D, Spatial3D, RGBA }
    public enum SpatialDistanceMetric { Manhattan, Euclidean, Sup }
    public enum CenterType { Mean, Medoid }
    public enum TerminateCondition { IterationNumber, MinimalDelta }
    public enum ClusterInitializationMethod { RandomDataPoint, UserDefined }
    public enum RandomizationType { Random, Seeded } //techincally random is also seeded, but with very little chance of producing repeats

}
