namespace Helpers;

public class MatrixHelper
{
    public static T[][] CreateQuadratic<T>(int size)
    {
        T[][] matrix = new T[size][];
        for (int i = 0; i < size; i++)
            matrix[i] = new T[size];

        return matrix;
    }

    public static int[][] CreateSimple2x2(int a1 = 0, int a2 = 0, int a3 = 3, int a4 = 0)
    {
        int[][] matrix = new int[2][];
        matrix[0] = new int[] { a1, a2 };
        matrix[1] = new int[] { a3, a4 };

        return matrix;
    }

    public static int[][] CreateSimple3x3(int[] row1, int[] row2, int[] row3)
    {
        int[][] matrix = new int[3][];
        matrix[0] = row1;
        matrix[1] = row2;
        matrix[2] = row3;

        return matrix;
    }

    public static void FillRandomly(ref int[][] matrix, int min, int max)
    {
        var rand = new Random();
        for (int i = 0; i < matrix.Length; i++)
            for (int j = 0; j < matrix[i].Length; j++)
                matrix[i][j] = rand.Next(min, max);
    }

    public static void Fill(ref double[][] matrix, int min, int max)
    {
        var rand = new Random();
        for (int i = 0; i < matrix.Length; i++)
            for (int j = 0; j < matrix[i].Length; j++)
                matrix[i][j] = rand.Next(min, max) + rand.NextDouble();
    }
    
    public static int[][] SubMatrix(int[][] source, int startX, int startY, int size)
    {
        if (source.Length < startX + size)
            throw new ArgumentException("Out of matrix range");

        int[][] sub = new int[size][];
        for (int i = startX; i < startX + size; i++)
        {
            sub[i - startX] = new int[size];
            for (int j = startY; j < startY + size; j++)
                sub[i - startX][j - startY] = source[i][j];
        }             

        return sub;
    }
    
    public static int[][] Combine(int[][] a, int[][] b, int[][] c, int[][] d)
    {
        int[][] result = new int[a.Length + b.Length][];

        for (int i = 0;i < a.Length; i++)
        {
            result[i] = new int[a.Length + b.Length];

            Array.Copy(a[i], 0, result[i], 0, a.Length);
            Array.Copy(b[i], 0, result[i], b.Length, b.Length);
        }

        int shift = a.Length;

        for (int i = 0; i < c.Length; i++)
        {
            result[i + shift] = new int[c.Length + d.Length];

            Array.Copy(c[i], 0, result[i + shift], 0, c.Length);
            Array.Copy(d[i], 0, result[i + shift], b.Length, b.Length);
        }

        return result;
    }
}