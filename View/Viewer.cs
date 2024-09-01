namespace View;

public static class Viewer
{
    public static void ShowArray<T>(T[] array, string? description = null, Dictionary<T, string>? specialRepresentarions = null)
    {
        Console.WriteLine($"{description}{new string('_', Console.WindowWidth - description?.Length ?? 0)}");

        for (int i = 0; i < array.Length && array.Length < 100; i++)
        {
            if (specialRepresentarions?.ContainsKey(array[i]) ?? false)
                Console.Write($"{GetFormat(i)} {specialRepresentarions[array[i]]}");
            else if (typeof(T) == typeof(double))
                Console.Write($"{GetFormat(i)} {array[i]:0.00}");
            else
                Console.Write($"{GetFormat(i)} {array[i]}");
        }

        Console.Write("\n");
        Console.WriteLine(new string('_', Console.WindowWidth));
    }

    public static void ShowMatrix<T>(T?[][] matrix, Dictionary<T, string>? specialRepresentarions = null)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                if (specialRepresentarions?.ContainsKey(matrix[i][j]) ?? false)
                    Console.Write($"\t {specialRepresentarions[matrix[i][j]]}");
                else
                    Console.Write($"\t {matrix[i][j]}");
            }

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
