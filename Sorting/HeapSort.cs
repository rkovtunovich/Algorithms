using DataStructures.Heaps;

namespace Sorting;

// The Heap Sort algorithm works by building a heap (a complete binary tree) from the input array,
// and then extracting the minimum (or maximum) element from the heap one by one until the heap is empty,
// effectively producing a sorted array. 
// When sorting sequences with relatively few records, heap sort is not recommended.
// However, when sorting sequences with a relatively large number of records, heap sort will be relatively effective, since the main time cost will be
// spent in the initialization of the heap and the “filtering” repeatedly done when adjusting the new heap. In the worst-case scenario, the time complexity of heap sort
// will still be O(n log2 n), while in comparison quick sort’s time complexity will deteriorate to O(n2). The fact that heap sort has relatively good time complexity even
// under the worst-case scenario is its biggest advantage.

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