
using Clustering.Model.DataRepresentation;

namespace Clustering.Model.Algorithm
{
    internal class KMeansAlgorithm : IAlgorithm
    {
        private readonly ParameterSet _parameters;
        private IDistanceStrategy _distanceStrategy;

        public KMeansAlgorithm(ParameterSet parameters)
        {
            _parameters = parameters;
            if (parameters.DisplayMode == DisplayMode.Spatial2D || parameters.DisplayMode == DisplayMode.Spatial3D)
            {
                switch (parameters.SpatialDistanceMetric)
                {
                    case SpatialDistanceMetric.Euclidean:
                        {
                            _distanceStrategy = new EuclideanDistance();
                            break;
                        }
                    case SpatialDistanceMetric.Manhattan:
                        {
                            _distanceStrategy = new ManhattanDistance();
                            break;
                        }
                    case SpatialDistanceMetric.Sup:
                        {
                            _distanceStrategy = new SupDistance();
                            break;
                        }
                }
            }
            else _distanceStrategy = new RGBADistance();
        }

        public StepSequence Run(PointCloud pointCloud)
        {
            StepSequence results = new StepSequence();

            //add initial clusters, with randomized center elements from points
            List<int> centerIds = new List<int>();
            int[] pointIds = pointCloud.GetIds();
            Random rand = new Random();
            for(int i = 0; i < _parameters.InitialClusterNumber; i++)
            {
                int index = rand.Next(pointIds.Length);
                while (centerIds.Contains(pointIds[index])){
                    index = rand.Next(pointIds.Length);
                }
                centerIds.Add(pointIds[index]);
            }
            for (int i = 0; i < _parameters.InitialClusterNumber; i++)
            {
                pointCloud.AddCluster(i, pointCloud.GetPoint(centerIds[i]));
            }

            IDataPoint[] points = pointCloud.GetAllPoints().ToArray();
            Cluster[] clusters = pointCloud.GetClusters().ToArray();
            results.SaveStep(points, clusters);

            do
            {
                RedistributePoints(points, clusters);
                AssignNewCenters(points, clusters);

                results.SaveStep(points, clusters);
            } while (!IsTerminateConditionFullfilled(results));

        }

        private void RedistributePoints(IDataPoint[] points, Cluster[] clusters)
        {
            double minDistance;
            Cluster closestCluster;
            double distance;

            foreach (IDataPoint point in points)
            {
                minDistance = 0;
                closestCluster = null!;
                foreach (Cluster cluster in clusters)
                {
                    distance = _distanceStrategy.Distance(point, cluster.CenterPoint);
                    if(distance >= minDistance)
                    {
                        minDistance = distance;
                        closestCluster = cluster;
                    }
                }
                point.ClusterId = closestCluster.Id;
            }
        }

        private bool IsTerminateConditionFullfilled(StepSequence pastSteps)
        {
            switch (_parameters.TerminateCondition)
            {
                case TerminateCondition.IterationNumber:
                    {
                        if (pastSteps.GetStepCount() == _parameters.IterationNumber) return true;
                        else return false;
                    }
                case TerminateCondition.MinimalDelta:
                    {
                        double delta = 0;
                        //todo: calculate delta of the last two steps
                        if (delta < _parameters.MinimalDelta) return true;
                        else return false;
                    }
                default: throw new Exception("Unexpected value in parameter Terminate Condition");
            }
        }

        private void AssignNewCenters(IDataPoint[] points, Cluster[] clusters)
        {
            IDataPoint[] pointsInCluster;
            //nem euklidészi metrikánál ugyanaz az átlag?
            foreach(Cluster cluster in clusters)
            {
                pointsInCluster = points.Where((p) => p.ClusterId == cluster.Id).ToArray();


            }
        }
    }
}
