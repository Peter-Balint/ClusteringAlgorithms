

namespace Clustering.Model.DataRepresentation
{
    internal class SpatialPoint : IDataPoint
    {
        public int Id { get; }
        public int ClusterId {  get; set; }

        private double[] _coordinates;

        public int Dimension { get { return _coordinates.Length; } }


        public SpatialPoint(double[] coordinates, int dimension, int id, int clusterId = 0) 
        {
            _coordinates = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                _coordinates[i] = coordinates[i];
            }
            Id = id;
            ClusterId = clusterId;
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

        public IDataPoint DeepCopy()
        {
            double[] copiedCoordinates = new double[Dimension];
            for(int i = 0; i < Dimension; i++)
            {
                copiedCoordinates[i] = _coordinates[i];
            }
            //with Id copied caution must be taken where the copy is used
            //main gist: don't readd to the same collection
            return new SpatialPoint(copiedCoordinates,Dimension,Id,ClusterId);
        }
    }
}
