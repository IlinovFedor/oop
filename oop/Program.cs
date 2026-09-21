namespace oop;

class Programm
{
    // TODO: вынести в другой проект для последующих импортов
    private static void Input<T>(string message, out T data) where T : IParsable<T>
    {
        Console.WriteLine(message);
        var buf = Console.ReadLine();

        while (!T.TryParse(buf, null, out data))
        {
            Console.WriteLine("Format error. Try again >:(");
            buf = Console.ReadLine();
        }
    }

    static void PrintFloatBits(float value)
    {
        var bits = BitConverter.SingleToInt32Bits(value);

        var binary = Convert.ToString(bits, 2).PadLeft(32, '0');
        for (int i = 1; i < 64; i += 2)
        {
            binary = binary.Insert(i, ",");
        }
        
        Console.WriteLine(binary);
    }

    static void PrintDoubleBits(double value)
    {
        var bits = BitConverter.DoubleToInt64Bits(value);

        var binary = Convert.ToString(bits, 2).PadLeft(64, '0');

        for (int i = 1; i < 128; i += 2)
        {
            binary = binary.Insert(i, ",");
        }
        Console.WriteLine(binary);
    }

    private static void Task1()
    {
        Input("Enter n: ", out int n);
        Input("Enter m: ", out int m);
        Input("Enter x: ", out int x);

        var res1 = n++ + m--;
        var res2 = n * m < n++;
        var res3 = n-- > ++m;
        var res4 = Math.Pow(2, x) * x * Math.Cos(x) + 1;

        Console.WriteLine($"n\t\t{n}");
        Console.WriteLine($"m\t\t{m}");
        Console.WriteLine($"n++ + m--\t\t{res1}");
        Console.WriteLine($"n * m < n++\t\t{res2}");
        Console.WriteLine($"n-- > ++m\t\t{res3}");
        Console.WriteLine($"2^x * x * cos(x) + 1\t{res4}");
    }

    private static void Task2()
    {
        double x1, y1;
        Input("Enter x1: ", out x1);
        Input("Enter y1: ", out y1);

        var isInside = (Math.Pow(x1, 2) + Math.Pow(y1, 2) <= 1) && (x1 >= 0 || y1 >= 0);
        Console.WriteLine($"Is inside: {isInside}");
    }

    private static void Task3()
    {
        const float floatA = 1000f, floatB = 0.0001f;
        var floatAns = (MathF.Pow((floatA + floatB), 4) - (MathF.Pow(floatA, 4) + 4 * MathF.Pow(floatA, 3) * floatB)) /
                       (6 * MathF.Pow(floatA, 2) * MathF.Pow(floatB, 2) + 4 * floatA * MathF.Pow(floatB, 3) +
                        MathF.Pow(floatB, 4));
        const double doubleA = 1000, doubleB = 0.0001;
        var doubleAns =
            (Math.Pow((doubleA + doubleB), 4) - (Math.Pow(doubleA, 4) + 4 * Math.Pow(doubleA, 3) * doubleB)) /
            (6 * Math.Pow(doubleA, 2) * Math.Pow(doubleB, 2) + 4 * doubleA * Math.Pow(doubleB, 3) +
             Math.Pow(doubleB, 4));

        Console.Write(
            $"a = 1000, b = 0.0001\n" +
            $"((a + b) ^ 4 - (a ^ 4 + 4 * a ^ 3 * b)) / (6 * a ^ 2 * b ^ 2 + 4 * a * b ^ 3 + b ^ 4) =\n" +
            $"Float:\t{floatAns}\n" +
            $"Double:\t{doubleAns}\n");
    }

    private static void Main()
    {
        while (true)
        {
            Input("Choose task: 1 - 3, 0 - exit", out int operationId);
            switch (operationId)
            {
                case 0:
                    return;
                case 1:
                    Task1();
                    break;
                case 2:
                    Task2();
                    break;
                case 3:
                    Task3();
                    break;
                default:
                    Console.WriteLine("Unknown operation");
                    break;
            }
        }
    }
}