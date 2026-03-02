

namespace Clustering.Model.DataRepresentation
{
    internal class SpatialPoint : IDataPoint
    {
        public SpatialPoint(double[] coordinates) { }

        int IDataPoint.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        int IDataPoint.Dimension => throw new NotImplementedException();

        void IDataPoint.Modify(double[] coordinates)
        {
            throw new NotImplementedException();
        }
    }
}
