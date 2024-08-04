namespace Sorting.Common;

// Bucket Sort is a sorting algorithm that works by dividing an array into a number of buckets.
// Each bucket is then sorted individually, either using a different sorting algorithm, or by recursively applying the bucket sorting algorithm.
// It is a distribution sort, and is a cousin of radix sort in the most to least significant digit flavor.
// This version of the Bucket Sort algorithm works specifically with positive integers, and it sorts the buckets using QuickSort.
// Bucket sort is a sorting strategy that trades space for time.
// Theoretically, if we use the same number of buckets as the number of records in the sequence, we can achieve linear time complexity.
// However, when the number of records is huge, it might cause huge space usage, and might even make the sorting impossible.
// Bucket sort is in practice slower than high-efficiency sorting algorithms such as quick sort.
// However, it is faster than traditional sorting methods.

public static class BucketSort
{
    // Sorts an integer array using Bucket Sort algorithm
    public static void Sort(int[] array)
    {
        // Get the maximum number of digits in the numbers to sort
        var maxDigit = GetMaxDigit(array);

        // Initialize the buckets
        var buckets = new List<int>[maxDigit];

        // Distribute the elements into the buckets
        for (int i = 0; i < array.Length; i++)
        {
            var index = GetDigit(array[i], maxDigit);
            if (buckets[index] == null)
                buckets[index] = new List<int>();

            buckets[index].Add(array[i]);
        }

        // Sort the buckets and collect the elements back into the array
        var k = 0;
        for (int i = 0; i < buckets.Length; i++)
        {
            if (buckets[i] == null)
                continue;

            QuickSortClassic.Sort(ref buckets[i]);
            for (int j = 0; j < buckets[i].Count; j++)
            {
                array[k++] = buckets[i][j];
            }
        }
    }

    // Get the maximum number of digits in the array
    private static int GetMaxDigit(int[] array)
    {
        var max = array[0];

        // Find the maximum number in the array
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > max)
                max = array[i];
        }

        var digit = 0;

        // Count the number of digits in the maximum number
        while (max > 0)
        {
            max /= 10;
            digit++;
        }

        return digit;
    }

    // Get the digit at the specified position in the number
    private static int GetDigit(int number, int digit)
    {
        var result = number;

        // Divide the number by 10 for each position to get to the digit
        for (int i = 0; i < digit; i++)
        {
            result /= 10;
        }

        // Return the digit at the position
        return result % 10;
    }
}
