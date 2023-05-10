using DataStructures.Heaps;
using Helpers;
using Sorting;
using System.Diagnostics;
using View;

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
        QuickSort.Sort(array);
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

        var stopWatch = new Stopwatch();

        //var heap = new Heap<int>();

        //for (int i = 0; i < array.Length; i++)
        //{
        //    heap.Insert(array[i]);
        //}
        //var generator = new GraphByHeapGenerator<int>(heap);
        //var graph = generator.Generate();
        //DOTVisualizer.VisualizeGraph(graph);

        //for (int i = 0; i < array.Length; i++)
        //{
        //    array[i] = heap.ExtractMimimum();
        //    graph = generator.Generate();
        //    DOTVisualizer.VisualizeGraph(graph);
        //}

        //stopWatch.Start();
        HeapSort<int, int>.Sort(array, new HeapMin<int, int>());
        //stopWatch.Stop();

        //Console.WriteLine($"-------------------");
        //Console.WriteLine($"Run time {stopWatch.Elapsed}");

        Viewer.ShowArray(array);

        HeapSort<int, int>.Sort(array, new HeapMax<int, int>());
        //stopWatch.Stop();

        //Console.WriteLine($"-------------------");
        //Console.WriteLine($"Run time {stopWatch.Elapsed}");

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
        DirectInsertionSort.Sort(array);
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
}

