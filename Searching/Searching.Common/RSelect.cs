namespace Searching.Common;

// description: Randomized selection algorithm to find the k-th order statistic in an array.
// The k-th order statistic of an array is the k-th smallest element in the array.
// The algorithm is based on the quicksort algorithm, but instead of sorting the whole array,
// it only sorts the part of the array that contains the k-th order statistic.
// time complexity: O(n) on average, O(n^2) in the worst case
// space complexity: O(1)

public class RSelect
{
    private static readonly Random _random = new();

    public static int Find(int[] array, int orderStatistics)
    {
        return FindRec(array, 0, array.Length - 1, orderStatistics);
    }

    private static int FindRec(int[] array, int startIndex, int endIndex, int orderStatistics)
    {
        if (startIndex >= endIndex)
            return array[endIndex];

        int pivotIndex = GetBaseIndexRandom(startIndex, endIndex);
        (array[pivotIndex], array[endIndex]) = (array[endIndex], array[pivotIndex]); // move pivot to the end
        pivotIndex = endIndex;

        int innerBorder = startIndex; // border between smaller and greater elements compared with base

        for (int i = startIndex; i < endIndex; i++)
        {
            if (array[i] < array[pivotIndex])
            {
                (array[i], array[innerBorder]) = (array[innerBorder], array[i]);

                innerBorder++;
            }
        }

        (array[pivotIndex], array[innerBorder]) = (array[innerBorder], array[pivotIndex]);

        if (innerBorder == orderStatistics)
            return array[innerBorder];
        else if (innerBorder > orderStatistics)
            return FindRec(array, startIndex, innerBorder - 1, orderStatistics);
        else
            return FindRec(array, innerBorder + 1, endIndex, orderStatistics);
    }

    private static int GetBaseIndexRandom(int min, int max)
    {
        return _random?.Next(min, max) ?? 0;
    }
}

