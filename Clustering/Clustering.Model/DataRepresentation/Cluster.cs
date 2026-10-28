
namespace Clustering.Model.DataRepresentation
{
    public class Cluster
    {
        public int Id { get; }

        public IDataPoint CenterPoint;

        public Cluster(int id, IDataPoint? centerPoint = null)
        {
            Id = id;
            if(centerPoint is not null) CenterPoint = centerPoint;
        }
    }
}
