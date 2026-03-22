
using Clustering.Model;
using HelixToolkit.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace Clustering.ViewModel
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        //in the lifecycle of this viewmodel the displaymode cannot change
        public DisplayMode DisplayMode => _model.ParameterSet.DisplayMode;

        public ObservableCollection<SphereVisual3D> Spheres { get; }

        public string TestText { get; }

        public SetupViewModel(MainModel model)
        {
            _model = model;
            Spheres = new();
            Spheres.Add(new SphereVisual3D() { Center = new Point3D(0, 0, 0), Radius = 1 });
            TestText = "setupviewmodel is visible in component";
        }
    }
}
