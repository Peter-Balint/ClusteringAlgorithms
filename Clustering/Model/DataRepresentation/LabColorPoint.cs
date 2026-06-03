
using Colourful;

namespace Clustering.Model.DataRepresentation
{
    public class LabColorPoint : IDataPoint
    {
        public int Id { get; }
        public int ClusterId {  get; set; }

        private LabColor lab;

        public int Dimension { get { return 3; } }

        public LabColorPoint(double[] coordinates, int id, int clusterId = 0) 
        {
            lab = new LabColor(coordinates);
            Id = id;
            ClusterId = clusterId;
        }
        public LabColorPoint(LabColor labColor, int id, int clusterId = 0)
        {
            lab = labColor;
            Id = id;
            ClusterId = clusterId;
        }

        /// <param name="index"> L=0,a=1,b=2 </param>
        public double GetCoordinateAt(int index)
        {
            return lab.Vector[index];
        }
        public double[] GetAllCoordinates()
        {
            return lab.Vector;
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
