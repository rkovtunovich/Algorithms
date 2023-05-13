namespace View;

public static class Viewer
{
    public static void ShowArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length && array.Length < 100; i++)
        {
            if (typeof(T) == typeof(double))
                Console.Write($"{GetFormat(i)} {array[i]:0.00}");
            else
                Console.Write($"{GetFormat(i)} {array[i]}");
        }

        Console.Write("\n");
        Console.WriteLine($"-------------------");
    }

    public static void ShowMatrix<T>(T[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
                Console.Write($"\t {matrix[i][j]}");

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public static void ShowMatrix<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write($"\t {matrix[i, j]}");

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    private static string GetFormat(int i)
    {
        var format = "";

        if (i == 0)
            return format;

        format = i % 10 == 0 ? "\n" : "\t";

        return format;
    }
}
