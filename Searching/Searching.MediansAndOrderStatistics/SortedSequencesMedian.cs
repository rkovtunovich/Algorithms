namespace Searching.MediansAndOrderStatistics;

public static class SortedSequencesMedian
{
    /// <summary>
    /// Finds the median of two sorted arrays using binary search.
    /// The arrays must be sorted and have no duplicate elements.
    /// This method has a time complexity of O(log(min(n, m))) where n and m are the lengths of the two arrays.
    /// </summary>
    /// <param name="sequence1">The first sorted array.</param>
    /// <param name="sequence2">The second sorted array.</param>
    /// <returns>The median of the two arrays.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the input arrays are not sorted</exception>
    public static double FindMedian(int[] sequence1, int[] sequence2)
    {
        // If both arrays are empty, return 0
        if (sequence1.Length is 0 && sequence2.Length is 0)
            return 0;

        // Ensure that sequence1 is the smaller array for binary search efficiency
        if (sequence1.Length > sequence2.Length)
            return FindMedian(sequence2, sequence1);

        int x = sequence1.Length;
        int y = sequence2.Length;

        int low = 0;
        int high = x;

        while (low <= high)
        {
            // Partition indices for both arrays
            int partitionX = (low + high) / 2;
            int partitionY = (x + y + 1) / 2 - partitionX;

            // Handling edge cases where partition is at the beginning or end
            int maxLeftX = partitionX == 0 ? int.MinValue : sequence1[partitionX - 1];
            int minRightX = partitionX == x ? int.MaxValue : sequence1[partitionX];

            int maxLeftY = partitionY == 0 ? int.MinValue : sequence2[partitionY - 1];
            int minRightY = partitionY == y ? int.MaxValue : sequence2[partitionY];

            // Check if we have found the correct partitions
            if (maxLeftX <= minRightY && maxLeftY <= minRightX)
            {
                // If the combined length is even, the median is the average of the two middle values
                if ((x + y) % 2 is 0)
                    return (Math.Max(maxLeftX, maxLeftY) + Math.Min(minRightX, minRightY)) / 2.0;
                // If the combined length is odd, the median is the maximum of the left side
                else
                    return Math.Max(maxLeftX, maxLeftY);
            }
            // Adjust the binary search range
            else if (maxLeftX > minRightY)
                high = partitionX - 1;
            else
                low = partitionX + 1;
        }

        // If input arrays are not sorted, throw an exception
        throw new InvalidOperationException("Input arrays are not sorted.");
    }
}
