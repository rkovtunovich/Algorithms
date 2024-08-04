namespace Sorting.QuickSort;

// Quick sort is an improvement on bubble sort.
// The QuickSort algorithm is a divide-and-conquer sorting algorithm that works by selecting a 'pivot' element from the array and partitioning the other elements into two groups,
// according to whether they are less than or greater than the pivot. The sub-arrays are then sorted recursively.
// Quick sort essentially is a recursive algorithm using divide-and-conquer idea.
// Under normal circumstances, quick sort is faster than most sorting algorithms, and is currently the publicly acknowledged fastest sorting method.
// However, when the sequence is basically ordered, it will deteriorate into bubble sort, which affects the performance of the sort.
// In addition, quick sort is based on recursion and involves a huge amount of stack operations in the memory.
// For machines with very limited memory, it would not be a good choice.

public class QuickSortClassic
{
    // A random number generator to be used for selecting pivot indices
    private static readonly Random _random = new();

    // Sorts an array of elements of type T
    public static void Sort<T>(T[] array, bool isDescending = false) where T : INumber<T>
    {
        if (array.Length is 0)
            return;

        SortRec(array, 0, array.Length - 1, isDescending);
    }

    // Sorts a list of elements of type T and updates the original list
    public static void Sort<T>(ref List<T> list, bool isDescending = false) where T : INumber<T>
    {
        // Convert the list to an array
        var array = list.ToArray();

        // Sort the array
        SortRec(array, 0, array.Length - 1, isDescending);

        // Update the original list with the sorted element
        list = [.. array];
    }

    // Recursive function to sort an array of elements of type T within the specified range
    private static void SortRec<T>(T[] array, int leftIndex, int rightIndex, bool isDescending) where T : INumber<T>
    {
        // Base case: return if there's only one element in the range
        if (leftIndex == rightIndex)
            return;

        // Randomly select a pivot index within the range
        int pivotIndex = GetBaseIndexRandom(leftIndex, rightIndex);

        // Swap the pivot element with the last element in the range
        (array[pivotIndex], array[rightIndex]) = (array[rightIndex], array[pivotIndex]);
        pivotIndex = rightIndex;

        // Initialize the border between smaller and greater elements compared to the pivot
        int innerBorder = leftIndex;

        // Partition the elements based on their comparison with the pivot element
        for (int i = leftIndex; i < rightIndex; i++)
        {
            var isNeedSwap = isDescending ? array[i] > array[pivotIndex] : array[i] < array[pivotIndex];

            if (isNeedSwap)
            {
                // Swap the current element with the element at the border
                (array[i], array[innerBorder]) = (array[innerBorder], array[i]);

                // Move the border one position to the right
                innerBorder++;
            }
        }

        // Swap the pivot element with the element at the border
        (array[pivotIndex], array[innerBorder]) = (array[innerBorder], array[pivotIndex]);

        // Recursively sort the sub-arrays on the left and right of the pivot
        if (innerBorder != leftIndex)
            SortRec(array, leftIndex, innerBorder - 1, isDescending);

        if (innerBorder != rightIndex)
            SortRec(array, innerBorder + 1, rightIndex, isDescending);
    }

    // Returns a random integer between min (inclusive) and max (exclusive)
    private static int GetBaseIndexRandom(int min, int max)
    {
        return _random?.Next(min, max) ?? 0;
    }
}
