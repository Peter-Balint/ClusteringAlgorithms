
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Clustering.Model.DimensionalityReduction
{
    internal class PCA
    {
        internal class InputData
        {
            public float[] Features { get; set; }
        }

        internal class TransformedData
        {
            public float[] PCAFeatures { get; set; }
        }
        internal static void ReduceDimension(double[][] pointCoordinates, int toDim)
        {
            var mlContext = new MLContext();

            InputData[] data = new InputData[pointCoordinates.Count()];
            for(int i = 0; i < pointCoordinates.Count();i++)
            {
                float[] convertedCoordinates = new float[pointCoordinates[i].Length];
                for(int j = 0; j < convertedCoordinates.Length; j++)
                {
                    convertedCoordinates[j] = (float)pointCoordinates[i][j];
                }
                data[i] = new InputData { Features = convertedCoordinates };
            }

            int dimension = data.First().Features.Length;

            var schema = SchemaDefinition.Create(typeof(InputData));
            schema["Features"].ColumnType =
                new VectorDataViewType(NumberDataViewType.Single, dimension);

            var dataView = mlContext.Data.LoadFromEnumerable(data, schema);

            // Define PCA transformation
            var pipeline = mlContext.Transforms.ProjectToPrincipalComponents(
                outputColumnName: "PCAFeatures",
                inputColumnName: "Features",
                rank: toDim // target dimensionality
            );

            // Train the PCA model
            var model = pipeline.Fit(dataView);

            // Transform the data
            var transformedData = model.Transform(dataView);

            // Extract results
            var results = mlContext.Data.CreateEnumerable<TransformedData>(
                transformedData, reuseRowObject: false);

            for(int i=0;i<results.Count();i++)
            {
                pointCoordinates[i] = new double[toDim];
                for (int j = 0; j < toDim; j++)
                {
                    pointCoordinates[i][j] = results.ElementAt(i).PCAFeatures[j];
                }
            }
            Console.WriteLine(pointCoordinates);
        }
    }
}
