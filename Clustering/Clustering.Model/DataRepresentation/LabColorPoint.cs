
using Colourful;

namespace Clustering.Model.DataRepresentation
{
    public class LabColorPoint : IDataPoint
    {
        public int Id { get; }
        public int ClusterId { get; set; }

        private LabColor _lab;

        public int Dimension { get { return 3; } }

        public LabColorPoint(double[] coordinates, int id, int clusterId = 0)
        {
            _lab = new LabColor(coordinates);
            Id = id;
            ClusterId = clusterId;
        }

        /// <param name="index"> L=0,a=1,b=2 </param>
        public double GetCoordinateAt(int index)
        {
            return _lab.Vector[index];
        }
        public double[] GetAllCoordinates()
        {
            return _lab.Vector;
        }

        public IDataPoint DeepCopy()
        {
            double[] copiedCoordinates = new double[3];
            var colorCoordinates = _lab.Vector;
            for (int i = 0; i < 3; i++)
            {
                copiedCoordinates[i] = colorCoordinates[i];
            }
            return new LabColorPoint(copiedCoordinates, Id, ClusterId);
        }
    }
}
