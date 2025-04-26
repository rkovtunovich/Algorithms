using DataStructures.HashTables.OpenAddressing;
using DataStructures.Heaps;
using DataStructures.Trees.BinaryTrees.Search;
using DataStructures.Trees.BinaryTrees.Search.AVL;
using DataStructures.Trees.BinaryTrees.Search.RedBlack;
using DataStructures.Trees.BTrees;
using Graphs.Core;
using Graphs.Core.Generators;
using Helpers;

namespace ExamplesRunning;

public static class DataStructuresExample
{
    public static void RunHeapExample()
    {
        var keys = new List<int>() { 4, 9, 4, 13, 12, 8, 11, 9, 4 };
        var heap = new HeapMin<int, int>();

        foreach (var key in keys)
        {
            heap.Insert(key);
        }

        var generator = new GraphByHeapGenerator<int, int>(heap);
        var graph = generator.Generate();
        DOTVisualizer.VisualizeGraph(graph);

        int min = heap.ExtractKey();

        graph = generator.Generate();
        DOTVisualizer.VisualizeGraph(graph);

        int max = heap.ExtractKey();

        graph = generator.Generate();
        DOTVisualizer.VisualizeGraph(graph);
    }

    public static void RunSearchTreeExample()
    {
        int random = new Random().Next(0, 10);
        var array = ArrayHelper.GetUnsortedArray(10, 1, 40);


        //var keys = new List<int>() { 2, 1, 3, 4, 5 };
        var tree = new SearchTree<int, int>();

        foreach (var key in array)
        {
            tree.Insert(key, key);
        }

        var generator = new GraphSearchTreeGenerator<int, int>(tree);
        var graph = generator.Generate("search_tree");
        DOTVisualizer.VisualizeGraph(graph);

        Console.WriteLine($"Minimum: {tree.Minimum?.Value}");
        Console.WriteLine($"Maximum: {tree.Maximum?.Value}");
        Console.WriteLine($"Predecessor: {tree.GetPredecessor(5)?.Value}");
        Console.WriteLine($"Successor: {tree.GetSuccessor(4)?.Value}");

        tree.TraverseInOrder(node => Console.WriteLine(node.Key.ToString()));

        Console.WriteLine($"Remove with key: {array[random]}");
        tree.Remove(array[random]);

        graph = generator.Generate("search_tree");
        DOTVisualizer.VisualizeGraph(graph);

        Console.ReadLine();
    }

    public static void RunAVLTreeExample()
    {
        //int random = new Random().Next(0, 15);
        int random = 2;
        //var array = ArrayHelper.GetUnsortedArray(15, 1, 50);

        //var array = new List<int>() { 7, 30, 32, 24, 14, 39, 7, 23, 31, 9 };
        var array = new List<int>() { 4, 22, 7, 42, 49, 35, 32, 28, 14, 49, 25, 15, 44, 23, 37 };

        //var array = new List<int>() { 4, 22, 7, 42, 37, 7, 37, 22, 14, 7, 25, 4, 44, 23, 37 };

        var tree = new AVLTree<int, int>();

        var generator = new GraphByAVLTreeGenerator<int, int>(tree);
        var graph = generator.Generate("search_tree");

        int number = 0;
        foreach (var key in array)
        {
            number++;
            tree.Insert(key, key);

            graph = generator.Generate($"search_tree_{number}");
            DOTVisualizer.VisualizeGraph(graph);
        }

        graph = generator.Generate("search_tree");
        DOTVisualizer.VisualizeGraph(graph);

        tree.TraverseInOrder(node => Console.WriteLine(node.Key.ToString()));

        Console.WriteLine($"Remove with key: {array[random]}");

        var toRemove = new List<int>() { 4, 23, 25, 28, 32, 22, 7 };
        foreach (var key in toRemove)
        {
            tree.Remove(key);

            graph = generator.Generate($"search_tree");
            DOTVisualizer.VisualizeGraph(graph);
        }

        graph = generator.Generate("search_tree");
        DOTVisualizer.VisualizeGraph(graph);

        Console.ReadLine();
    }

