using System;
using System.Linq;

namespace Shahovska_IP91_kursova.Extensions
{
    public static class MatrixExtensions
    {
        public static float[][] CreateRandomMatrix(int n)
        {
            var rand = new Random();
            var randRowFunc = new Func<int, float[]>(_ => Enumerable
                .Range(1, n + 1)
                .Select(__ => (float)rand.NextDouble() * 10)
                .ToArray());

            return Enumerable
                .Range(1, n)
                .Select(randRowFunc)
                .ToArray();
        }

        public static float[][] CopyMatrix(this float[][] matrix) => matrix.Select(x => x.ToArray()).ToArray();
    }
}
