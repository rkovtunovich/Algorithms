using Sorting.Common;

namespace Helpers;

public static class ArrayHelper
{
    public static int[] GetUnsortedArray(int length = 10, int min = 0, int max = 100, bool uniqueItems = false)
    {
        var set = new HashSet<int>();

        Random random = new Random();
        int[] array = new int[length];
        for (int i = 0; i < array.Length; i++)
        {
            int item = random.Next(min, max);
            if (uniqueItems && set.Contains(item))
            {
                i--;
                continue;
            }

            set.Add(item);
            array[i] = item;
        }

        return array;
    }

    public static int[] GetUnimodalArray(int length = 10, int min = 0, int max = 100)
    {
        var sourse = GetUnsortedArray(length, min, max, true);
        MergeSort.Sort(ref sourse);

        var array = new int[length];

        int leftIndex = 0;
        int rightIndex = length - 1;

        var random = new Random();
        for (int i = 0; i < length; i++)
        {
            int balanceWeight = random.Next(1, 3);
            if (balanceWeight % 2 == 0)
            {
                array[leftIndex] = sourse[i];
                leftIndex++;
            }

            else
            {
                array[rightIndex] = sourse[i];
                rightIndex--;
            }
        }

        return array;
    }

    public static int[] GetSortedArray(int length = 10, int min = 0, int max = 100, bool uniqueItems = false)
    {
        var array = GetUnsortedArray(length, min, max, uniqueItems);
        MergeSort.Sort(ref array);

        return array;
    }

    public static int[] GetMonotonicArray(int length = 10, int min = 0, bool reverse = false)
    {
        var array = new int[length];

        for (int i = 0; i < length; i++)
        {
            if(reverse)
                array[i] = length - i - 1 + min;
            else
                array[i] = i + min; 
        }

        return array;
    }
}
