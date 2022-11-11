using DataStructures.Heaps;
using System.Numerics;

namespace Sorting;

public static class HeapSort<TKey, TValue> where TKey : INumber<TKey>
{
    public static void Sort(ref TKey[] array, Heap<TKey, TValue> heap)
    {
        for (int i = 0; i < array.Length; i++)
        {
            heap.Insert(array[i]);
        }

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = heap.ExtractNode().Key;
        }
    }
}
