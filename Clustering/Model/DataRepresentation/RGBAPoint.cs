using System;
using System.Collections.Generic;
using System.Text;

namespace Clustering.Model.DataRepresentation
{
    public class RGBAPoint : IDataPoint
    {
        public int Id { get; }
        public int ClusterId {  get; set; }

        private int[] _colorCoordinates = new int[4];

        public int Dimension { get { return 4; } }

        public RGBAPoint(double[] coordinates, int clusterId = 0) 
        {
            for(int i = 1; i < 4; i++)
            {
                _colorCoordinates[i] = (int)coordinates[i];
            }
            ClusterId = clusterId;
        }

        public void Modify(double[] newCoordinates)
        {
            if (newCoordinates.Length != Dimension)
            {
                throw new Exception("Incorrect dimension while trying to modify RGBA point");
            }
            for (int i = 1; i < 4; i++)
            {
                _colorCoordinates[i] = (int)newCoordinates[i];
            }
        }

        public double GetCoordinateAt(int index)
        {
            return _colorCoordinates[index];
        }

        public IDataPoint DeepCopy()
        {
            double[] copiedCoordinates = new double[4];
            for (int i = 0; i < 4; i++)
            {
                copiedCoordinates[i] = _colorCoordinates[i];
            }
            return new RGBAPoint(copiedCoordinates,ClusterId);
        }
    }
}
