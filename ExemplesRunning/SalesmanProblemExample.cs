using Graphs.Core;
using Graphs.Core.Generators;
using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;
using SalesmanProblem;

namespace ExamplesRunning;

internal static class SalesmanProblemExample
{
    public static void Run2Opt()
    {
        var generator = new UndirectedFullyConnectedGraphGenerator(5);
        var graph = generator.Generate("salesman_full");

        DOTVisualizer.VisualizeGraph(graph);

        var greedyTour = GreedyTour.Build(graph);

        Viewer.ShowArray(greedyTour.ToArray());

        TwoOpt.Optimize(greedyTour, graph);

        Viewer.ShowArray(greedyTour.ToArray());
    }

    public static void RunBellmanHeldKarp()
    {
        int[,] dist = new int[,]
           {
                {0, 20, 30, 10},
                {20, 0, 15, 5},
                {30, 15, 0, 25},
                {10, 5, 25, 0}
           };

        var shortestPathLength = BellmanHeldKarp.TSP(dist, 0, 1);

        Console.WriteLine($"int array: Shortest path length: {shortestPathLength}");

        var A = new Vertex(1)
        {
            Label = "A"
        };
        var B = new Vertex(2)
        {
            Label = "B"
        };
        var C = new Vertex(3)
        {
            Label = "C"
        };
        var D = new Vertex(4)
        {
            Label = "D"
        };

        var graph = new UndirectedVariableEdgeLengthGraph("BellmanHeldKarp");

        graph.AddVertex(A);
        graph.AddVertex(B);
        graph.AddVertex(C);
        graph.AddVertex(D);

        graph.AddEdge(A, B, 20);
        graph.AddEdge(A, C, 30);
        graph.AddEdge(A, D, 10);
        graph.AddEdge(B, C, 15);
        graph.AddEdge(B, D, 5);
        graph.AddEdge(C, D, 25);

        shortestPathLength = BellmanHeldKarp.TSP(graph, A);
        Console.WriteLine($"Shortest path length: {shortestPathLength}");
    }
}

