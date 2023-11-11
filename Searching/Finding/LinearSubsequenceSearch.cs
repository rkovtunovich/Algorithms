namespace Searching.Common;

public static class LinearSubsequenceSearch<T> where T : IComparable<T>
{
    public static int[]? Search(T[] array, T[] subsequence)
    {
        var subsequencePointer = 0;
        var result = new int[subsequence.Length];

        // Iterate through each element in the array
        for (int i = 0; i < array.Length; i++)
        {
            // If the current element is not equal to the first element in the subsequence
            if (array[i].CompareTo(subsequence[subsequencePointer]) is not 0)
                continue;

            result[subsequencePointer] = i;

            subsequencePointer++;

            // If the subsequence is found, return the starting index
            if (subsequencePointer == subsequence.Length)
                return result;
        }

        // Return null if the subsequence is not found
        return null;
    }
}
