namespace Clustering.Model.DataRepresentation
{
    internal interface IDataPoint
    {
        public int Id { get; set; }
        public int Dimension { get; }

        public void Modify(double[] coordinates);
    }
}
