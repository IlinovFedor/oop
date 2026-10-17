namespace oop;

class Programm
{
    private static void Main()
    {
        var _task5 = new _task5();
        var _task6 = new _task6();
        var inputer = new Inputer(_task5, _task6);
        
        inputer.ShowMenuMain();
    }
}

class Error(Error.Type value)
{
    public enum Type
    {
        Ok,
        OutOfRange,
        WrongSize,
        Empty
    }

    public Type Value = value;

    public string String()
    {
        switch (Value)
        {
            case Type.Ok:
                return "Nil";
            case Type.OutOfRange:
                return "Index out of range";
            case Type.WrongSize:
                return "Wrong size";
            case Type.Empty:
                return "Array is empty";
            default:
                return "Unexpected error";
        }
    }
}

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
                try
                {
                    actions[command - 1].Value();
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nUnexpected error");
                    Console.WriteLine(e);
                }
            else if (command != 0)
                Console.WriteLine("Unknown command");
        } while (command != 0);
    }
}

class Inputer(_task5 task5, _task6 task6)
{
    private _task5 _task5 = task5;
    private _task6 _task6 = task6;

    public void ShowMenuMain()
    {
        new Menu(
            new KeyValuePair<string, Action>("Task 5.1 (5)", ActionMenuT5),
            new KeyValuePair<string, Action>("Task 5.2 (6)", () => Console.WriteLine("таск 6"))
        ).Show();
    }

    private void ActionMenuT5()
    {
        new Menu(
            new KeyValuePair<string, Action>("Two-dimensional array", ActionMenuArray),
            new KeyValuePair<string, Action>("Jagged array", ActionMenuRagArray)
        ).Show();
    }

    private void ActionMenuArray()
    {
        new Menu(
            new KeyValuePair<string, Action>("Create array", ActionMenuCreateArray),
            new KeyValuePair<string, Action>("Print array", ActionPrintArray),
            new KeyValuePair<string, Action>("Delete row with number K", ActionDeleteK)
        ).Show();
    }

    private void ActionMenuCreateArray()
    {
        new Menu(
            new KeyValuePair<string, Action>("Generate random values", ActionCreateArrayFromRnd),
            new KeyValuePair<string, Action>("Enter array from keyboard", ActionReadArray)
        ).Show();
    }

    private void ActionReadArray()
    {
        Input2DSize(out var rowsCnt, out var colCnt);
        var arr = new int[rowsCnt, colCnt];
        for (int i = 0; i < rowsCnt; i++)
        {
            var row = EnterRowOfSize(i, colCnt);
            for (int j = 0; j < colCnt; j++)
                arr[i, j] = row[j];
        }

        _task5.Array = arr;
        ActionPrintArray();
    }

    private void ActionCreateArrayFromRnd()
    {
        Random rnd = new Random();
        Input2DSize(out var rowsCnt, out var colCnt);
        var arr = new int[rowsCnt, colCnt];
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            var len = arr.GetLength(1);
            for (int j = 0; j < len; j++)
                arr[i, j] = rnd.Next(1, 10);
        }

