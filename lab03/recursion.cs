namespace oop;

class Programm
{
    private static double CalculateRealY(double x)
    {
        return (1 + 2 * Math.Pow(x, 2)) * Math.Exp(Math.Pow(x, 2));
    }

    const int k = 9;
    const int n = 10;
    const double a = 0.1;
    const double b = 1.0;
    const double eps = 0.0001;
    
    private static double CalculateNextAn(double anPrev, double x, int i)
    {
        return anPrev * Math.Pow(x, 2) * (2 * i + 3) / (2 * i + 1) / (i + 1);
    }
    
    private static void Main()
    {
        for (double x = a; x <= b + eps; x += (b - a) / k)
        {
            double realY = CalculateRealY(x);

            double sn = 1;
            double an = 1;
            for (int i = 0; i < n; i++)
            {
                an = CalculateNextAn(an, x, i);
                sn += an;
            }

            double se = 0;
            an = 1;
            int j = 0;
            while (an > eps)
            {
                se += an;
                an = CalculateNextAn(an, x, j);
                j++;
            }

            Console.WriteLine($"X={x:F1}\t" +
                              $"SN={sn}\t" +
                              $"SE={se:F4}\t" +
                              $"Y={realY}\t" +
                              $"dN={realY - sn:F15}\t" +
                              $"dE={realY - se:F15}\t");
        }
    }
}