using System;

class Programm
{
    // TODO: вынести в другой проект для последующих импортов
    static void input<T>(string message, out T data) where T : IParsable<T>
    {
        Console.WriteLine(message);
        var buf = Console.ReadLine();

        while (!T.TryParse(buf, null, out data))
        {
            Console.WriteLine("Format error. Try again >:(");
            buf = Console.ReadLine();
        }
    }

    static void task1()
    {
        int n, m, x;

        input("Enter n: ", out n);
        input("Enter m: ", out m);
        input("Enter x: ", out x);

        var res1 = n++ + m--;
        var res2 = n * m < n++;
        var res3 = n-- > ++m;
        var res4 = Math.Pow(2, x) * Math.Cos(x) + 1;

        Console.Write($"n++ + m-- =\t\t{res1}\n");
        Console.Write($"n * m < n++ =\t\t{res2}\n");
        Console.Write($"n-- > ++m =\t\t{res3}\n");
        Console.Write($"2^x * cox(x) + 1 =\t{res4}\n");

    }

    static void task2()
    {
        
        double x1, y1;
        input("Enter x1: ", out x1);
        input("Enter y1: ", out y1);

        var isInside = (Math.Pow(x1, 2) + Math.Pow(y1, 2) <= 1) ;
        Console.WriteLine($"Is inside: {isInside}");
    }

    static void task3()
    {
        
    }
    
    static void Main()
    {
        task2();
    }
}
