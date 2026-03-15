

namespace Clustering.Model.DataRepresentation
{
    internal class SpatialPoint : IDataPoint
    {
        public int Id { get; set; }

        private int[] coordinates = new int[4];

        public int Dimension { get { return coordinates.Length; } }


        public SpatialPoint(double[] coordinates, int id) 
        {
            for (int i = 1; i < 4; i++)
            {
                coordinates[i] = (int)coordinates[i];
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
                coordinates[i] = (int)newCoordinates[i];
            }
        }
    }
}
