namespace Searching.Common;

public static class MatchIndexToArrayValue
{
    public static int step = 0;

    public static int IsExist(int[] array)
    {
        return Find(array, 0, array.Length - 1);
    }

    private static int Find(int[] array, int startIndex, int endIndex)
    {
        Console.WriteLine($"step {step++} :{startIndex} - {endIndex}");

        if (endIndex - startIndex == 0)
        {
            return startIndex == array[startIndex] ? startIndex : -1;
        }

        int middle = (startIndex + endIndex) / 2;

        if (middle == array[middle])
            return middle;

        int left = -1;
        if (array[middle] < middle && array[middle] > 0)
            left = Find(array, startIndex, middle);

        if (left != -1)
            return left;

        int right;
        if (endIndex > middle + 1)
            right = Find(array, middle + 1, endIndex);
        else
            right = array[middle + 1];

        return right;
    }
}
