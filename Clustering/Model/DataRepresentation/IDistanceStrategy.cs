using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.DataRepresentation
{
    internal interface IDistanceStrategy
    {
        double Distance(IDataPoint a, IDataPoint b);
    }
}
