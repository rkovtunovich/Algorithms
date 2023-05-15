using DataStructures.Lists;

namespace Sorting;

public static class BucketSort
{
    public static void Sort(int[] array)
    {
        var digit = GetMaxDigit(array);
        var buckets = new List<int>[digit];
        for (int i = 0; i < array.Length; i++)
        {
            var index = GetDigit(array[i], digit);
            if (buckets[index] == null)
                buckets[index] = new List<int>();

            buckets[index].Add(array[i]);
        }

        var k = 0;
        for (int i = 0; i < buckets.Length; i++)
        {
            if (buckets[i] is null)
                continue;

            QuickSort.Sort<int>(ref buckets[i]);
            for (int j = 0; j < buckets[i].Count; j++)
            {
                array[k++] = buckets[i][j];
            }
        }
    }

    private static int GetMaxDigit(int[] array)
    {
        var max = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > max)
                max = array[i];
        }

        var digit = 0; 
        while (max > 0)
        {
            max /= 10;
            digit++;
        }

        return digit;
    }

    private static int GetDigit(int number, int digit)
    {
        var result = number;
        for (int i = 0; i < digit; i++)
        {
            result /= 10;
        }
        return result % 10;
    }
}
