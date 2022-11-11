using DataStructures.Heaps;
using DataStructures.SearchTrees;
using Graphs;
using Graphs.Generators;
using Helpers;
using OpenTK.Graphics.OpenGL;

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
        int random = new Random().Next(0, 8);
        var array = ArrayHelper.GetUnsortedArray(8, 1, 30);


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
}
