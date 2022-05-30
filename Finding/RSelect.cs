namespace Finding;

public class RSelect
{
    private static readonly Random _random = new();

    public static int Find(int[] array, int orderStatistics)
    {
        return FindRec(array, 0, array.Length - 1, orderStatistics);
    }

    private static int FindRec(int[] array, int startIndex, int endIndex, int orderStaticsics)
    {
        if(startIndex >= endIndex)
            return array[endIndex];

        int pivotIndex = GetBaseIndexRandom(startIndex, endIndex);
        (array[pivotIndex], array[endIndex]) = (array[endIndex], array[pivotIndex]); // move pivot to the end
        pivotIndex = endIndex;

        int innerBorder = startIndex; // border between smaller and greater elements compared with base

        for (int i = startIndex; i < endIndex; i++)
        {
            if (array[i] < array[pivotIndex])
            {
                (array[i], array[innerBorder]) = (array[innerBorder], array[i]);

                innerBorder++;
            }
        }

        (array[pivotIndex], array[innerBorder]) = (array[innerBorder], array[pivotIndex]);

        if (innerBorder == orderStaticsics)
            return array[innerBorder];
        else if (innerBorder > orderStaticsics)
            return FindRec(array, startIndex, innerBorder - 1, orderStaticsics);
        else
            return FindRec(array, innerBorder + 1, endIndex, orderStaticsics);
    }

    private static int GetBaseIndexRandom(int min, int max)
    {
        return _random?.Next(min, max) ?? 0;
    }
}

