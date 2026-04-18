namespace Clustering.Model.DataRepresentation
{
    public interface IDataPoint
    {
        public int Id { get; }
        public int Dimension { get; }
        public int ClusterId {  get; set; } 

        public double GetCoordinateAt(int index);
        public IDataPoint DeepCopy();
    }
}
