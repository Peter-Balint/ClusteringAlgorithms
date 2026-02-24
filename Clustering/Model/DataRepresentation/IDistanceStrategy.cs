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
            throw new NotImplementedException();
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
