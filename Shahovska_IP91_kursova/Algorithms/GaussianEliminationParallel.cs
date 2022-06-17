using Shahovska_IP91_kursova.Extensions;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Shahovska_IP91_kursova.Algorithms
{
    internal static class GaussianEliminationParallel
    {
        public static async Task<(float[] results, Stopwatch stopwatch)> RunAsync(float[][] matrix, int threadsCount)
        {
            var sw = Stopwatch.StartNew();

            var triangularMatrix = await TransformToTriangularMatrixAsync(matrix, threadsCount);
            var results = FindUnknowns(triangularMatrix);

            sw.Stop();

            return (results, sw);
        }

        private static async Task<float[][]> TransformToTriangularMatrixAsync(float[][] matrix, int threadsCount)
        {
            var triangularMatrix = matrix.CopyMatrix();
            var n = matrix.Length;
            var throttle = new SemaphoreSlim(initialCount: threadsCount);
            var rowsProcessed = 0;
            var taskCompletionSrc = new TaskCompletionSource();

            for (int k = 0; k < n; k++)
            {
                var columnIdx = k;
                await throttle.WaitAsync();

                Task.Run(() => ProcessRowAsync(columnIdx)).Forget();
            }

            await taskCompletionSrc.Task;
            return triangularMatrix;

            async Task ProcessRowAsync(int k)
            {
                for (int i = k + 1; i < n; i++)
                {
                    while (k > 0 && triangularMatrix[i][k - 1] != 0)
                    {
                        await Task.Delay(1);
                    }

                    for (int j = k + 1; j < triangularMatrix[i].Length; j++)
                    {
                        triangularMatrix[i][j] = triangularMatrix[i][j] - triangularMatrix[k][j] * (triangularMatrix[i][k] / triangularMatrix[k][k]);
                    }
                    triangularMatrix[i][k] = 0;
                }

                throttle.Release();
                if (Interlocked.Increment(ref rowsProcessed) == n)
                {
                    taskCompletionSrc.SetResult();
                }
            }
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
