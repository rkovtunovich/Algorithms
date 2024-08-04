using DataStructures.Heaps;
using Helpers;
using Searching.Common;
using Sorting.Common;
using Sorting.Insertion;
using Sorting.QuickSort;
using System.Diagnostics;

namespace ExamplesRunning;

internal class SortingExample
{
    static public void RunMergeSort()
    {
        var array = ArrayHelper.GetUnsortedArray(5);
        Viewer.ShowArray(array);
        var stopWatch = new Stopwatch();

        stopWatch.Start();
        MergeSort.Sort(ref array);
        stopWatch.Stop();

        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");

        stopWatch.Start();
        MergeSort2.Sort(ref array);
        stopWatch.Stop();

        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");

        array = ArrayHelper.GetUnsortedArray(5);

        Viewer.ShowArray(array);

        Console.WriteLine($"array contains {ArrayInversions.Count(ref array)} inversions");
        Console.ReadKey();
    }

    static public void RunQuickSort()
    {
        //var array = ArrayHelper.GetUnsortedArray(30000000, 0, 100000000);
        //int[] array = new[] { 7, 13, 4, 3, 4, 7 };
        var array = ArrayHelper.GetUnsortedArray(6, 0, 20);
        Viewer.ShowArray(array);

        var stopWatch = new Stopwatch();

        stopWatch.Start();
        QuickSortClassic.Sort(array);
        stopWatch.Stop();

        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");

        Viewer.ShowArray(array);

        Console.ReadKey();
    }

    static public void RunHeapSort()
    {
        var array = ArrayHelper.GetUnsortedArray(10, 0, 15);
        //int[] array = new[] { 9, 5, 4, 3, 9, 1, 6 };
        //var array = ArrayHelper.GetUnsortedArray(6, 0, 20);
        Viewer.ShowArray(array);

        HeapSort<int, int>.Sort(array);

        Viewer.ShowArray(array);

        HeapSort<int, int>.Sort(array);

        Viewer.ShowArray(array);

        Console.ReadKey();
    }

    static public void RunDirectInsertionSort()
    {
        var array = ArrayHelper.GetUnsortedArray(10, 0, 15);

        //int[] array = new[] { 9, 5, 4, 3, 9, 1, 6 };
        //var array = ArrayHelper.GetUnsortedArray(6, 0, 20);
        Viewer.ShowArray(array);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        InsertionSort.Sort(array, true);
        stopWatch.Stop();
        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");
        Viewer.ShowArray(array);
        Console.ReadKey();
    }

    static public void RunShellSort()
    {
        int[] numbers = new int[] { 5, 2, 8, 1, 9, 3, 7, 6, 4 };
        ShellSort.Sort(numbers);

        foreach (int number in numbers)
        {
            Console.Write(number + " ");
        }
        // Output: 1 2 3 4 5 6 7 8 9
    }

    static public void RunBucketSort()
    {
        var array = ArrayHelper.GetUnsortedArray(50, 0, 100);
        //int[] array = new[] { 9, 5, 4, 3, 9, 1, 6 };
        //var array = ArrayHelper.GetUnsortedArray(6, 0, 20);
        Viewer.ShowArray(array);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        BucketSort.Sort(array);
        stopWatch.Stop();
        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");
        Viewer.ShowArray(array);
        Console.ReadKey();
    }

    static public void RunRadixSort()
    {
        var array = ArrayHelper.GetUnsortedArray(10, 0, 100);
        //int[] array = new[] { 9, 5, 4, 3, 9, 1, 6 };
        //var array = ArrayHelper.GetUnsortedArray(6, 0, 20);
        Viewer.ShowArray(array);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        RadixSort.Sort(array);
        stopWatch.Stop();
        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");
        Viewer.ShowArray(array);
        Console.ReadKey();
    }

    static public void RunMergeInsertionSort()
    {
        var array = ArrayHelper.GetUnsortedArray(1000_000, 0, 10000);
        //int[] array = new[] { 9, 5, 4, 3, 9, 1, 6 };
        //var array = ArrayHelper.GetUnsortedArray(6, 0, 20);
        Viewer.ShowArray(array);
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        MergeInsertionSort.Sort(ref array);
        stopWatch.Stop();
        Console.WriteLine($"-------------------");
        Console.WriteLine($"Run time {stopWatch.Elapsed}");
        Viewer.ShowArray(array);
        Console.ReadKey();
    }
}

