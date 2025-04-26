namespace Searching.MediansAndOrderStatistics;

public static class MinMaxSearch
{
    // Pairwise Comparisons:
    // 
    // Divide the numbers into pairs.
    // For each pair, compare the two elements:
    // The smaller element becomes a candidate for the minimum.
    // The larger element becomes a candidate for the maximum.
    // In this way, each comparison gives us information about both the minimum and maximum, reducing the number of overall comparisons.

    public static (int min, int max) FindMinMax(int[] array)
    {
        if (array.Length is 0)
            throw new ArgumentException("Array must not be empty", nameof(array));

        int n = array.Length;
        int min, max;

        // Initialize min and max
        if (n % 2 == 0)
        {
            min = Math.Min(array[0], array[1]);
            max = Math.Max(array[0], array[1]);
        }
        else
        {
            min = max = array[0];
        }

        // Start from the second element (if n is odd) or the third element (if n is even)
        int i = n % 2 == 0 ? 2 : 1;

        while (i < n - 1)
        {
            int smaller, larger;

            // Compare the current pair of elements
            if (array[i] < array[i + 1])
            {
                smaller = array[i];
                larger = array[i + 1];
            }
            else
            {
                smaller = array[i + 1];
                larger = array[i];
            }

            // Update min and max
            min = Math.Min(min, smaller);
            max = Math.Max(max, larger);

            // Move to the next pair
            i += 2;
        }

        return (min, max);
    }
}