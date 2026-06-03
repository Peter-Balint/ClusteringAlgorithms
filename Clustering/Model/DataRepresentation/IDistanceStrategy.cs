
using Colourful;

namespace Clustering.Model.DataRepresentation
{
    internal interface IDistanceStrategy
    {
        double Distance(IDataPoint a, IDataPoint b);
    }
    internal interface ISpatialDistanceStrategy : IDistanceStrategy{}

    internal class ManhattanDistance : ISpatialDistanceStrategy
    {
        public double Distance(IDataPoint a, IDataPoint b)
        {
            double distance = 0;
            for(int i=0; i<a.Dimension; i++)
            {
                distance += Math.Abs(a.GetCoordinateAt(i) - b.GetCoordinateAt(i));
            }
            return distance;
        }
    }
    internal class EuclideanDistance : ISpatialDistanceStrategy
    {
        public double Distance(IDataPoint a, IDataPoint b)
        {
            double distance = 0;
            for (int i = 0; i < a.Dimension; i++)
            {
                distance += Math.Pow(a.GetCoordinateAt(i) - b.GetCoordinateAt(i),2);
            }
            return Math.Sqrt(distance);
        }
    }
    internal class SupDistance : ISpatialDistanceStrategy
    {
        public double Distance(IDataPoint a, IDataPoint b)
        {
            double maxDistance = 0;
            double distance;
            for (int i = 0; i < a.Dimension; i++)
            {
                distance = Math.Abs(a.GetCoordinateAt(i) - b.GetCoordinateAt(i));
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }
            return maxDistance;
        }
    }

    internal class LabDistance : IDistanceStrategy
    {
        private readonly CIEDE2000ColorDifference diffCalculator = new CIEDE2000ColorDifference();
        public double Distance(IDataPoint colorOne, IDataPoint colorTwo)
        {
            return diffCalculator.ComputeDifference(new LabColor(colorOne.GetAllCoordinates()), new LabColor(colorTwo.GetAllCoordinates()));
        }
    }
}
