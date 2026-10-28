

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

        public double GetCoordinateAt(int index)
        {
            return _coordinates[index];
        }
        public double[] GetAllCoordinates()
        {
            var copy = new double[Dimension];
            _coordinates.CopyTo(copy, 0);
            return copy;
        }

        public IDataPoint DeepCopy()
        {
            double[] copiedCoordinates = new double[Dimension];
            //can be changed to .copyTo
            //but shouldn't be merged with GetAllCoordinates
            //as this returns a typed variable through its interface, might be useful
            for(int i = 0; i < Dimension; i++)
            {
                copiedCoordinates[i] = _coordinates[i];
            }
            //with Id copied caution must be taken where the copy is used
            //main gist: don't read to the same collection
            return new SpatialPoint(copiedCoordinates,Dimension,Id,ClusterId);
        }
    }
}
