
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
            else _distanceStrategy = new LabDistance();
        }

        public StepSequence Run(PointCloud pointCloud)
        {
            StepSequence results = new StepSequence();

            //add initial clusters, with randomized center elements from points
            List<int> centerIds = new List<int>();
            int[] pointIds = pointCloud.GetIds();
            Random rand = _parameters.GetRandomInstance();
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
            results.SaveStepCopy(points, clusters);

            do
            {
                //is this in the right order?
                RedistributePoints(points, clusters);
                AssignNewCenters(points, clusters, pointCloud.GetPointFactoryMethod());

                results.SaveStepCopy(points, clusters);
            } while (!IsTerminateConditionFullfilled(results));

            return results;
        }

        private void RedistributePoints(IDataPoint[] points, Cluster[] clusters)
        {
            double minDistance;
            Cluster closestCluster;
            double distance;

            foreach (IDataPoint point in points)
            {
                minDistance = double.MaxValue;
                closestCluster = null!;
                foreach (Cluster cluster in clusters)
                {
                    distance = _distanceStrategy.Distance(point, cluster.CenterPoint);
                    if(distance <= minDistance)
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
                        double maxDelta = 0; double delta;
                        int stepCount = pastSteps.GetStepCount();
                        if (stepCount < 2) return false;
                        Cluster[] lastStepClusters = pastSteps.GetStepAt(stepCount-1).Item2;
                        Cluster[] stepBeforeClusters = pastSteps.GetStepAt(stepCount-2).Item2;
                        for(int i = 0; i < lastStepClusters.Length; i++)
                        {
                            delta = _distanceStrategy.Distance(lastStepClusters[i].CenterPoint, stepBeforeClusters[i].CenterPoint);
                            if (delta > maxDelta) maxDelta = delta;
                        }
                        if (maxDelta < _parameters.MinimalDelta) return true;
                        else return false;
                    }
                default: throw new Exception("Unexpected value in parameter Terminate Condition");
            }
        }

        private void AssignNewCenters(IDataPoint[] points, Cluster[] clusters, Func<double[], int, IDataPoint> CreatePoint)
        {
            IDataPoint[] pointsInCluster;
            int pointNumber;
            //nem euklidészi metrikánál ugyanaz az átlag?
            double[] centerCoordinates;
            foreach(Cluster cluster in clusters)
            {
                centerCoordinates = new double[points[0].Dimension];
                pointsInCluster = points.Where((p) => p.ClusterId == cluster.Id).ToArray();
                pointNumber = pointsInCluster.Length;
                foreach(IDataPoint point in pointsInCluster)
                {
                    for(int i=0; i<point.Dimension; i++)
                    {
                        centerCoordinates[i] += point.GetCoordinateAt(i)/pointNumber;
                    }
                }
                if(_parameters.CenterType == CenterType.Mean)
                {
                    //cluster centers don't need their own id when they are not actual points, and if they are they aren't assigned like this
                    cluster.CenterPoint = CreatePoint(centerCoordinates, -1);
                }
                else if(_parameters.CenterType == CenterType.Medoid)
                {
                    //what happens if cluster is empty? for now not possible, but could become problematic later
                    IDataPoint centerMean = CreatePoint(centerCoordinates, -1);
                    IDataPoint bestRepresentative = pointsInCluster[0];
                    double minDistance = _distanceStrategy.Distance(bestRepresentative, centerMean);
                    double distance;
                    for(int i=1;i<pointsInCluster.Length;i++)
                    {
                        distance = _distanceStrategy.Distance(pointsInCluster[i], centerMean);
                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            bestRepresentative = pointsInCluster[i];
                        }
                    }
                    cluster.CenterPoint = bestRepresentative;
                }
            }
        }
    }
}
