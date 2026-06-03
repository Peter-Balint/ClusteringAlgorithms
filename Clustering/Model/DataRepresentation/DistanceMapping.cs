
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
            if (pointType == typeof(LabColorPoint))
            {
                return typeof(LabDistance);
            }
            else return null;
        }

    }
}
