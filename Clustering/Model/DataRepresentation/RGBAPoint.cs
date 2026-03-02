using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.DataRepresentation
{
    internal class RGBAPoint : IDataPoint
    {
        int[] colorCoordinates = new int[4];

        public int Dimension { get { return 4; } }

        public RGBAPoint(double[] coordinates) 
        {
            //some assertion that there are exactly four elements, or is all validation already done on the vm level?
            for(int i = 1; i < 4; i++)
            {
                colorCoordinates[i] = (int)coordinates[i];
            }
        }
    }
}
