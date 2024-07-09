namespace MathAlgo.Common;

public static class KaratsubaMult
{
    // Karatsuba multiplication
    // divide and conquer algorithm
    // O(n^1.59) time complexity
    // recurrence relation: T(n) = 3T(n/2) + O(n)
    public static long Mult(int x, int y)
    {
        return RecMult(x, y);
    }

    private static long RecMult(int x, int y)
    {
        // Base case for single digit numbers
        if (x < 10 && y < 10)
            return x * y;

        // Get the length of the numbers
        int n = Math.Max(x.ToString().Length, y.ToString().Length);

        // Split position
        int m = (n / 2) + (n % 2);

        // Split x and y into high and low parts
        int high1 = x / (int)Math.Pow(10, m);
        int low1 = x % (int)Math.Pow(10, m);
        int high2 = y / (int)Math.Pow(10, m);
        int low2 = y % (int)Math.Pow(10, m);

        // 3 recursive calls    
        long z0 = RecMult(low1, low2);
        long z1 = RecMult((low1 + high1), (low2 + high2));
        long z2 = RecMult(high1, high2);

        // Karatsuba formula
        long result = (z2 * (int)Math.Pow(10, 2 * m)) + ((z1 - z2 - z0) * (int)Math.Pow(10, m)) + z0;

        return result;
    }
}
