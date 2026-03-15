using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
    internal class SupDistance : ISpatialDistanceStrategy
    {
        public double Distance(IDataPoint a, IDataPoint b)
        {
            throw new NotImplementedException();
        }
    }

    internal class RGBADistance : IDistanceStrategy
    {
        public double Distance(IDataPoint a, IDataPoint b)
        {
            throw new NotImplementedException();
        }
    }
}
