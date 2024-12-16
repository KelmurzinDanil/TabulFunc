using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TabulatingTranscendentalFunctions
{
    internal class FunctionTabulator
    {
        private double start;
        private double end;
        private double step;
        private double eps;
        private Func<int, double, double> func_q;
        Func<int, double, double> function;
        List<(double X, double Sx)> forX { get; set; } 
        List<(double X, double Sx)> forLagrange {  get; set; }
        List<(double X, double Sx)> forChb { get; set; }
        double MaxError { get; set; }
        double MaxErrorChebush { get; set; }
        public FunctionTabulator(double start, double end, double step, double eps, Func<int, double, double> func_q, Func<int, double, double> function)
        {
            this.start = start;
            this.end = end;
            this.step = step;
            this.eps = eps;
            this.func_q = func_q;
            this.function = function;

            
        }
        public List<(double X, double Sx)> CalculateSum(int num, double[] valuesX)
        {
            double[] valuesSx = new double[valuesX.Length];
            List<(double X, double Sx)> tableXandSx = new List<(double, double)>();
            for (int i = 0; i < valuesX.Length; i ++)
            {
                double a_n = function(0, valuesX[i]); //a0
                int n = 0;
                double Sx = 0;
                double previousSx = double.NaN;
                while ((double.IsNaN(previousSx) || Math.Abs(Sx - previousSx) > eps))
                {
                    previousSx = Sx;
                    Sx += a_n;
                    double q = func_q(n, valuesX[i]);
                    a_n = a_n * q;
                    n++;
                }
                valuesSx[i] = Sx;
                tableXandSx.Add((valuesX[i], Sx));
            }
            return tableXandSx;
        }
        public static double Lagrange(List<(double X, double Sx)> points, double x)
        {
            bool error = false;
            double result = 0;
            int n = points.Count;
            for (int i = 0; i < n; i++)
            {
                double Ln_x = points[i].Sx;
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        if (points[i].X == points[j].X)
                        {
                            error = true;
                            break;
                        }
                        Ln_x *= (x - points[j].X) / (points[i].X - points[j].X);
                    }
                }
                if (error)
                {
                    return double.NaN;
                }
                result += Ln_x;

            }
            return result;
        }
        public double[] GeneratedChebushevNodes(int n)
        {
            var nodes = new double[n];
            for(int i = 0;i < n; i++)
            {
                nodes[i] = (start + end)/2.0 + (end - start) /2.0 * Math.Cos(((2.0*i + 1) * Math.PI)/(2.0*n + 2));
            }
            return nodes;
        }
        public void PrintInterpolationTable(int n1, int n2)
        {
            forX = CalculateSum(n1, MathMetods.DividingSegment(start, end, n1));
            forLagrange = CalculateSum(n2, MathMetods.DividingSegment(start, end, n2));
            forChb = CalculateSum(n1, GeneratedChebushevNodes(n1));
            List<double> arrError = new List<double>();
            List<double> arrErrorChebush = new List<double>();
            Console.WriteLine($"X\t     True Value\t        Interpolated Value\t            Error\t         Error Chebush");
            for (int i = 0; i < forLagrange.Count; i++)
            {
                double interpolatedValue = Lagrange(forX, forLagrange[i].X);
                double interpolatedValueCh = Lagrange(forChb, forLagrange[i].X);
                double error = Math.Abs(forLagrange[i].Sx - interpolatedValue);
                double errorCB = Math.Abs(forLagrange[i].Sx - interpolatedValueCh);
                arrError.Add(error);
                arrErrorChebush.Add(errorCB);
                Console.WriteLine($"{forLagrange[i].X}\t{forLagrange[i].Sx}\t\t{interpolatedValue}\t\t{Math.Round(error, 12)}\t\t{errorCB}");
            }
            MaxError = arrError.Max();
            MaxErrorChebush = arrErrorChebush.Max();
            //WriteColoredString($"Максимальная ошибка при равнораспределенных узлах:\t    {MaxError}\t при n = {n1}", ConsoleColor.Red);
            //WriteColoredString($"Максимальная ошибка при Чебушевских узлах:\t            {MaxErrorChebush}\t при n = {n1}", ConsoleColor.Green);
        }
        public void Experiment(int n1, int n2)
        {
            if (n1 == 100) return;
            if (n1%10 != 0)
            {
                forX = CalculateSum(n1, MathMetods.DividingSegment(start, end, n1));
                forLagrange = CalculateSum(n2, MathMetods.DividingSegment(start, end, n2));
                forChb = CalculateSum(n1, GeneratedChebushevNodes(n1));
                List<double> arrError = new List<double>();
                List<double> arrErrorChebush = new List<double>();
                for (int i = 0; i < forLagrange.Count; i++)
                {
                    double interpolatedValue = Lagrange(forX, forLagrange[i].X);
                    double interpolatedValueCh = Lagrange(forChb, forLagrange[i].X);
                    double error = Math.Abs(forLagrange[i].Sx - interpolatedValue);
                    double errorCB = Math.Abs(forLagrange[i].Sx - interpolatedValueCh);
                    arrError.Add(error);
                    arrErrorChebush.Add(errorCB);
                }
                MaxError = arrError.Max();
                MaxErrorChebush = arrErrorChebush.Max();
                arrError.Clear();
                arrErrorChebush.Clear();
                WriteColoredString($"Максимальная ошибка при равнораспределенных узлах:\t    {MaxError}\t при n = {n1}", ConsoleColor.Red);
                WriteColoredString($"Максимальная ошибка при Чебышевских узлах:\t            {MaxErrorChebush}\t при n = {n1}", ConsoleColor.Green);
                n1++;
                Experiment(n1, n2);
            }
            else
            {
                n1++;
                Experiment(n1, n2);
            }
           
        }
        public static void WriteColoredString(string text, ConsoleColor color)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColor;
        }
    }
}
