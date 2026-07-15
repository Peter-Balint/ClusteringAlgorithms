
using Clustering.ViewModel;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Clustering.View.Components
{
    /// <summary>
    /// Interaction logic for SetupSpatial3D.xaml
    /// </summary>
    public partial class SetupSpatial3D : UserControl
    {
        private readonly List<SphereVisual3D> _spheres = new();
        private readonly Random _random = new();
        private SetupSpatial3DViewModel _viewModel => (SetupSpatial3DViewModel)DataContext;

        public SetupSpatial3D()
        {
            InitializeComponent();
        }

        private void AddSphere(Point3D center, double radius, Brush fill)
        {
            var sphere = new SphereVisual3D
            {
                Center = center,
                Radius = radius,
                Fill = fill
            };

            _spheres.Add(sphere);
            Viewport.Children.Add(sphere);
        }

        private void AddSphereButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(XTextBox.Text, out double x) && 
                double.TryParse(YTextBox.Text, out double y) && 
                double.TryParse(ZTextBox.Text, out double z)) {
                AddSphere(new Point3D(x, y, z), 0.5, Brushes.Red);
                _viewModel.AddPoint(x, y, z);
            } 
            else { MessageBox.Show("Please enter valid numeric coordinates."); }
        }

        private void LoadPointsButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.Filter = "Setup files (*.sup)|*.sup";
            if (fd.ShowDialog() == false)
            {
                return;
            }
            using var reader = new StreamReader(fd.FileName);
            List<double[]> pointsFromFile = new List<double[]>();
            int lineCounter = 1;
            string? line = reader.ReadLine();
            if (line is null)
            {
                MessageBox.Show($"Opened file was empty:\n{fd.FileName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            line = line.Trim();
            if (int.TryParse(line, out int dimension) == false)
            {
                MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nDimension of points was not declared", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dimension < 2)
            {
                MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nDimension of points must be at least 2", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            while ((line = reader.ReadLine()) is not null)
            {
                lineCounter++;
                string[] splits = line.Split(';');
                if (splits.Length != dimension)
                {
                    MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nNumber of coordinates of point didn't match the declared dimension", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                double[] coordinates = new double[dimension];
                for (int i = 0; i < dimension; i++)
                {
                    if (double.TryParse(splits[i], out double coordinate) == false)
                    {
                        MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nCoordinate could not be parsed: {splits[i]}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    coordinates[i] = coordinate;
                }
                pointsFromFile.Add(coordinates);
            }
            foreach (double[] coordinates in pointsFromFile)
            {
                AddSphere(new Point3D(coordinates[0], coordinates[1], coordinates[2]), 0.5, Brushes.Red);
            }
        }
    }
}
