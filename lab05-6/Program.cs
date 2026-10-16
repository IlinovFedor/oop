namespace oop;

enum Context
{
    Nil,
    Array,
    RuggedArray
}

class Error(Error.Type _type)
{
    public enum Type
    {
        Nil,
        OutOfRange,
        WrongSize
    }

    public Type GetType()
    {
        return _type;
    }

    public string String()
    {
        switch (_type)
        {
            case Type.Nil:
                return "Nil";
            case Type.OutOfRange:
                return "Index out of range";
            case Type.WrongSize:
                return "Wrong size";
            default:
                return "Unexpected error";
        }
    }
}

class Menu(params KeyValuePair<string, Action<Context>>[] actions)
{
    public void Show(Context ctx)
    {
        int command = 0;
        do
        {
            for (int i = 0; i < actions.GetLength(0); i++)
                Console.WriteLine($"{i + 1}. {actions[i].Key}");
            Console.WriteLine("0. Back");

            Inputer.Input(out command);
            if (1 <= command && command <= actions.GetLength(0))
                actions[command - 1].Value(ctx);
            else if (command != 0)
                Console.WriteLine("Unknown command");
        } while (command != 0);
    }
}

static class Inputer
{
    public static void ShowMainMenu(Context ctx)
    {
        var menu = new Menu(
            new KeyValuePair<string, Action<Context>>("Task 5.1 (5)", ShowMenuT5),
            new KeyValuePair<string, Action<Context>>("Task 5.2 (6)", (ctx) => Console.WriteLine("таск 6"))
        );
        menu.Show(Context.Nil);
    }

    private static void ShowMenuT5(Context ctx)
    {
        var menu = new Menu(
            new KeyValuePair<string, Action<Context>>("Two-dimensional array", ShowMenuT5S1),
            new KeyValuePair<string, Action<Context>>("Jagged array", (ctx) => Console.WriteLine("Рваный массив"))
        );
        menu.Show(ctx);
    }

    private static void ShowMenuT5S1(Context ctx)
    {
        var menu = new Menu(
            new KeyValuePair<string, Action<Context>>("Create array", ShowMenuCreateArray),
            new KeyValuePair<string, Action<Context>>("Print array", PrintArray),
            new KeyValuePair<string, Action<Context>>("Delete row with number K", DeleteK),
            new KeyValuePair<string, Action<Context>>("Add K rows starting from number N", AddKRows)
        );
        menu.Show(ctx);
    }

    private static void ShowMenuCreateArray(Context ctx)
    {
        var menu = new Menu(
            new KeyValuePair<string, Action<Context>>("Enter array from keyboard", ReadArray2D),
            new KeyValuePair<string, Action<Context>>("Generate random values", CreateArrayFromRnd)
        );
        menu.Show(ctx);
    }

    private static void ReadArray2D(Context ctx)
    {
        Input2DSize(out var rowsCnt, out var columnsCnt);

        var inputArr = new int[rowsCnt, columnsCnt];
        for (int i = 0; i < rowsCnt; i++)
        {
            var row = EnterIntRow(i, columnsCnt);
            for (int j = 0; j < columnsCnt; j++)
                inputArr[i, j] = row[j];
        }

        T5S1.SetArray(inputArr);
    }

    private static void CreateArrayFromRnd(Context ctx)
    {
        Input2DSize(out var rowsCnt, out var columnsCnt);

        var newArr = new int[rowsCnt, columnsCnt];

        Random rnd = new Random();
        for (int i = 0; i < newArr.GetLength(0); i++)
        for (int j = 0; j < newArr.GetLength(1); j++)
            newArr[i, j] = rnd.Next(1, 10);

        T5S1.SetArray(newArr);
    }

    private static void DeleteK(Context ctx)
    {
        Console.WriteLine("Enter K:");
        Input(out int k);
        var err = T5S1.DeleteK(k - 1);
        if (err.GetType() != Error.Type.Nil)
            Console.WriteLine(err.String());
    }

    private static void PrintArray(Context ctx)
    {
        var arr = T5S1.GetArray();
        var rowsCnt = arr.GetLength(0);
        var columnsCnt = arr.GetLength(1);
        if (rowsCnt == 0 || columnsCnt == 0)
        {
            Console.WriteLine("Array is empty");
            return;
        }

        for (int i = 0; i < rowsCnt; i++)
        {
            for (int j = 0; j < columnsCnt; j++)
                Console.Write($"{arr[i, j]} ");
            Console.Write("\n");
        }
    }

