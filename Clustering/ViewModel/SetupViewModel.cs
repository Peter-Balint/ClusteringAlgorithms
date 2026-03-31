
using Clustering.Model;
using CommunityToolkit.Mvvm.Input;
using HelixToolkit.Wpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Clustering.ViewModel
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly MainModel _model;

        //in the lifecycle of this viewmodel the displaymode cannot change
        public DisplayMode DisplayMode => _model.ParameterSet.DisplayMode;

        //there probably should be unique viewmodels for the different displays
        //will they be reusable for results? let's try

        public ViewModelBase DisplayViewModel { get; private set; }

        //public ObservableCollection<SphereVisual3D> Spheres { get; set; } = [new SphereVisual3D() { Center = new Point3D(2, 0, 0), Radius = 1 }];

        public string TestText { get; }

        public ICommand TestCommand { get; }

        public SetupViewModel(MainModel model)
        {
            _model = model;

            DisplayViewModel = new Display2DViewModel();
            /*Spheres = new();
            Spheres.Add(new SphereVisual3D() { Center = new Point3D(0, 0, 0), Radius = 1 });
            TestText = "setupviewmodel is visible in component";*/
            //TestCommand = new RelayCommand(() => AddSphere());
        }

        /*private void AddSphere()
        {
            Spheres.Add(new SphereVisual3D() { Center = new Point3D(-2, 0, 0), Radius = 1 });
        }*/
    }
}
