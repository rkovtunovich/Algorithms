using Sorting.Common;

namespace Helpers;

public static class ArrayHelper
{
    public static int[] GetUnsortedArray(int lenght = 10, int min = 0, int max = 100, bool uniqueItems = false)
    {
        var set = new HashSet<int>();

        Random random = new Random();
        int[] array = new int[lenght];
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

    public static int[] GetUnimodalArray(int lenght = 10, int min = 0, int max = 100)
    {
        var sourse = GetUnsortedArray(lenght, min, max, true);
        MergeSort.Sort(ref sourse);

        var array = new int[lenght];

        int leftIndex = 0;
        int rightIndex = lenght - 1;

        var random = new Random();
        for (int i = 0; i < lenght; i++)
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

    public static int[] GetSortedArray(int lenght = 10, int min = 0, int max = 100, bool uniqueItems = false)
    {
        var array = GetUnsortedArray(lenght, min, max, uniqueItems);
        MergeSort.Sort(ref array);

        return array;
    }

    public static int[] GetMomotonicArray(int lenght = 10, int min = 0, bool reverse = false)
    {
        var array = new int[lenght];

        for (int i = 0; i < lenght; i++)
        {
            if(reverse)
                array[i] = lenght - i - 1 + min;
            else
                array[i] = i + min; 
        }

        return array;
    }
}
