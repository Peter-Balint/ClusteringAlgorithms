using Clustering.ViewModel;
using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;


namespace Clustering.View.Components
{
    public partial class ResultsSpatial3D : UserControl
    {
        private ResultsSpatial3DViewModel _viewModel => (ResultsSpatial3DViewModel)DataContext;
        private int _currentStep = 0;
        private int _stepCount = 0;

        private List<SphereVisual3D[]> _stepSpheres = new List<SphereVisual3D[]>();
        private List<CubeVisual3D[]> _stepCenters = new List<CubeVisual3D[]>();

        SolidColorBrush[] brushes = [Brushes.Red, Brushes.Pink, Brushes.Blue, Brushes.Green, Brushes.Yellow, Brushes.Turquoise];

        public ResultsSpatial3D()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not ResultsSpatial3DViewModel vm)
                return;

            _viewModel.StepAdded += OnStepAdded;
            _viewModel.LoadResults();

            DataContextChanged -= OnDataContextChanged;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.StepAdded -= OnStepAdded;
            }

            Unloaded -= OnUnloaded;
        }

        private void OnStepAdded(object? sender, (SphereViewModel[] points, CubeViewModel[] centers) args)
        {
            
            SphereVisual3D[] spheres = new SphereVisual3D[args.points.Length];
            CubeVisual3D[] centers = new CubeVisual3D[args.centers.Length];
            for(int i = 0; i < args.points.Length; i++)
            {
                spheres[i] = new SphereVisual3D
                {
                    Center = new Point3D(args.points[i].X, args.points[i].Y, args.points[i].Z),
                    Radius = 0.5,
                    Fill = brushes[args.points[i].ClusterId % brushes.Length]
                };
                if(_stepCount == 0) Viewport.Children.Add(spheres[i]);
            }
            for (int i = 0; i < args.centers.Length; i++)
            {
                centers[i] = new CubeVisual3D
                {
                    Center = new Point3D(args.centers[i].X, args.centers[i].Y, args.centers[i].Z),
                    SideLength = 0.5,
                    Fill = Brushes.White
                };
                if (_stepCount == 0) Viewport.Children.Add(centers[i]);
            }

            _stepSpheres.Add(spheres);
            _stepCenters.Add(centers);
            _stepCount++;
        }

        private void StepBackButton_Click(object? sender, RoutedEventArgs e)
        {
            if (_currentStep <= 0) return;
            SphereVisual3D[] currentSpheres = _stepSpheres.ElementAt(_currentStep);
            foreach(var sphere in currentSpheres)
            {
                Viewport.Children.Remove(sphere);
            }
            CubeVisual3D[] currentCenters = _stepCenters.ElementAt(_currentStep);
            foreach(var cube in currentCenters)
            {
                Viewport.Children.Remove(cube);
            }

            _currentStep--;
            currentSpheres = _stepSpheres.ElementAt(_currentStep);
            foreach (var sphere in currentSpheres)
            {
                Viewport.Children.Add(sphere);
            }
            currentCenters = _stepCenters.ElementAt(_currentStep);
            foreach (var cube in currentCenters)
            {
                Viewport.Children.Add(cube);
            }

            CurrentStepTextBlock.Text = $"Current step: {_currentStep}";
        }
        private void StepForwardButton_Click(Object? sender, RoutedEventArgs e)
        {
            if (_currentStep >= _stepCount-1) return;
            SphereVisual3D[] currentSpheres = _stepSpheres.ElementAt(_currentStep);
            foreach (var sphere in currentSpheres)
            {
                Viewport.Children.Remove(sphere);
            }
            CubeVisual3D[] currentCenters = _stepCenters.ElementAt(_currentStep);
            foreach (var cube in currentCenters)
            {
                Viewport.Children.Remove(cube);
            }

            _currentStep++;
            currentSpheres = _stepSpheres.ElementAt(_currentStep);
            foreach (var sphere in currentSpheres)
            {
                Viewport.Children.Add(sphere);
            }
            currentCenters = _stepCenters.ElementAt(_currentStep);
            foreach (var cube in currentCenters)
            {
                Viewport.Children.Add(cube);
            }

            CurrentStepTextBlock.Text = $"Current step: {_currentStep}";
        }
    }
}
