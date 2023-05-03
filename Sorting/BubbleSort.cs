namespace Sorting;

public static class BubbleSort
{
    public static void Sort<T>(T[] array) where T : IComparable<T>
    { 
        int n = array.Length;
        bool swapped;

        do
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if (array[i - 1].CompareTo(array[i]) > 0)
                {
                    (array[i], array[i - 1]) = (array[i - 1], array[i]);
                    swapped = true;
                }
            }
            n--;
        } while (swapped);
    }
}