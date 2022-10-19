using DataStructures;

namespace Sorting;

public static class HeapSort
{
    public static void Sort(ref int[] array, Heap<int> heap)
    {
        for (int i = 0; i < array.Length; i++)
        {
            heap.Insert(array[i]);
        }

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = heap.Extract();
        }
    }
}
