namespace Sorting.Distribution;

public class CountingSort
{
    // Sorts the input array in ascending order using the counting sort algorithm.
    // The input array should contain values in the range [0, maxValue].
    // The maxValue parameter specifies the maximum value in the input array.
    // Time complexity: O(n + k), where n is the number of elements in the input array and k is the range of input values.
    // One of the important properties of counting sort is that it is stable, meaning that the relative order of equal elements is preserved.
    // This makes it suitable for sorting objects with multiple keys, where the order of keys matters
    public static void Sort(int[] array, int maxValue)
    {
        // Create an array to store the count of each value in the input array.
        var counts = new int[maxValue + 1];

        // Count the occurrences of each value in the input array.
        foreach (var value in array)
            counts[value]++;

        // Update the counts array to store the number of elements less than or equal to each value.
        for (var i = 1; i < counts.Length; i++)
            counts[i] += counts[i - 1];

        // Create a temporary array to store the sorted output.
        var sortedArray = new int[array.Length];

        // Iterate over the input array in reverse order to maintain stability.
        for (var i = array.Length - 1; i >= 0; i--)
        {
            var value = array[i];
            var index = counts[value] - 1;
            sortedArray[index] = value;
            counts[value]--;
        }

        // Copy the sorted output back to the input array.
        for (var i = 0; i < array.Length; i++)
            array[i] = sortedArray[i];

        // Note: The input array is now sorted in ascending order.
    }
}
