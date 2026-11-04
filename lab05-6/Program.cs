using System.Text.RegularExpressions;

namespace oop;

public class Program
{
    private static void Main()
    {
        var task5 = new Task5();
        var task6 = new Task6();
        var inputer = new Inputer(task5, task6);

        inputer.ShowMenuMain();
    }
}

public class Error(Error.Type value)
{
    public enum Type
    {
        Ok,
        OutOfRange,
        WrongSize,
        EmptyArray,
        EmptyText
    }

    public Type Value = value;

    public string GetMessage()
    {
        switch (Value)
        {
            case Type.Ok:
                return "No error";
            case Type.OutOfRange:
                return "Index out of range";
            case Type.WrongSize:
                return "Wrong size";
            case Type.EmptyArray:
                return "Array is empty";
            case Type.EmptyText:
                return "String is empty";
            default:
                return "Unexpected error";
        }
    }
}

public class Menu(params KeyValuePair<string, Action>[] actions)
{
    public void Show()
    {
        int command;
        do
        {
            for (int i = 0; i < actions.Length; i++)
                Console.WriteLine($"{i + 1}. {actions[i].Key}");
            Console.WriteLine("0. Back");

            Inputer.Input(out command);
            if (1 <= command && command <= actions.Length)
            {
                try
                {
                    actions[command - 1].Value();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\nUnexpected error: {e.Message}");
                }
            }
            else if (command != 0)
            {
                Console.WriteLine("Unknown command");
            }
        } while (command != 0);
    }
}

public class Inputer(Task5 task5, Task6 task6)
{
    private static readonly string[] TestStrings =
    [
        "В лесу родилась елочка! В лесу она росла. Зимой и летом стройная, зеленая была!",
        "Цена 3.5 рубля. Мы   купили хлеб;  молоко: сыр. Вы пойдете гулять? Да!",
        "Первое предложение. Второе предложение!",
        "Одно предложение без точки",
        ""
    ];

    private string _text6 = string.Empty;

    public void ShowMenuMain()
    {
        new Menu(
            new KeyValuePair<string, Action>("Task 5.1 (5)", ActionMenuT5),
            new KeyValuePair<string, Action>("Task 5.2 (6)", ShowMenuTask6)
        ).Show();
    }

    public static void Input<T>(out T data) where T : IParsable<T>
    {
        while (true)
        {
            var buf = Console.ReadLine();
            if (T.TryParse(buf, null, out var parsed))
            {
                data = parsed;
                return;
            }

            Console.WriteLine("Invalid input. Try again :(");
        }
    }

    private static int ReadInt(string prompt, int min = 1, int max = int.MaxValue)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            Input(out int value);
            if (min <= value && value <= max)
                return value;

