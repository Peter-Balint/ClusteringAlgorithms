

namespace Clustering.Model.DataRepresentation
{
    internal class SpatialPoint : IDataPoint
    {
        public int Id { get; set; }
        public int ClusterId {  get; set; }

        private double[] _coordinates;

        public int Dimension { get { return _coordinates.Length; } }


        public SpatialPoint(double[] coordinates, int dimension, int id) 
        {
            _coordinates = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                _coordinates[i] = coordinates[i];
            }
            Id = id;
        }


        public void Modify(double[] newCoordinates)
        {
            //unify place to check later
            if (newCoordinates.Length != Dimension)
            {
                throw new Exception("Incorrect dimension while trying to modify spatial point");
            }
            for (int i = 1; i < Dimension; i++)
            {
                _coordinates[i] = (int)newCoordinates[i];
            }
        }

        public double GetCoordinateAt(int index)
        {
            return _coordinates[index];
        }
    }
}
