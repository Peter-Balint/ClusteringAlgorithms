
namespace Clustering.Model.DataRepresentation
{
    public class RGBAPoint : IDataPoint
    {
        public int Id { get; }
        public int ClusterId {  get; set; }

        private byte[] _colorCoordinates = new byte[4];

        public int Dimension { get { return 4; } }

        public RGBAPoint(double[] coordinates, int clusterId = 0) 
        {
            for(int i = 1; i < 4; i++)
            {
                _colorCoordinates[i] = (byte)Math.Clamp(Math.Round(coordinates[i]), 0, 255);
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
                _colorCoordinates[i] = (byte)Math.Clamp(Math.Round(newCoordinates[i]), 0, 255);
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