    public static void RunRedBlackTreeExample()
    {
        //int random = new Random().Next(0, 15);
        int random = 2;
        //var array = ArrayHelper.GetUnsortedArray(15, 1, 50);

        //var array = new List<int>() { 7, 30, 32, 24, 14, 39, 7, 23, 31, 9 };
        var array = new List<int>() { 4, 22, 7, 42, 49, 35, 32, 28, 14, 49, 25, 15, 44, 23, 37 };

        //var array = new List<int>() { 4, 22, 7, 42, 37, 7, 37, 22, 14, 7, 25, 4, 44, 23, 37 };

        var tree = new RedBlackTree<int, int>();

        var generator = new GraphRedBlackTreeGenerator<int, int>(tree);
        var graph = generator.Generate("red_black_tree");

        int number = 0;
        foreach (var key in array)
        {
            number++;
            tree.Insert(key, key);

            graph = generator.Generate($"red_black_tree_{number}");
            DOTVisualizer.VisualizeGraph(graph);
        }

        graph = generator.Generate("red_black_tree");
        DOTVisualizer.VisualizeGraph(graph);

        tree.TraverseInOrder(node => Console.WriteLine(node.Key.ToString()));

        //tree.Remove(array[3]);

        //Console.WriteLine($"Remove with key: {array[random]}");

        var toRemove = new List<int>() { 4, 23, 25, 28, 32, 22, 7 };
        foreach (var key in toRemove)
        {
            tree.Remove(key);

            graph = generator.Generate($"red_black_tree");
            DOTVisualizer.VisualizeGraph(graph);
        }

        graph = generator.Generate("red_black_tree");
        DOTVisualizer.VisualizeGraph(graph);

        Console.ReadLine();
    }

    public static void RunHashSetExample()
    {
        var set = new SimpleHashSet<string>();
        set.Add("test");
        set.Add("test2");
        set.Add("test");
        set.Add("test3");
        set.Add("1");
        set.Add("2");
        set.Add("3");
        set.Add("4");
        set.Add("5");
        set.Add("6");
        set.Add("7");
        set.Add("8");

        Console.WriteLine(set.Contains("test3"));

        set.Remove("test2");
        set.Remove("test");

        Console.WriteLine(set.Contains("test2"));
    }

    public static void RunOBSTExample()
    {
        var keys = new int[] { 1, 2, 3, 4, 5 };
        var frequency = new double[] { 2, 4, 1, 5, 1 };

        var obst = new OptimalBinarySearchTree<int, int>(keys, keys, frequency);

        Viewer.ShowMatrix(obst.Span);
    }

    public static void RunTwoTheeTreeExample()
    {
        //var keys = new int[] { 50, 30, 10, 70, 60};
        //var keys = new int[] { 2, 5, 6, 9, 4, 10, 1 };
        //var keys = new int[] { 54, 36, 69, 90, 18, 27, 45, 63, 72, 81, 99 };
        var keys = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 5, 15, 25, 8 };

        var tree = new TwoThreeTree<int>();

        var generator = new GraphByTwoTheeTreeGenerator<int>(tree);
        var graph = generator.Generate("two_three_tree");

        for (int i = 0; i < keys.Length; i++)
        {
            tree.Insert(keys[i]);

            graph = generator.Generate($"two_three_tree_{i + 1}");
            DOTVisualizer.VisualizeGraph(graph);
        }

        graph = generator.Generate("two_three_tree");
        DOTVisualizer.VisualizeGraph(graph);

        var keysForDeleting = new int[] { 5, 8, 10, 30, 15 };


        for (int i = 0; i < keysForDeleting.Length; i++)
        {
            tree.Remove(keysForDeleting[i]);

            graph = generator.Generate($"two_three_tree");
            DOTVisualizer.VisualizeGraph(graph);
        }
    }

    public static void RunBPlusTheeTreeExample()
    {
        //var keys = new int[] { 50, 30, 10, 70, 60};
        //var keys = new int[] { 2, 5, 6, 9, 4, 10, 1 };
        //var keys = new int[] { 54, 36, 69, 90, 18, 27, 45, 63, 72, 81, 99 };
        var keys = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 5, 15, 25, 8 };

        var tree = new BPlusTree<int, int>(3);

        var generator = new GraphByBPLusTheeTreeGenerator<int, int>(tree);
        var graph = generator.Generate("B_Plus_three_tree");

        for (int i = 0; i < keys.Length; i++)
        {
            tree.Insert(keys[i]);

            graph = generator.Generate($"B_Plus_three_tree_{i + 1}");
            DOTVisualizer.VisualizeGraph(graph);
        }

        graph = generator.Generate("B_Plus_three_tree");
        DOTVisualizer.VisualizeGraph(graph);

        var keysForDeleting = new int[] { 5, 8, 10, 30, 15 };

        for (int i = 0; i < keysForDeleting.Length; i++)
        {
            tree.Remove(keysForDeleting[i]);

            graph = generator.Generate($"B_Plus_three_tree");
            DOTVisualizer.VisualizeGraph(graph);
        }
    }
}
