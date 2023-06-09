using DataStructures.BinaryTrees.Search;
using DataStructures.BinaryTrees.Search.AVL;
using DataStructures.BinaryTrees.Search.RedBlack;
using DataStructures.HashTables;
using DataStructures.Heaps;
using Graphs;
using Graphs.Generators;
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

        Console.WriteLine($"Mimimum: {tree.Minimum?.Value}");
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

        var generator = new GraphAVLTreeGenerator<int, int>(tree);
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

        tree.Remove(array[3]);

        //Console.WriteLine($"Remove with key: {array[random]}");

        //var toRemove = new List<int>() { 4, 23, 25, 28, 32, 22, 7 };
        //foreach (var key in toRemove)
        //{
        //    tree.Remove(key);

        //    graph = generator.Generate($"red_black_tree");
        //    DOTVisualizer.VisualizeGraph(graph);
        //}

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
}
