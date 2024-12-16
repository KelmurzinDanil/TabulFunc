using System;

namespace TabulatingTranscendentalFunctions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tabulator = new FunctionTabulator(0, 1.5, 0.15, Math.Pow(10, -6),
                (n, x) => (-1 * Math.PI * Math.PI * (Math.Pow(4 * n + 1, Math.Pow(x, 4 * n + 1)))) / (8 * (n + 1) * (2 * n + 1) * Math.Pow(4 * n + 5, Math.Pow(x, 4 * n + 5))),
                (n, x) => (Math.Pow(-1, n) * Math.Pow(Math.PI / 2, 2 * n)) / (MathMetods.Factorial(2 * n) * Math.Pow(4 * n + 1, Math.Pow(x, 4 * n + 1))));
            tabulator.PrintInterpolationTable(5, 10);

            tabulator.Experiment(5, 10);
            Console.ReadKey();
        }
    }
}
