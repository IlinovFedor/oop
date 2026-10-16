namespace oop;

class Programm
{
    private static double CalculateRealY(double x)
    {
        return (1 + 2 * Math.Pow(x, 2)) * Math.Exp(Math.Pow(x, 2));
    }

    private static int[] Factorials(int n)
    {
        int[] l = new int[n + 1];
        l[0] = 1;

        for (int i = 1; i <= n; i++)
            l[i] = l[i - 1] * i;

        return l;
    }

    const int k = 9;
    const int n = 10;
    const double a = 0.1;
    const double b = 1.0;
    const double eps = 0.0001;
    static int[] factorials = Factorials(n);
    
    private static double CalculateAn(int i, double x)
    {
        return (2.0 * i + 1) / factorials[i] * Math.Pow(x, 2 * i);
    }
    
    private static void Main()
    {
        for (double x = a; x <= b + eps; x += (b - a) / k)
        {
            double realY = CalculateRealY(x);

            double sn = 1;
            for (int i = 1; i <= n; i++)
                sn += CalculateAn(i, x);

            double se = 0;
            double an = 1;
            int j = 1;
            while (an > eps)
            {
                se += an;
                an = CalculateAn(j, x);
                j++;
            }

            Console.WriteLine($"X={x:F1}\t" +
                              $"SN={sn}\t" +
                              $"SE={se:F4}\t" +
                              $"Y={realY}\t" +
                              $"dN={realY - sn:F15}\t" +
                              $"dE={realY - se:F15}\t" +
                              $"j={j}");
        }
    }
}