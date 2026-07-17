
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Clustering.ViewModel.FileHandling
{
    internal static class PointsLoader
    {
        public static List<double[]> Load(int minimumDimension)
        {
            FileDialog fd = new OpenFileDialog();
            fd.Filter = "Setup files (*.sup)|*.sup";
            if (fd.ShowDialog() == false)
            {
                return [];
            }
            using var reader = new StreamReader(fd.FileName);
            List<double[]> pointsFromFile = new List<double[]>();
            int lineCounter = 1;
            string? line = reader.ReadLine();
            if (line is null)
            {
                MessageBox.Show($"Opened file was empty:\n{fd.FileName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return [];
            }
            line = line.Trim();
            if (int.TryParse(line, out int dimension) == false)
            {
                MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nDimension of points was not declared", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return [];
            }
            if (dimension < minimumDimension)
            {
                MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nDimension of points must be at least {minimumDimension}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return [];
            }
            while ((line = reader.ReadLine()) is not null)
            {
                lineCounter++;
                string[] splits = line.Split(';');
                if (splits.Length != dimension)
                {
                    MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nNumber of coordinates of point didn't match the declared dimension", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return [];
                }
                double[] coordinates = new double[dimension];
                for (int i = 0; i < dimension; i++)
                {
                    if (double.TryParse(splits[i], out double coordinate) == false)
                    {
                        MessageBox.Show($"Error on line {lineCounter} in opened file:\n{fd.FileName}\nCoordinate could not be parsed: {splits[i]}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return [];
                    }
                    coordinates[i] = coordinate;
                }
                pointsFromFile.Add(coordinates);
            }
            return pointsFromFile;
        }
    }
}
