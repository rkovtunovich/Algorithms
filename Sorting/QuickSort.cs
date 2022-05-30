using System;
using View;

namespace Sorting;

public class QuickSort
{
    private static readonly Random _random = new();

    public static void Sort<T>(ref T[] array) where T : INumber<T>
    {
        SortRec(array, 0, array.Length - 1);
    }

    private static void SortRec<T>(T[] array, int leftIndex, int rightIndex) where T : INumber<T>
    {
        if (leftIndex == rightIndex)
            return;

        int pivotIndex = GetBaseIndexRandom(leftIndex, rightIndex);
   
        (array[pivotIndex], array[rightIndex]) = (array[rightIndex], array[pivotIndex]);
        pivotIndex = rightIndex;

        int innerBorder = leftIndex; // border between smaller and greater elements compared with base

        for (int i = leftIndex; i < rightIndex; i++)
        {
            if (array[i] < array[pivotIndex])
            {              
                (array[i], array[innerBorder]) = (array[innerBorder], array[i]);

                innerBorder++;
            }
        }

        (array[pivotIndex], array[innerBorder]) = (array[innerBorder], array[pivotIndex]);
 

        if (innerBorder != leftIndex)
            SortRec(array, leftIndex, innerBorder - 1);

        if (innerBorder != rightIndex)
            SortRec(array, innerBorder + 1, rightIndex);
    }

    private static int GetBaseIndexRandom(int min, int max)
    {
        return _random?.Next(min, max) ?? 0;
    }

    private static int Partition(int[] array, int start, int end)
    {
        int temp;//swap helper
        int marker = start;//divides left and right subarrays
        for (int i = start; i < end; i++)
        {
            if (array[i] < array[end]) //array[end] is pivot
            {
                temp = array[marker]; // swap
                array[marker] = array[i];
                array[i] = temp;
                marker += 1;
            }
        }
        //put pivot(array[end]) between left and right subarrays
        temp = array[marker];
        array[marker] = array[end];
        array[end] = temp;
        return marker;
    }

    private static void QuicksortWiki(int[] array, int start, int end)
    {
        if (start >= end)
            return;

        int pivot = Partition(array, start, end);
        QuicksortWiki(array, start, pivot - 1);
        QuicksortWiki(array, pivot + 1, end);
    }
}