    private static void AddKRows(Context ctx)
    {
        Console.WriteLine("Enter K:");
        Input(out int k);
        Console.WriteLine("Enter N:");
        Input(out int n);

        var colCnt = T5S1.GetArray().GetLength(1);
        var arr = new int[n, colCnt];
        for (int i = 0; i < n; i++)
        {
            var row = EnterIntRow(i, colCnt);
            for (int j = 0; j < colCnt; j++)
                arr[i, j] = row[i];
        }

        var err = T5S1.AddKRows(k, n, arr);
        if (err.GetType() != Error.Type.Nil)
            Console.WriteLine(err.String());
    }

    public static void Input<T>(out T data) where T : IParsable<T>
    {
        var buf = Console.ReadLine();

        while (!T.TryParse(buf, null, out data))
        {
            Console.WriteLine("Format error. Try again :(");
            buf = Console.ReadLine();
        }
    }

    public static void Input2DSize(out int rowsCnt, out int columnsCnt)
    {
        rowsCnt = 0;
        columnsCnt = 0;
        while (rowsCnt < 1)
        {
            Console.WriteLine("Amount of rows (must be greater than 0): ");
            Input(out rowsCnt);
        }

        while (columnsCnt < 1)
        {
            Console.WriteLine("Amount of columns (must be greater than 0): ");
            Input(out columnsCnt);
        }
    }

    public static int[] EnterIntRow(int rowIdx, int columnsCnt)
    {
        var arr = new int[columnsCnt];
        bool ok = false;
        do
        {
            Console.WriteLine($"Enter {rowIdx + 1} line. Space is separator");
            var str = Console.ReadLine();
            var nums = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (nums.GetLength(0) != columnsCnt)
            {
                Console.WriteLine($"Wrong amount of columns. " +
                                  $"Expected {columnsCnt} " +
                                  $"Got {nums.GetLength(0)}." +
                                  $"Try again.");
                ok = false;
            }
            else
                ok = true;


            for (int j = 0; j < nums.GetLength(0); j++)
            {
                if (!int.TryParse(nums[j], null, out arr[j]))
                {
                    Console.WriteLine($"Cannot read element with number {j + 1}." +
                                      $"Try again.");
                    ok = false;
                    break;
                }

                ok = true;
            }
        } while (!ok);

        return arr;
    }
}

class Programm
{
    private static void Main()
    {
        Inputer.ShowMainMenu(Context.Nil);
    }
}

static class T5S1
{
    private static int[,] _arr = new int[,] { };

    public static void SetArray(int[,] arr)
    {
        _arr = arr;
    }

    public static int[,] GetArray()
    {
        return _arr;
    }

    public static Error DeleteK(int k)
    {
        if (_arr.GetLength(0) == 0 || _arr.GetLength(1) == 0)
            return new Error(Error.Type.OutOfRange);
        if (!(0 <= k && k < _arr.GetLength(0)))
            return new Error(Error.Type.OutOfRange);


        var newArr = new int[_arr.GetLength(0) - 1, _arr.GetLength(1)];
        var newRowIdx = 0;
        for (int i = 0; i < _arr.GetLength(0); i++)
        {
            if (i == k)
                continue;
            for (int j = 0; j < _arr.GetLength(1); j++)
                newArr[newRowIdx, j] = _arr[i, j];
            newRowIdx++;
        }

        _arr = newArr;
        return new Error(Error.Type.Nil);
    }

    public static Error AddKRows(int k, int n, int[,] rows)
    {
        if (!(0 <= k && k < _arr.GetLength(0)))
            return new Error(Error.Type.OutOfRange);
        if (rows.GetLength(1) != _arr.GetLength(1))
            return new Error(Error.Type.WrongSize);


        var newArr = new int[_arr.GetLength(0) + k, _arr.GetLength(1)];
        var curIdx = 0;
        for (int i = 0; i < newArr.GetLength(0); i++)
        {
            if (n - 1 <= i && i < n + k - 1)
                continue;

            for (int j = 0; j < newArr.GetLength(1); j++)
                newArr[i, j] = _arr[curIdx, j];
            curIdx++;
        }

        var newRowIdx = 0;
        for (int j = 0; j < k; j++)
        {
            for (int l = 0; l < newArr.GetLength(1); l++)
                newArr[n + j - 1, l] = rows[newRowIdx, l];
            newRowIdx++;
        }

        _arr = newArr;
        return new Error(Error.Type.Nil);
    }
}

/*
1
1
1
2
4
5
0
2
4
3
2
0 0 0 0 0
1 1 1 1 1
2 2 2 2 2
2

*/