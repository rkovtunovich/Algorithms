using DataStructures.Heaps;
using DataStructures.SearchTrees;
using Graphs;
using Graphs.Generators;
using Helpers;

namespace ExemplesRunning;

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

        Console.WriteLine($"Mimimum: {tree.Mimimum?.Value}");
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
        var array = new List<int>() { 4, 22, 7, 42, 49, 35, 32, 28, 14, 49, 25, 15, 44, 23, 37};

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
        
        var toRemove = new List<int>() { 4, 23, 25, 28, 32, 22, 7};
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
}
