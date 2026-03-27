using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Clustering.Model
{
    //maybe figure out a more generic way later
    public class ParameterSet
    {
        public DisplayMode DisplayMode { get; 
            set; }
        public SpatialDistanceMetric SpatialDistanceMetric { get; set; }
        public CenterType CenterType { get; set; }
        public TerminateCondition TerminateCondition { get; set; }
        public int IterationNumber { get; set; }
        public double MinimalDelta { get; set; }
        public ClusterInitializationMethod ClusterInitializationMethod { get; set; }
        public int InitialClusterNumber { get; set; }

    }

    //add unset and description?
    public enum DisplayMode { Spatial2D, Spatial3D, RGBA }
    public enum SpatialDistanceMetric { Manhattan = 1, Euclidean = 2, Sup = int.MaxValue /*will it be useful to number them?*/ };
    public enum CenterType { Mean, Medoid }
    public enum TerminateCondition { IterationNumber, MinimalDelta }
    public enum ClusterInitializationMethod { Random, UserDefined }
}
