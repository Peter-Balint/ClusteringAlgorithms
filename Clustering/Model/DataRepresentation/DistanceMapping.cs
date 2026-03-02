using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.DataRepresentation
{
    internal static class DistanceMapping
    {
        public static Type? GetApplicableDistanceStrategies(Type pointType)
        {
            if (pointType == typeof(SpatialPoint))
            {
                return typeof(ISpatialDistanceStrategy);
            }
            if (pointType == typeof(RGBAPoint))
            {
                return typeof(RGBADistance);
            }
            else return null;
        }

    }
}
