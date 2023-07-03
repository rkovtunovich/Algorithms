namespace Sorting.Insertion;

// This is an implementation of the Direct Insertion Sort algorithm for sorting arrays.
// Insertion sort keeps inserting the values in the sequence into an already sorted sequence, until the end of this sequence.
// Insertion sort is an improvement on bubble sort.
// Now it is not widely used. However, since the algorithm is relatively simple, it is still effective for the sorting of some relatively small sequences.

public static class InsertionSort
{
    // The Sort method is a generic implementation of the Direct Insertion Sort algorithm
    // for sorting arrays of any type T that implements IComparable<T>.
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        // Iterate through the array starting from the second element.
        for (int i = 1; i < array.Length; i++)
        {
            // Store the current element as the key.
            T key = array[i];
            // Initialize the comparison index j as one less than the current index i.
            int j = i - 1;

            // Move elements greater than the key one position ahead in the array
            // until a smaller element is found or the start of the array is reached.
            while (j >= 0 && array[j].CompareTo(key) > 0)
            {
                array[j + 1] = array[j];
                j--;
            }
            // Insert the key in its correct sorted position in the array.
            array[j + 1] = key;
        }
    }
}