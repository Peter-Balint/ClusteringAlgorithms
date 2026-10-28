
using Clustering.ViewModel;
using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Clustering.View.Components
{
    public partial class SetupSpatial3D : UserControl
    {
        private readonly Dictionary<int,SphereVisual3D> _spheres = new();
        private readonly Random _random = new();
        private SetupSpatial3DViewModel _viewModel => (SetupSpatial3DViewModel)DataContext;

        private SphereVisual3D? _selectedSphere = null;

        public SetupSpatial3D()
        {
            InitializeComponent();
            RemoveButton.Visibility = Visibility.Collapsed;

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not SetupSpatial3DViewModel vm)
                return;

            _viewModel.PointAdded += OnPointAdded;
            _viewModel.PointsAdded += OnPointsAdded;
            _viewModel.AddInitialPoints();

            DataContextChanged -= OnDataContextChanged;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PointAdded -= OnPointAdded;
                _viewModel.PointsAdded -= OnPointsAdded;
            }

            Unloaded -= OnUnloaded;
        }

        private void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(XTextBox.Text, out double x) && 
                double.TryParse(YTextBox.Text, out double y) && 
                double.TryParse(ZTextBox.Text, out double z)) {
                _viewModel.AddPoint(x, y, z);
            } 
            else { MessageBox.Show("Please enter valid numeric coordinates."); }
        }

        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(Viewport);

            var hits = Viewport.Viewport.FindHits(position);

            if (hits.Count == 0) return;

            var hit = hits[0];

            if (hit.Visual is SphereVisual3D sphere)
            {
                if (_selectedSphere is not null)
                {
                    _selectedSphere.Fill = Brushes.Red;
                }
                _selectedSphere = sphere;
                _selectedSphere.Fill = Brushes.Pink;
                RemoveButton.Visibility = Visibility.Visible;
            }
        }

        private void RemovePointButton_Click(object sender, RoutedEventArgs e)
        {
            //Remove flow is different from creation, look at it todo, noted in SetupSpatial2DViewModel
            _viewModel.RemovePoint(_spheres.Where(pair => pair.Value == _selectedSphere).First().Key);
            Viewport.Children.Remove(_selectedSphere);
            _selectedSphere = null;
            RemoveButton.Visibility= Visibility.Collapsed;
        }

        private void LoadPointsButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadPoints();
        }

        private void OnPointAdded(object? sender, SphereViewModel point)
        {
            AddSphere(point);
        }
        private void OnPointsAdded(object? sender, SphereViewModel[] points)
        {
            //assuming Loading a new set of points is the only multi-point creation
            //same as in the model
            ClearSpheres();
            AddSpheres(points);
        }

        private void AddSphere(SphereViewModel point)
        {
            var sphere = new SphereVisual3D
            {
                Center = new Point3D(point.X, point.Y, point.Z),
                Radius = 0.5,
                Fill = Brushes.Red
            };

            _spheres.Add(point.Id, sphere);
            Viewport.Children.Add(sphere);
        }
        private void AddSpheres(IEnumerable<SphereViewModel> spheres)
        {
            foreach (SphereViewModel sphere in spheres)
            {
                AddSphere(sphere);
            }
        }
        private void ClearSpheres()
        {
            foreach(SphereVisual3D sphere in _spheres.Values)
            {
                Viewport.Children.Remove(sphere);
            }
            _spheres.Clear();
        }
    }
}
