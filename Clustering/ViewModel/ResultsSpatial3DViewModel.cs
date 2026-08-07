
using Clustering.Model;
using Clustering.Model.DataRepresentation;

namespace Clustering.ViewModel
{
    internal class ResultsSpatial3DViewModel : ViewModelBase
    {
        private MainModel _model;

        public event EventHandler<(SphereViewModel[] points, CubeViewModel[] centers)>? StepAdded;

        public ResultsSpatial3DViewModel(MainModel model)
        {
            _model = model;
        }

        public void LoadResults()
        {
            if (!_model.ResultsAvailable) return;
            int stepCount = _model.Results!.GetStepCount();
            for(int i = 0; i < stepCount; i++)
            {
                (IDataPoint[] stepPoints, Cluster[] stepClusters) = _model.Results!.GetStepAt(i);
                var pointVMs = new SphereViewModel[stepPoints.Length];
                var centerVMs = new CubeViewModel[stepClusters.Length];
                for(int j = 0; j < stepPoints.Length; j++)
                {
                    pointVMs[j] = new SphereViewModel(stepPoints[j]);
                }
                for (int j = 0; j < stepClusters.Length; j++)
                {
                    centerVMs[j] = new CubeViewModel(stepClusters[j].CenterPoint);
                }
                StepAdded?.Invoke(this, (pointVMs,centerVMs));
            }
        }
    }
}
