using DataStructures;
using Graphs;
using Graphs.Generators;

namespace ExemplesRunning;

public static class DataStructuresExample
{
    public static void RunHeapExample()
    {
        var keys = new List<int>() { 4, 9, 4, 13, 12, 8, 11, 9, 4};
        var heap = new Heap<int>();

        foreach (var key in keys)
        {
            heap.Insert(key);
        }

        var generator = new GraphByHeapGenerator<int>(heap);
        var graph = generator.Generate();
        DOTVisualizer.VisualizeGraph(graph);

        int min = heap.ExtractMimimum();

        graph = generator.Generate();
        DOTVisualizer.VisualizeGraph(graph);

        int max = heap.ExtractMimimum();

        graph = generator.Generate();
        DOTVisualizer.VisualizeGraph(graph);
    }
}
