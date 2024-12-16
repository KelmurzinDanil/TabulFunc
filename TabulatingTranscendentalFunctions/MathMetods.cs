namespace TabulatingTranscendentalFunctions
{
    public class MathMetods
    {
        public static int Factorial(int number)
        {
            if (number == 1 || number == 0)
                return 1;
            else
                return number * Factorial(number - 1);
        }
        public static double[] DividingSegment(double a, double b, int number)
        {
            if (a > b)
            {
                return new double[] { 0 };
            }
            double[] result = new double[number + 1]; 
            double step = (b - a) / number;
            for (int i = 0; i <= number; i++) 
            {
                result[i] = a + i * step; 
            }
            return result;
        }
    }
}