            Console.WriteLine("Value is out of range. Try again.");
        }
    }

    private static void FillRandom(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = Random.Shared.Next(1, 10);
        }
    }

    private static void FillRandom(int[] row)
    {
        for (int i = 0; i < row.Length; i++)
            row[i] = Random.Shared.Next(1, 10);
    }

    private static void PrintArray(int[,] matrix)
    {
        Console.WriteLine("=======  ARRAY =======");

        int rowCount = matrix.GetLength(0);
        if (rowCount == 0)
        {
            Console.WriteLine("Array is empty");
        }

        for (int i = 0; i < rowCount; i++)
        {
            Console.Write($"{i + 1}: ");
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write($"{matrix[i, j]} ");
            Console.WriteLine();
        }

        Console.WriteLine("=======  ARRAY =======");
    }

    private static void PrintArray(int[][] rows)
    {
        Console.WriteLine("==== RAGGED ARRAY ====");

        if (rows.Length == 0)
        {
            Console.WriteLine("Array is empty");
        }

        for (int i = 0; i < rows.Length; i++)
        {
            Console.Write($"{i + 1}: ");
            for (int j = 0; j < rows[i].Length; j++)
                Console.Write($"{rows[i][j]} ");
            Console.WriteLine();
        }

        Console.WriteLine("==== RAGGED ARRAY ====");
    }

    private void ShowMenuTask6()
    {
        new Menu(
            new KeyValuePair<string, Action>("Enter text from keyboard", ActionReadText),
            new KeyValuePair<string, Action>("Choose test string", ActionChooseTestString),
            new KeyValuePair<string, Action>("Remove first and last sentence", ActionExecuteTask)
        ).Show();
    }

    private void ActionReadText()
    {
        Console.WriteLine("Enter text:");
        _text6 = Console.ReadLine();
    }

    private void ActionChooseTestString()
    {
        for (int i = 0; i < TestStrings.Length; i++)
            Console.WriteLine($"{i + 1}. \"{TestStrings[i]}\"");

        var number = ReadInt($"Choose test string (1-{TestStrings.Length}):", 1, TestStrings.Length);
        _text6 = TestStrings[number - 1];
    }

    private void ActionExecuteTask()
    {
        var result = _text6;
        var err = task6.RemoveFirstAndLastSentence(ref result);
        if (err.Value != Error.Type.Ok)
        {
            Console.WriteLine(err.GetMessage());
            return;
        }

        Console.WriteLine($"Source: \"{_text6}\"");
        _text6 = result;
        if (result.Length == 0)
        {
            Console.WriteLine("Result: string is empty (there were less than 3 sentences)");
            return;
        }

        Console.WriteLine($"Result: \"{result}\"");
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
            new KeyValuePair<string, Action>("Print array", () => PrintArray(task5.Matrix)),
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
        var matrix = new int[rowsCnt, colCnt];
        for (int i = 0; i < rowsCnt; i++)
        {
            var row = EnterRowOfSize(i, colCnt);
            for (int j = 0; j < colCnt; j++)
                matrix[i, j] = row[j];
        }

        task5.Matrix = matrix;
        PrintArray(task5.Matrix);
    }

    private void ActionCreateArrayFromRnd()
    {
        Input2DSize(out var rowsCnt, out var colCnt);
        var matrix = new int[rowsCnt, colCnt];
        FillRandom(matrix);

        task5.Matrix = matrix;
        PrintArray(task5.Matrix);
    }

    private void ActionDeleteK()
    {
        if (task5.Matrix.GetLength(0) == 0)
        {
            Console.WriteLine(new Error(Error.Type.EmptyArray).GetMessage());
            return;
        }

        Console.WriteLine("Enter K:");
        Input(out int k);
        var err = task5.DeleteK(k);
        if (err.Value != Error.Type.Ok)
        {
            Console.WriteLine(err.GetMessage());
            return;
        }

        PrintArray(task5.Matrix);
    }

    private void ActionMenuRagArray()
    {
        new Menu(
            new KeyValuePair<string, Action>("Create array", ShowMenuCreateRagArray),
            new KeyValuePair<string, Action>("Print array", () => PrintArray(task5.JaggedArray)),
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
        var rowsCnt = ReadInt("Enter amount of rows (at least 1):");
        var rows = new int[rowsCnt][];
        for (int i = 0; i < rowsCnt; i++)
            rows[i] = EnterIntRow(i);

        task5.JaggedArray = rows;
        PrintArray(task5.JaggedArray);
    }

    private void ActionCreateRagArrayFromRnd()
    {
        var rowsCnt = Random.Shared.Next(2, 10);
        var rows = new int[rowsCnt][];
        for (int i = 0; i < rowsCnt; i++)
        {
            rows[i] = new int[Random.Shared.Next(2, 10)];
            FillRandom(rows[i]);
        }

        task5.JaggedArray = rows;
        PrintArray(task5.JaggedArray);
    }

    private void ActionAddKRows()
    {
        int rowCount = task5.JaggedArray.Length;
        var n = ReadInt($"Enter N (1-{rowCount + 1}):", 1, rowCount + 1);
        var k = ReadInt("Enter K (at least 1):");

        var newRows = new int[k][];
        for (int i = 0; i < k; i++)
            newRows[i] = EnterIntRow(i);

        var err = task5.AddKRows(n - 1, newRows);
        if (err.Value != Error.Type.Ok)
        {
            Console.WriteLine(err.GetMessage());
            return;
        }

        PrintArray(task5.JaggedArray);
    }

    private void Input2DSize(out int rowsCnt, out int colCnt)
    {
        rowsCnt = ReadInt("Amount of rows (at least 1):");
        colCnt = ReadInt("Amount of columns (at least 1):");
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

    private int[] EnterIntRow(int rowIdx)
    {
        while (true)
        {
            Console.WriteLine($"Enter {rowIdx + 1} line. Space is separator");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Line is empty. Try again.");
                continue;
            }

            var nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var row = new int[nums.Length];
            var isParsed = true;

            for (int j = 0; j < nums.Length; j++)
            {
                if (!int.TryParse(nums[j], out row[j]))
                {
                    Console.WriteLine($"Cannot read element with number {j + 1}. Try again.");
                    isParsed = false;
                    break;
                }
            }

            if (isParsed)
                return row;
        }
    }
}

public class Task5
{
    public int[,] Matrix = new int[0, 0];

    public int[][] JaggedArray = [];

    public Error DeleteK(int k)
    {
        int idx = k - 1;
        int rowCnt = Matrix.GetLength(0);
        int colCnt = Matrix.GetLength(1);

        if (rowCnt == 0)
            return new Error(Error.Type.EmptyArray);
        if (!(0 <= idx && idx < rowCnt))
            return new Error(Error.Type.OutOfRange);

        var result = new int[rowCnt - 1, colCnt];
        Array.Copy(Matrix, 0, result, 0, idx * colCnt);
        Array.Copy(Matrix, (idx + 1) * colCnt, result, idx * colCnt, (rowCnt - idx - 1) * colCnt);

        Matrix = result;
        return new Error(Error.Type.Ok);
    }

    public Error AddKRows(int index, int[][] newRows)
    {
        if (!(0 <= index && index <= JaggedArray.Length))
            return new Error(Error.Type.OutOfRange);
        if (newRows.Length == 0)
            return new Error(Error.Type.WrongSize);

        var result = new int[JaggedArray.Length + newRows.Length][];
        Array.Copy(JaggedArray, 0, result, 0, index);
        Array.Copy(newRows, 0, result, index, newRows.Length);
        Array.Copy(JaggedArray, index, result, index + newRows.Length, JaggedArray.Length - index);

        JaggedArray = result;
        return new Error(Error.Type.Ok);
    }
}

public class Task6
{
    private static readonly Regex SentencePattern = new(@"\S.*?(?:[.!?](?=\s|$)|$)");

    public Error RemoveFirstAndLastSentence(ref string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new Error(Error.Type.EmptyText);

        var trimmed = text.Trim();
        var sentences = SentencePattern.Matches(trimmed);
        if (sentences.Count < 3)
        {
            text = string.Empty;
            return new Error(Error.Type.Ok);
        }

        var result = "";
        for (int i = 1; i < sentences.Count - 1; i++)
            result += sentences[i].Value + " ";

        text = result.Trim();
        return new Error(Error.Type.Ok);
    }
}
