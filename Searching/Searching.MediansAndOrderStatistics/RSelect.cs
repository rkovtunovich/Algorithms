namespace Searching.MediansAndOrderStatistics;

// description: Randomized selection algorithm to find the k-th order statistic in an array.
// The k-th order statistic of an array is the k-th smallest element in the array.
// The algorithm is based on the quicksort algorithm, but instead of sorting the whole array,
// it only sorts the part of the array that contains the k-th order statistic.
// time complexity: O(n) on average, O(n^2) in the worst case
// space complexity: O(1)

public class RSelect
{
    // Static Random instance to be used for generating random pivot indices.
    private static readonly Random _random = new();

    /// <summary>
    /// Public method to find the k-th smallest element in an unsorted array (order statistic).
    /// This is the entry point for the Randomized-Select algorithm.
    /// </summary>
    /// <param name="array">The input array of integers.</param>
    /// <param name="orderStatistics">The order statistic to find (0-based index, i.e., 0 means the smallest element).</param>
    /// <returns>The k-th smallest element in the array.</returns>
    public static int Find(int[] array, int orderStatistics)
    {
        // Ensure that the input order statistic is within bounds.
        if (orderStatistics < 0 || orderStatistics >= array.Length)
            throw new ArgumentOutOfRangeException(nameof(orderStatistics), "Order statistic is out of range.");

        // Call the recursive helper function to find the k-th smallest element.
        return FindRec(array, 0, array.Length - 1, orderStatistics);
    }

    public static int Find(List<int> sequence, int orderStatistics)
    {
        return Find(sequence.ToArray(), orderStatistics);
    }

    /// <summary>
    /// Recursive helper method to implement Randomized-Select.
    /// Partitions the array around a randomly chosen pivot and recursively processes the appropriate subarray.
    /// </summary>
    /// <param name="array">The array being processed.</param>
    /// <param name="startIndex">The starting index of the current subarray.</param>
    /// <param name="endIndex">The ending index of the current subarray.</param>
    /// <param name="orderStatistics">The order statistic to find (0-based index).</param>
    /// <returns>The k-th smallest element in the current subarray.</returns>
    private static int FindRec(int[] array, int startIndex, int endIndex, int orderStatistics)
    {
        // Base case: if the subarray has one element, return it (this is the k-th element).
        if (startIndex >= endIndex)
            return array[endIndex];

        // Randomly select a pivot index within the current range.
        int pivotIndex = GetBaseIndexRandom(startIndex, endIndex);

        // Move the chosen pivot to the end of the current subarray for easier partitioning.
        (array[pivotIndex], array[endIndex]) = (array[endIndex], array[pivotIndex]);
        pivotIndex = endIndex; // Now the pivot is at the end.

        // Partition the array: `innerBorder` will separate elements smaller than the pivot from larger ones.
        int innerBorder = startIndex;

        // Traverse the array and move elements smaller than the pivot to the left.
        for (int i = startIndex; i < endIndex; i++)
        {
            if (array[i] < array[pivotIndex])
            {
                // Swap smaller element to the `innerBorder` position.
                (array[i], array[innerBorder]) = (array[innerBorder], array[i]);
                innerBorder++;
            }
        }

        // Place the pivot in its final sorted position (between smaller and larger elements).
        (array[pivotIndex], array[innerBorder]) = (array[innerBorder], array[pivotIndex]);

        // Now, `innerBorder` holds the index of the pivot in the sorted order.
        if (innerBorder == orderStatistics)
            return array[innerBorder]; // We found the k-th smallest element.
        else if (innerBorder > orderStatistics)
            // Recurse into the left part if the order statistic is smaller than the pivot index.
            return FindRec(array, startIndex, innerBorder - 1, orderStatistics);
        else
            // Recurse into the right part if the order statistic is greater than the pivot index.
            return FindRec(array, innerBorder + 1, endIndex, orderStatistics);
    }

    /// <summary>
    /// Helper method to generate a random index between min and max, inclusive.
    /// </summary>
    /// <param name="min">The minimum index (inclusive).</param>
    /// <param name="max">The maximum index (inclusive).</param>
    /// <returns>A random integer between min and max.</returns>
    private static int GetBaseIndexRandom(int min, int max)
    {
        return _random.Next(min, max);
    }
}