        _task5.Array = arr;
        ActionPrintArray();
    }

    private void ActionDeleteK()
    {
        Console.WriteLine("Enter K:");
        Input(out int k);
        var err = _task5.DeleteK(k - 1);
        if (err.Value != Error.Type.Ok)
        {
            Console.WriteLine(err.String());
            return;
        }

        ActionPrintArray();
    }

    private void ActionMenuRagArray()
    {
        new Menu(
            new KeyValuePair<string, Action>("Create array", ShowMenuCreateRagArray),
            new KeyValuePair<string, Action>("Print array", ActionPrintRagArray),
            new KeyValuePair<string, Action>("Add K rows starting from number N", ActionAddKRows)
        ).Show();
    }

    private void ShowMenuCreateRagArray()
    {
        new Menu(
            new KeyValuePair<string, Action>("Generate random values", ActionCreateRagArrayFromRnd),
            new KeyValuePair<string, Action>("Enter array from keyboard", ActionReadRagArray)
        ).Show();
    }

    private void ActionReadRagArray()
    {
        int rowsCnt = 0;
        while (rowsCnt < 1)
        {
            Console.WriteLine("Enter amount of rows");
            Input(out rowsCnt);
        }

        var arr = new int[rowsCnt][];

        for (int i = 0; i < rowsCnt; i++)
        {
            var row = EnterIntRow(i);
            arr[i] = row;
        }

        _task5.JaggedArray = arr;
        ActionPrintRagArray();
    }

    private void ActionCreateRagArrayFromRnd()
    {
        Random rnd = new Random();
        var rowsCnt = rnd.Next(2, 10);
        var arr = new int[rowsCnt][];

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            var len = rnd.Next(2, 10);
            arr[i] = new int[len];
            for (int j = 0; j < len; j++)
                arr[i][j] = rnd.Next(1, 10);
        }

        _task5.JaggedArray = arr;
        ActionPrintRagArray();
    }


    private void ActionAddKRows()
    {
        int cntRow = _task5.JaggedArray.Length;

        int n = 0;
        while (!(0 < n && n < cntRow + 2))
        {
            Console.WriteLine($"Enter N. It must be greater than 0 and less than {cntRow + 2}: ");
            Input(out n);
        }

        int k = 0;
        while (k < 1)
        {
            Console.WriteLine("Enter K. It musts be greater than 0");
            Input(out k);
        }

        var jagArr = new int[k][];

        for (int i = 0; i < k; i++)
        {
            var row = EnterIntRow(i);
            jagArr[i] = row;
        }

        var err = _task5.AddKRows(n - 1, jagArr);
        if (err.Value != Error.Type.Ok)
        {
            Console.WriteLine(err.String());
            return;
        }

        ActionPrintRagArray();
    }

    private int[] EnterRowOfSize(int rowIdx, int colCnt)
    {
        while (true)
        {
            var row = EnterIntRow(rowIdx);
            if (row.Length == colCnt)
                return row;

            Console.WriteLine($"Wrong amount of columns. Expected {colCnt}, got {row.Length}. Try again.");
        }
    }

    private void ActionPrintArray()
    {
        Console.WriteLine("=======  ARRAY =======");

        var arr = _task5.Array;
        int rowsCnt = 0;
        rowsCnt = arr.GetLength(0);
        if (rowsCnt == 0)
        {
            Console.WriteLine("Array is empty");
            return;
        }

        for (int i = 0; i < rowsCnt; i++)
        {
            Console.Write($"{i + 1}: ");
            for (int j = 0; j < arr.GetLength(1); j++)
                Console.Write($"{arr[i, j]} ");
            Console.Write("\n");
        }

        Console.WriteLine("=======  ARRAY =======");
    }

    private void ActionPrintRagArray()
    {
        Console.WriteLine("==== RAGGED ARRAY ====");

        var arr = _task5.JaggedArray;
        int rowsCnt = arr.Length;
        if (rowsCnt == 0)
        {
            Console.WriteLine("Array is empty");
            return;
        }

        for (int i = 0; i < rowsCnt; i++)
        {
            Console.Write($"{i + 1}: ");
            for (int j = 0; j < arr[i].Length; j++)
                Console.Write($"{arr[i][j]} ");
            Console.Write("\n");
        }

        Console.WriteLine("==== RAGGED ARRAY ====");
    }

    public static void Input<T>(out T data) where T : IParsable<T>
    {
        var buf = Console.ReadLine();

        while (!T.TryParse(buf, null, out data))
        {
            Console.WriteLine("Cannot format result. Try again :(");
            buf = Console.ReadLine();
        }
    }

    private void Input2DSize(out int rowsCnt, out int colCnt)
    {
        rowsCnt = 0;
        colCnt = 0;
        while (rowsCnt < 1)
        {
            Console.WriteLine("Amount of rows. It musts be greater than 0:");
            Input(out rowsCnt);
        }

        while (colCnt < 1)
        {
            Console.WriteLine("Amount of columns. It musts be greater than 0: ");
            Input(out colCnt);
        }
    }

    private int[] EnterIntRow(int rowIdx)
    {
        while (true)
        {
            Console.WriteLine($"Enter {rowIdx + 1} line. Space is separator");
            var str = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(str)) continue;

            var nums = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var arr = new int[nums.Length];
            bool ok = true;

            for (int j = 0; j < nums.Length; j++)
            {
                if (!int.TryParse(nums[j], out arr[j]))
                {
                    Console.WriteLine($"Cannot read element with number {j + 1}. Try again.");
                    ok = false;
                    break;
                }
            }

            if (ok) return arr;
        }
    }
}

class _task5
{
    public int[,] Array = new int[,] { };
    public int[][] JaggedArray = [];

    public Error DeleteK(int k)
    {
        int rowCnt = Array.GetLength(0);

        if (rowCnt == 0)
            return new Error(Error.Type.Empty);
        if (!(0 <= k && k < rowCnt))
            return new Error(Error.Type.OutOfRange);


        var arr = new int[rowCnt - 1, Array.GetLength(1)];
        var newRowIdx = 0;
        for (int i = 0; i < rowCnt; i++)
        {
            if (i == k)
                continue;

            for (int j = 0; j < Array.GetLength(1); j++)
                arr[newRowIdx, j] = Array[i, j];
            newRowIdx++;
        }

        Array = arr;
        return new Error(Error.Type.Ok);
    }

    public Error AddKRows(int index, int[][] jagRows)
    {
        var rowsCnt = jagRows.Length;
        if (!(0 <= index && index <= JaggedArray.Length))
            return new Error(Error.Type.OutOfRange);

        int newRowCnt = JaggedArray.Length + rowsCnt;

        var jagArr = new int[newRowCnt][];
        int curIdx = 0;

        for (int i = 0; i < newRowCnt; i++)
        {
            if (index <= i && i < index + rowsCnt)
                jagArr[i] = jagRows[i - index];
            else
                jagArr[i] = JaggedArray[curIdx++];
        }

        JaggedArray = jagArr;
        return new Error(Error.Type.Ok);
    }
}

class _task6
{
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