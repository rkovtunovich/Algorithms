using DataStructures.Heaps;

namespace Sorting.Common;

// The Heap Sort algorithm works by building a heap (a complete binary tree) from the input array,
// and then extracting the minimum (or maximum) element from the heap one by one until the heap is empty, effectively producing a sorted array. 
// When sorting sequences with relatively few records, heap sort is not recommended.
// However, when sorting sequences with a relatively large number of records, heap sort will be relatively effective, since the main time cost will be
// spent in the initialization of the heap and the “filtering” repeatedly done when adjusting the new heap. In the worst-case scenario, the time complexity of heap sort
// will still be O(n log2 n), while in comparison quick sort’s time complexity will deteriorate to O(n2). The fact that heap sort has relatively good time complexity even
// under the worst-case scenario is its biggest advantage.
// Heap sort is suitable for situations where the amount of data is huge, for example, millions of records.
// Heap sort does not require a lot of recursion or multidimensional temporary arrays, which makes it suitable for sequences with a huge amount of data.
// When there are more than millions of records, since quick sort and merge sort essentially base their ideas on recursion, stack overflow error might occur.
// Heap sort will construct a heap out of all the data, with the maximum (minimum) data at the top of the heap.
// Then it exchanges the top data with the last data in the sequence, then it rebuilds the heap and swaps data. In this manner, it sorts all the data.

public static class HeapSort<TKey, TValue> where TKey : INumber<TKey>
{
    // Sorts an array of keys using a custom Heap class
    public static void Sort(TKey[] array, Heap<TKey, TValue> heap)
    {
        // Insert each key from the input array into the heap
        for (int i = 0; i < array.Length; i++)
        {
            heap.Insert(array[i]);
        }

        // Extract the minimum (or maximum) key from the heap and store it in the input array
        // until the heap is empty, effectively producing a sorted array
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = heap.ExtractNode().Key;
        }
    }
}