
using Clustering.Model;
using Clustering.Model.DataRepresentation;
using Clustering.ViewModel.FileHandling;
using Clustering.ViewModel.Navigation;
using System.Numerics;

namespace Clustering.ViewModel
{
    internal class SetupSpatial3DViewModel : ViewModelBase
    {
        private MainModel _mainModel;
        private INavigationService _progressNavigationService;

        public bool IsRunnable => _mainModel.IsRunnable;

        public event EventHandler<SphereViewModel>? PointAdded;
        public event EventHandler<SphereViewModel[]>? PointsAdded;

        public SetupSpatial3DViewModel(MainModel model, INavigationService progressNavigationService)
        {
            _mainModel = model;
            _progressNavigationService = progressNavigationService;

            _mainModel.PointCreated += OnPointCreated;
            _mainModel.PointsCreated += OnPointsCreated;
        }

        public void AddInitialPoints()
        {
            IDataPoint[] modelPoints = _mainModel.GetAllPoints().ToArray();
            OnPointsCreated(null, modelPoints);
        }

        public void AddPoint(double x, double y, double z)
        {
            _mainModel.AddPoint([x, y, z]);
        }
        public void RemovePoint(int id)
        {
            _mainModel.RemovePoint(id);
        }

        private void OnPointCreated(object? sender, IDataPoint point)
        {
            PointAdded?.Invoke(this, new SphereViewModel(point));
        }
        private void OnPointsCreated(object? sender, IDataPoint[] points)
        {
            SphereViewModel[] spheres = new SphereViewModel[points.Length];
            for (int i = 0; i < spheres.Length; i++){
                spheres[i] = new SphereViewModel(points[i]);
            }
            PointsAdded?.Invoke(this, spheres);
        }

        public void LoadPoints()
        {
            var pointsFromFile = PointsLoader.Load(3);
            if (pointsFromFile.Count == 0) return;
            if (pointsFromFile.First().Length > 3)
            {
                _mainModel.NewPointSetFromHigherDimension(pointsFromFile.ToArray(), 3);
            }
            else
            {
                _mainModel.NewPointSet(pointsFromFile);
            }
        }

        private void RunClustering()
        {
        }

    }
}
