using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.DataRepresentation
{
    public class RGBAPoint : IDataPoint
    {
        public int Id { get; set; }

        private int[] colorCoordinates = new int[4];

        public int Dimension { get { return 4; } }

        public RGBAPoint(double[] coordinates) 
        {
            //some assertion that there are exactly four elements, or is all validation already done on the vm level?
            for(int i = 1; i < 4; i++)
            {
                colorCoordinates[i] = (int)coordinates[i];
            }
        }

        public void Modify(double[] newCoordinates)
        {
            if (newCoordinates.Length != Dimension)
            {
                throw new Exception("Incorrect dimension while trying to modify RGBA point");
            }
            for (int i = 1; i < 4; i++)
            {
                colorCoordinates[i] = (int)newCoordinates[i];
            }
        }
    }
}
