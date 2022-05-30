namespace Finding;

public static class MaxUnimodalArray
{
    public static int Find(int[] array)
    {
        return FindRec(array, 0, array.Length - 1, 0);
    }

    private static int FindRec(int[] array, int startIndex, int endIndex, int currMax)
    {
        Console.WriteLine($"call {startIndex} - {endIndex}");

        if (endIndex - startIndex == 1)
            return array[startIndex];
        else if (endIndex - startIndex == 2)
            return Math.Max(array[startIndex], array[endIndex]);

        int midlle = (endIndex + startIndex) / 2;

        currMax = Math.Max(array[midlle], currMax);

        int maxLeft = array[midlle - 1];
        if (maxLeft > currMax)
            maxLeft = FindRec(array, startIndex, midlle, maxLeft);

        currMax = Math.Max(maxLeft, currMax);

        int maxRight = array[midlle + 1];
        if (maxRight > currMax)
            maxRight = FindRec(array, midlle + 1, endIndex, maxRight);

        currMax = Math.Max(currMax, maxRight);

        return currMax;
    }
}
