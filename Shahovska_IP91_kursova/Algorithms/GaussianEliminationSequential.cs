using Shahovska_IP91_kursova.Extensions;
using System.Diagnostics;

namespace Shahovska_IP91_kursova.Algorithms
{
    internal static class GaussianEliminationSequential
    {
        public static (float[] results, Stopwatch stopwatch) Run(float[][] matrix)
        {
            var sw = Stopwatch.StartNew();

            var triangularMatrix = TransformToTriangularMatrix(matrix);
            var results = FindUnknowns(triangularMatrix);

            sw.Stop();

            return (results, sw);
        }

        private static float[][] TransformToTriangularMatrix(float[][] matrix)
        {
            var triangularMatrix = matrix.CopyMatrix();
            var n = matrix.Length;

            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = k + 1; j < n + 1; j++)
                    {
                        triangularMatrix[i][j] = triangularMatrix[i][j] - triangularMatrix[k][j] * (triangularMatrix[i][k] / triangularMatrix[k][k]);
                    }
                    triangularMatrix[i][k] = 0;
                }

            }

            return triangularMatrix;
        }

        /// <returns>array of results</returns>
        private static float[] FindUnknowns(float[][] triangularMatrix)
        {
            var n = triangularMatrix.Length;
            var results = new float[n];

            for (int k = n - 1; k >= 0; k--)
            {
                float sum = 0;
                for (int j = k + 1; j < n; j++)
                {
                    sum += triangularMatrix[k][j] * results[j];
                }
                results[k] = (triangularMatrix[k][n] - sum) / triangularMatrix[k][k];
            }

            return results;
        }
    }
}
