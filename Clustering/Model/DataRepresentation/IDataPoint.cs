namespace Clustering.Model.DataRepresentation
{
    public interface IDataPoint
    {
        public int Id { get; set; }
        public int Dimension { get; }

        public double GetCoordinateAt(int index);

        public void Modify(double[] coordinates);
    }
}
