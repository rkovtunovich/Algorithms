using Sorting.Insertion;

namespace Sorting.Common;

// To optimize the QuickSort algorithm by utilizing Insertion Sort for small subarrays,
// we can modify the QuickSort algorithm to use Insertion Sort when the size of the subarray is less than 𝑘.
// The idea is that Insertion Sort is efficient for nearly sorted sequences or small-sized arrays.
// This hybrid approach leverages the strengths of both algorithms.
//
// Steps to Hybrid QuickSort:
// 1. QuickSort: Use QuickSort to recursively sort the array. However, when the size of the subarray is less than 𝑘, stop the recursion.
// 2. Insertion Sort: After QuickSort has been applied to subarrays of size 𝑘 or more, use Insertion Sort to sort the entire array.
//
// Analysis:
// Let's denote the running time of QuickSort as 𝑇(𝑛).
// 1. Partitioning
//    - The partitioning step takes 𝑂(𝑛) time.
//    - For subarrays of size less than 𝑘, the partitioning step will not be performed.
// 2. Recursion
//    - The recurrence relation for the running time of hybrid QuickSort is: 𝑇(𝑛) = O(𝑛) + 2𝑇(𝑛/2) if 𝑛 > 𝑘, and 0 if 𝑛 ≤ 𝑘.
// 3. Solving the Recurrence Relation
//    - For n > k, the solution to the recurrence relation is 𝑇(𝑛) = O(n) + 2𝑇(𝑛/2) = O(n log n).
//    - The height of the recursion tree is log n, and each level takes O(n/k) time, because the partitioning step is not performed for subarrays of size less than k.
//    - At each level, the total time taken is O(n).
//    - Therefore, the total time complexity is O(n log n/k).
// 4. Insertion Sort
//    - Once the QuickSort reduces the subarrays to size k or less, the Insertion Sort is applied to the entire array.
//    - The InsertionSort runs O(n*k) time.
// 5. Total Time Complexity
//    - The total time complexity of the hybrid QuickSort is O(n log n/k) + O(n*k) = O(n log n + n*k).
//
// Choosing the value of k:
// To choose the optimal value of k, we need to balance the two components of the total time complexity:
// - O(n log n/k) for QuickSort
// - O(n*k) for Insertion Sort
// - For theoretical and practical considerations, k is typically chosen such as that log n/k = k.
// - That implies k = sqrt(log n).
// - Therefore, the optimal value of k = O(sqrt(n)).
public class QuickSortHybrid
{
    // A random number generator to be used for selecting pivot indices
    private static readonly Random _random = new();

    // Sorts an array of elements of type T
    public static void Sort<T>(T[] array, bool isDescending = false) where T : INumber<T>
    {
        if (array.Length is 0)
            return;

        SortRec(array, 0, array.Length - 1, isDescending);

        // Use Insertion Sort for the entire array
        InsertionSort.Sort(array, isDescending);
    }

    // Sorts a list of elements of type T and updates the original list
    public static void Sort<T>(ref List<T> list, bool isDescending = false) where T : INumber<T>
    {
        // Convert the list to an array
        var array = list.ToArray();

        // Sort the array
        SortRec(array, 0, array.Length - 1, isDescending);

        // Use Insertion Sort for the entire array
        InsertionSort.Sort(array, isDescending);

        // Update the original list with the sorted element
        list = [.. array];
    }

    // Recursive function to sort an array of elements of type T within the specified range
    private static void SortRec<T>(T[] array, int leftIndex, int rightIndex, bool isDescending) where T : INumber<T>
    {
        // If the size of the subarray is less than k, stop the recursion
        if (rightIndex - leftIndex + 1 <= DetermineK(array.Length))
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

    private static int DetermineK(int n) => (int)Math.Sqrt(n);
}
