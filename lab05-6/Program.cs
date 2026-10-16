namespace oop;


class Menu(params KeyValuePair<string, Action>[] actions)
{
    public void Show()
    {
        int command = 0;
        do
        {
            for (int i = 0; i < actions.GetLength(0); i++)
                Console.WriteLine($"{i + 1}. {actions[i].Key}");
            Console.WriteLine("0. Back");

            Inputer.Input(out command);
            if (1 <= command && command <= actions.GetLength(0))
                actions[command - 1].Value();
            else if (command != 0)
                Console.WriteLine("Unknown command");
        } while (command != 0);
    }
}

static class Inputer
{
    public static void Input<T>(out T data) where T : IParsable<T>
    {
        var buf = Console.ReadLine();

        while (!T.TryParse(buf, null, out data))
        {
            Console.WriteLine("Format error. Try again :(");
            buf = Console.ReadLine();
        }
    }

    public static int InputIntWithRange(string name, int min, int max)
    {
        int n = 0;
        while (! (min <= n && n < max))
        {
            Console.WriteLine($"{name} ({min} <= K < {max}): ");
            Input(out n);
        }

        return n;
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
        var menu = new Menu(
            new KeyValuePair<string, Action>("Task 5.1 (5)", Task5.MenuTask5),
            new KeyValuePair<string, Action>("Task 5.2 (6)", () => Console.WriteLine("таск 6"))
        );
        menu.Show();
    }
}

static class Task5
{
    private static int[,] _arr = new int[,] { };

    public static void MenuTask5()
    {
        var menu = new Menu(
            new KeyValuePair<string, Action>("Two-dimensional array", MenuTask5Subtask1),
            new KeyValuePair<string, Action>("Jagged array", () => Console.WriteLine("Рваный массив"))
        );
        menu.Show();
    }

    private static void MenuTask5Subtask1()
    {
        var menu = new Menu(
            new KeyValuePair<string, Action>("Create array", MenCreateArray),
            new KeyValuePair<string, Action>("Print array", Print),
            new KeyValuePair<string, Action>("Delete row with number K", DeleteK),
            new KeyValuePair<string, Action>("Add K rows starting from number N", AddKRows)
        );
        menu.Show();
    }

    private static void MenCreateArray()
    {
        var menu = new Menu(
            new KeyValuePair<string, Action>("Enter array from keyboard", InputArray2DFromConsole),
            new KeyValuePair<string, Action>("Generate random values", CreateArrayFromRnd)
        );
        menu.Show();
    }

    private static void InputArray2DFromConsole()
    {
        Inputer.Input2DSize(out var rowsCnt, out var columnsCnt);

        var inputArr = new int[rowsCnt, columnsCnt];
        for (int i = 0; i < rowsCnt; i++)
        {
            var row = Inputer.EnterIntRow(i, columnsCnt);
            for (int j = 0; j < columnsCnt; j++)
                inputArr[i, j] = row[j];

        }

        _arr = inputArr;
    }

    private static void CreateArrayFromRnd()
    {
        Inputer.Input2DSize(out var rowsCnt, out var columnsCnt);

        _arr = new int[rowsCnt, columnsCnt];

        Random rnd = new Random();
        for (int i = 0; i < _arr.GetLength(0); i++)
        for (int j = 0; j < _arr.GetLength(1); j++)
            _arr[i, j] = rnd.Next(1, 10);
    }

    private static void Print()
    {
        var rowsCnt = _arr.GetLength(0);
        var columnsCnt = _arr.GetLength(1);
        if (rowsCnt == 0 || columnsCnt == 0)
        {
            Console.WriteLine("Array is empty");
        }

        for (int i = 0; i < rowsCnt; i++)
        {
            for (int j = 0; j < columnsCnt; j++)
                Console.Write($"{_arr[i, j]} ");
            Console.Write("\n");
        }
    }

    private static void DeleteK()
    {
        if (_arr.GetLength(0) == 0 || _arr.GetLength(1) == 0)
        {
            Console.WriteLine("Array is empty");
            return;
        }

        var k = Inputer.InputIntWithRange("K", 1, _arr.GetLength(0) + 1);

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
    }

    private static void AddKRows()
    {
        var k = Inputer.InputIntWithRange("K", 1, _arr.GetLength(0) + 1);
        var n = Inputer.InputIntWithRange("N", 1, _arr.GetLength(0) + 1);
        
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
        
        for (int j = 0; j < k; j++)
        {
            var newRow = Inputer.EnterIntRow(j, _arr.GetLength(1));
            for (int l = 0; l < newArr.GetLength(1); l++)
                newArr[n + j - 1, l] = newRow[l];
        }

        _arr = newArr;
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