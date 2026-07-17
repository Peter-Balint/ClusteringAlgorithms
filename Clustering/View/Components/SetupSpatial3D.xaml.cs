
using Clustering.ViewModel;
using Clustering.ViewModel.FileHandling;
using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Clustering.View.Components
{
    /// <summary>
    /// Interaction logic for SetupSpatial3D.xaml
    /// For now not fully linked with the vm and model
    /// </summary>
    public partial class SetupSpatial3D : UserControl
    {
        private readonly List<SphereVisual3D> _spheres = new();
        private readonly Random _random = new();
        private SetupSpatial3DViewModel _viewModel => (SetupSpatial3DViewModel)DataContext;

        private SphereVisual3D? _selectedSphere = null;

        public SetupSpatial3D()
        {
            InitializeComponent();
            RemoveButton.Visibility = Visibility.Collapsed;
        }

        private void AddSphere(Point3D center, double radius, Brush fill)
        {
            var sphere = new SphereVisual3D
            {
                Center = center,
                Radius = radius,
                Fill = fill
            };

            _spheres.Add(sphere); //most még nem csinál sokat, majd id-val tároljuk
            Viewport.Children.Add(sphere);
        }

        private void AddPointButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(XTextBox.Text, out double x) && 
                double.TryParse(YTextBox.Text, out double y) && 
                double.TryParse(ZTextBox.Text, out double z)) {
                AddSphere(new Point3D(x, y, z), 0.5, Brushes.Red);
                _viewModel.AddPoint(x, y, z);
                //majd itt a flow más lesz:
                //elküldi a vmodellnek a koordinátákat, visszakapja onnnan eventtel és id-val, akkor hozza létre
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
            Viewport.Children.Remove(_selectedSphere);
            _selectedSphere = null;
            RemoveButton.Visibility= Visibility.Collapsed;
        }

        private void LoadPointsButton_Click(object sender, RoutedEventArgs e)
        {
            //ez is lehet vm-be megy majd
            //see the 2d implementation
            var pointsFromFile = PointsLoader.Load(3);
            foreach (double[] coordinates in pointsFromFile)
            {
                AddSphere(new Point3D(coordinates[0], coordinates[1], coordinates[2]), 0.5, Brushes.Red);
            }
        }
    }
}
