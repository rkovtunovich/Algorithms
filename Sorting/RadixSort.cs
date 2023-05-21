using View;

namespace Sorting;
 
// Radix Sort is a non-comparative sorting algorithm with linear time complexity.
// It avoids comparison by creating and distributing items into buckets according to their radix.
// The radix of a number refers to the base of the number system.
// For example, for the decimal system, the radix is 10.
// 
// Here's a basic idea of how the Radix Sort algorithm works:
// 
// 1. Start from the least significant digit (for decimal numbers, this is usually the rightmost digit) and put the items into 10 buckets according to the value of the digit.
//    The numbers with the same value of the digit will go into the same bucket, and they will maintain their relative order (this makes the Radix Sort a stable sort). 
// 2. Empty all buckets back into the list, starting with the bucket for digit 0 and ending with the bucket for digit 9. 
// 3. Repeat this process for the next more significant digit, until all digits have been processed.
// 
// In the end, the numbers will be sorted.
// Because you distribute items into buckets and combine them back again, the process is often done twice:
// once for the least significant digit, and again for the most significant digit.


public static class RadixSort
{
    // Radix Sort function
    public static void Sort(int[] arr)
    {
        int max = GetMax(arr); // Find the maximum number to know the number of digits

        // Perform counting sort for every digit
        for (int exp = 1; max / exp > 0; exp *= 10)
            CountSort(arr, arr.Length, exp);
    }

    // A utility function to get maximum value in arr[]
    private static int GetMax(int[] arr)
    {
        int max = arr[0];
        for (int i = 1; i < arr.Length; i++)
            if (arr[i] > max)
                max = arr[i];
        return max;
    }

    // A function to do counting sort of arr[] according to the digit represented by exp
    private static void CountSort(int[] arr, int n, int exp)
    {
        int[] output = new int[n]; // output array
        int[] count = new int[10]; // Initialize count array

        // Store count of occurrences in count[]
        for (int i = 0; i < n; i++)
            count[(arr[i] / exp) % 10]++;

        Viewer.ShowArray(count);

        // Change count[i] so that count[i] now contains actual position of this digit in output[]
        for (int i = 1; i < 10; i++)
            count[i] += count[i - 1];

        Viewer.ShowArray(count);

        // Build the output array
        // This loop is one of the most important parts of the radix sort algorithm.
        // It operates on each digit position of the numbers in the array (specified by the 'exp' variable) and places the numbers in a temporary output array based on the counts calculated earlier.
        // 
        // Here's a breakdown of each part of the loop:
        // 
        // 1. `(arr[i] / exp) % 10`:
        //     This expression is used to isolate the digit at the current position being considered in the radix sort.
        //     The division by `exp` moves the decimal place to the right, effectively ignoring digits that are less significant than the current digit position, and the modulus operation with 10 isolates the least significant digit of the result. 
        //     For example, if `arr[i]` is 1234 and `exp` is 100 (meaning we are considering the hundreds digit), `(arr[i] / exp)` would be 12.34, and `(arr[i] / exp) % 10` would be 2.
        // 2. `count[(arr[i] / exp) % 10] - 1`:
        //     This index is used to place the number into the output array.
        //     The count array at this index has been set up to represent the number of elements that have a digit less than or equal to the current digit at the current digit position.
        // 3. `output[count[(arr[i] / exp) % 10] - 1] = arr[i];`:
        //     Here we place `arr[i]` into the output array at the index calculated in the previous step. 
        // 4. `count[(arr[i] / exp) % 10]--;`:
        //    This line decrements the count at the current index.
        //    This is done because we just placed one number with this digit into the output array, so there's one less spot needed for such numbers in the future.
        // 5. The loop starts from `i = n - 1` and goes down to `i = 0`.
        //    It goes in this reverse order to maintain the relative order of equal elements, ensuring the sort is stable.
        // 
        // After all numbers have been placed in the output array for the current digit position, the output array is a version of the original array sorted by the current and all previous digit positions.
        // This process is repeated for all digit positions from least to most significant, which sorts the entire array.

        for (int i = n - 1; i >= 0; i--)
        {
            output[count[(arr[i] / exp) % 10] - 1] = arr[i];
            Console.WriteLine($"i:{i} item = {arr[i]} {(arr[i] / exp) % 10} count index = {(arr[i] / exp) % 10} output index = {count[(arr[i] / exp) % 10] - 1}");
            count[(arr[i] / exp) % 10]--;

            Viewer.ShowArray(count, nameof(count));
            Viewer.ShowArray(output, nameof(output));
        }

        // Copy the output array to arr[], so that arr[] now contains sorted numbers according to current digit
        for (int i = 0; i < n; i++)
            arr[i] = output[i];
    }
}
