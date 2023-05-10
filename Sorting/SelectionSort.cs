namespace Sorting;

// The Selection Sort algorithm works by dividing the input array into a sorted and unsorted region.
// In each iteration, the algorithm finds the smallest (or largest) element in the unsorted region and moves it to the end of the sorted region. 

public static class SelectionSort
{
    // Sorts an array of elements of type T
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        // Get the length of the array
        int N = array.Length;

        // Iterate through each element in the array
        for (int i = 0; i < N; i++)
        {
            // Assume the current element has the minimum value in the unsorted region
            int minIndex = i;

            // Iterate through the remaining elements in the unsorted region
            for (int j = i + 1; j < N; j++)
            {
                // If the current element is smaller than the assumed minimum,
                // update the minimum index
                if (array[j].CompareTo(array[minIndex]) < 0)
                {
                    minIndex = j;
                }
            }

            // Swap the element with the minimum value found in the unsorted region
            // with the first unsorted element, effectively adding it to the sorted region
            (array[i], array[minIndex]) = (array[minIndex], array[i]);
        }
    }
}