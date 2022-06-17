using Shahovska_IP91_kursova.Algorithms;
using Shahovska_IP91_kursova.Extensions;
using System;
using System.Threading.Tasks;

namespace Shahovska_IP91_kursova
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Count of unknows: ");
            var n = Convert.ToInt32(Console.ReadLine());

            Console.Write("Count of threads (parallel alg): ");
            var threadsCount = Convert.ToInt32(Console.ReadLine());

            var matrix = MatrixExtensions.CreateRandomMatrix(n);
            var (answer1, swSequential) = GaussianEliminationSequential.Run(matrix);
            var (answer2, swParallel) = await GaussianEliminationParallel.RunAsync(matrix, threadsCount);

            Console.WriteLine("Are answers equal: {0}", AreAnswersEqual(answer1, answer2));
            Console.WriteLine("Sequential (ms) : " + swSequential.ElapsedMilliseconds);
            Console.WriteLine("Parallel (ms) : " + swParallel.ElapsedMilliseconds);
        }

        private static bool AreAnswersEqual(float[] answer1, float[] answer2)
        {
            if (answer1.Length != answer2.Length)
            {
                return false;
            }

            for (int i = 0; i < answer1.Length; i++)
            {
                if (answer1[i] != answer2[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}
