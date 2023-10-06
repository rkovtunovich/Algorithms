namespace Finding;

public static class BinarySearch
{
    // Searches an integer array for a specified value using the binary search algorithm
    public static int Search(int[] array, int value)
    {
        // Set the starting and ending indices of the search range
        int startIndex = 0;
        int endIndex = array.Length - 1;

        // While the starting index is less than or equal to the ending index
        while (startIndex <= endIndex)
        {
            // Find the middle index of the search range
            int middleIndex = (startIndex + endIndex) / 2;

            // If the value is found at the middle index, return the middle index
            if (array[middleIndex] == value)
                return middleIndex;

            // If the value is less than the value at the middle index, search the left half of the search range
            if (value < array[middleIndex])
                endIndex = middleIndex - 1;
            // If the value is greater than the value at the middle index, search the right half of the search range
            else
                startIndex = middleIndex + 1;
        }

        // Return -1 if the value is not found
        return -1;
    }
}
