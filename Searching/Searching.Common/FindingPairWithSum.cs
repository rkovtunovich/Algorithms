using Sorting.Common;

namespace Searching.Common;

public class FindingPairWithSum
{
    // Finds a pair of integers in an integer array that sum to a specified value
    // using the binary search algorithm
    public static int[]? FindPairBinarySearch(int[] array, int sum)
    {
        // Sort the array
        QuickSort.Sort(array);

        // Iterate through each element in the array
        for (int i = 0; i < array.Length; i++)
        {
            // Find the difference between the current element and the sum
            int difference = sum - array[i];

            // Find the index of the difference in the array
            int index = BinarySearch.Search(array, difference);

            // If the difference is found and the index is not the same as the current index, return the pair
            if (index != -1 && index != i)
                return new int[] { array[i], array[index] };
        }

        // Return null if no pair is found
        return null;
    }

    // Finds a pair of integers in an integer array that sum to a specified value
    // using the two-pointer algorithm
    public static int[]? FindPairTwoPointer(int[] array, int sum)
    {
        // Sort the array
        QuickSort.Sort(array);

        // Set the starting and ending indices of the search range
        int startIndex = 0;
        int endIndex = array.Length - 1;

        // While the starting index is less than the ending index
        while (startIndex < endIndex)
        {
            // Find the sum of the elements at the starting and ending indices
            int currSum = array[startIndex] + array[endIndex];

            // If the sum is equal to the specified sum, return the pair
            if (currSum == sum)
                return [array[startIndex], array[endIndex]];

            // If the sum is less than the specified sum, increment the starting index
            if (currSum < sum)
                startIndex++;
            // If the sum is greater than the specified sum, decrement the ending index
            else
                endIndex--;
        }

        // Return null if no pair is found
        return null;
    }
}
