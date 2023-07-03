namespace Sorting.Common;

// The Merge Sort algorithm is a divide-and-conquer sorting algorithm that works by recursively dividing the input array into two halves, sorting each half,
// and then merging the two sorted halves back together.
// The total time complexity of this algorithm is O(n log2 n), but this algorithm requires O(n) auxiliary memory space.
// This algorithm is stable, meaning that the relative order of equal elements is preserved after sorting.
// This algorithm is not adaptive, meaning that it does not take advantage of existing order in the input array.
// This algorithm is not in-place, meaning that it requires O(n) auxiliary memory space.
// This algorithm is not online, meaning that it cannot sort an array as it receives it.
// Merge sort first views each individual record as an ordered sequence, and then obtains new sequences via two-way merges.
// In this way, it sorts all the records. Merge sort is faster than heap sort, though normally it is not as fast as quick sort.

public static class MergeSort
{
    // Sorts an integer array by reference
    public static void Sort(ref int[] array)
    {
        int[] arr = SortArray(array, 0, array.Length - 1);

        // Update the original array with the sorted elements
        array = arr;
    }

    // Recursive function to sort an integer array within the specified range
    private static int[] SortArray(int[] array, int startIndex, int endIndex)
    {
        // Base case: return a single-element array if there's only one element in the range
        if (startIndex == endIndex)
            return new int[] { array[startIndex] };

        // Find the middle index of the range
        int middle = (endIndex + startIndex) / 2;

        // Recursively sort the two halves of the range
        int[] firstHalf = SortArray(array, startIndex, middle);
        int[] secondHalf = SortArray(array, middle + 1, endIndex);

        // Merge the two sorted halves
        return Merge(firstHalf, secondHalf);
    }

    // Merges two sorted integer arrays into a single sorted array
    private static int[] Merge(int[] leftHalf, int[] rightHalf)
    {
        int length = leftHalf.Length + rightHalf.Length;
        int[] result = new int[length];

        int currLeft = 0;
        int currRight = 0;

        // Iterate through each element in the result array
        for (int i = 0; i < length; i++)
        {
            // If the left half is exhausted, copy the remaining elements from the right half
            if (currLeft == leftHalf.Length)
            {
                result[i] = rightHalf[currRight];
                currRight++;
            }
            // If the right half is exhausted, copy the remaining elements from the left half
            else if (currRight == rightHalf.Length)
            {
                result[i] = leftHalf[currLeft];
                currLeft++;
            }
            // If the current element in the left half is smaller or equal to the current
            // element in the right half, copy the element from the left half
            else if (leftHalf[currLeft] <= rightHalf[currRight])
            {
                result[i] = leftHalf[currLeft];
                currLeft++;
            }
            // Otherwise, copy the element from the right half
            else
            {
                result[i] = rightHalf[currRight];
                currRight++;
            }
        }

        return result;
    }
}
