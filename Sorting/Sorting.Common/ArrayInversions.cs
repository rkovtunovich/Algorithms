namespace Sorting.Common;

// This is an algorithm for counting inversions in an array.
// An inversion is a pair of elements in an array such that their order in the array is opposite to their natural order.
// For instance, in the array [2, 3, 8, 6, 1], the pair (8, 6) is an inversion since 8 comes before 6 in the array but is greater than 6.
// Counting inversions is a common problem in computer science and can be used, for example, in measuring how far an array is from being sorted.
// The algorithm is based on the divide-and-conquer approach, similar to Merge Sort.
// It splits the array into halves, counts inversions in each half recursively, and then counts inversions that occur during the merging of these halves.
// The merge step not only counts inversions but also sorts the array segments, which is crucial for correctly counting inversions across different parts of the array. 
// Time complexity: O(n log n)

public class ArrayInversions
{
    // Public method to count inversions in an array
    public static int Count(ref int[] array)
    {
        // Calls the recursive function to count inversions and sort the array
        (array, int count) = CountInversions(array, 0, array.Length - 1);

        return count;
    }

    // Recursive method to count inversions in a segment of the array
    private static (int[] array, int inversions) CountInversions(int[] array, int startIndex, int endIndex)
    {
        // Base case: if the segment is a single element, return it with 0 inversions
        if (startIndex == endIndex)
            return (new int[] { array[startIndex] }, 0);

        // Find the middle index to divide the array segment into two halves
        int middle = (endIndex + startIndex) / 2;

        // Recursively count inversions in the left half of the array segment
        (int[] leftHalf, int leftInv) = CountInversions(array, startIndex, middle);
        // Recursively count inversions in the right half of the array segment
        (int[] rightHalf, int rightInv) = CountInversions(array, middle + 1, endIndex);

        // Merge the two halves and count inversions caused during the merge
        (int[] merged, int mergeInversions) mergedResult = Merge(leftHalf, rightHalf);

        // Add inversions found in left, right, and during the merge
        mergedResult.mergeInversions += leftInv + rightInv;

        return mergedResult;
    }

    // Method to merge two halves of the array and count inversions
    private static (int[], int) Merge(int[] leftHalf, int[] rightHalf)
    {
        int length = leftHalf.Length + rightHalf.Length;
        int[] result = new int[length];
        int inversions = 0;

        int currLeft = 0, currRight = 0;
        for (int i = 0; i < length; i++)
        {
            // If the left half is exhausted, copy elements from the right half
            if (currLeft == leftHalf.Length)
            {
                result[i] = rightHalf[currRight++];
            }
            // If the right half is exhausted, copy elements from the left half
            else if (currRight == rightHalf.Length)
            {
                result[i] = leftHalf[currLeft++];
            }
            // If current element in left is less than or equal to right, copy it
            else if (leftHalf[currLeft] <= rightHalf[currRight])
            {
                result[i] = leftHalf[currLeft++];
            }
            // If current element in right is less than left, it's an inversion
            else
            {
                result[i] = rightHalf[currRight];
                inversions += leftHalf.Length - currLeft;
                currRight++;
            }
        }

        return (result, inversions);
    }
}

