using DataStructures.Heaps;
using DataStructures.Lists;
using Sorting.Insertion;

namespace Sorting.Common;

public static class MergeInsertionSort
{
    public static void Sort<T>(ref T[] array, int insertionSortCutoff = 8) where T : IComparable<T>
    {
        var slices = new SequentialList<(int start, int end)>();
        SortPart(array, 0, array.Length - 1, insertionSortCutoff, slices);

        MergeKWay(ref array, slices);
    }

    private static void SortPart<T>(T[] array, int left, int right, int insertionSortCutoff, SequentialList<(int start, int end)> slices) where T : IComparable<T>
    {
        if (left >= right)
            return;

        if (right - left <= insertionSortCutoff)
        {
            // +1 because right is the index of the last element
            var slice = new Span<T>(array, left, right - left + 1); 
            InsertionSort.Sort(slice);

            slices.Add((left, right));

            return;
        }

        int mid = left + (right - left) / 2;
        SortPart(array, left, mid, insertionSortCutoff, slices);
        SortPart(array, mid + 1, right, insertionSortCutoff, slices);
    }

    private static void MergeKWay<T>(ref T[] array, SequentialList<(int start, int end)> slices) where T : IComparable<T>
    {
        var heap = new HeapMin<T, Slice>();
        var output = new T[array.Length];

        // Initialize the heap with the first element from each slice
        for(int i = 0; i < slices.Count; i++)
        {
            var(start, end) = slices[i];
            heap.Insert(array[start], new(i, start, end));
        }

        int index = 0;
        while (!heap.Empty)
        {
            var node = heap.ExtractNode();
            var(sliceIndex, start, end) = node.Value;
            output[index] = node.Key;
            index++;

            start++;
            
            if (start <= end)           
                heap.Insert(array[start], new(sliceIndex, start, end));          
        }

        array = output;
    }

    private record struct Slice (int sliceIndex, int start, int end);
}

