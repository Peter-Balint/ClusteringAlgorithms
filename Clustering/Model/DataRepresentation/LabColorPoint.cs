
using Colourful;

namespace Clustering.Model.DataRepresentation
{
    public class LabColorPoint : IDataPoint
    {
        public int Id { get; }
        public int ClusterId {  get; set; }

        private LabColor lab;

        public int Dimension { get { return 3; } }

        public LabColorPoint(double[] coordinates, int clusterId = 0) 
        {
            lab = new LabColor(coordinates);
            ClusterId = clusterId;
        }
        public LabColorPoint(LabColor labColor, int clusterId = 0)
        {
            lab = labColor;
            ClusterId = clusterId;
        }

        /*public void Modify(double[] newCoordinates)
        {
            if (newCoordinates.Length != Dimension)
            {
                throw new Exception("Incorrect dimension while trying to modify RGBA point");
            }
            for (int i = 1; i < 4; i++)
            {
                _colorCoordinates[i] = (byte)Math.Clamp(Math.Round(newCoordinates[i]), 0, 255);
            }
        }*/

        /// <param name="index"> L=0,a=1,b=2 </param>
        public double GetCoordinateAt(int index)
        {
            return lab.Vector[index];
        }

        public IDataPoint DeepCopy()
        {
            double[] copiedCoordinates = new double[3];
            var colorCoordinates = lab.Vector;
            for (int i = 0; i < 3; i++)
            {
                copiedCoordinates[i] = colorCoordinates[i];
            }
            return new LabColorPoint(copiedCoordinates,ClusterId);
        }
    }
}
