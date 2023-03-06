using Graphs;
using Graphs.Generators;
using SalesmanProblem;
using View;

namespace ExamplesRunning;

internal static class SalesmanProblemExample
{
    public static void Run()
    {
        var generator = new UndirectedFullyConnectedGraphGenerator(5);
        var graph = generator.Generate("salesman_full");

        DOTVisualizer.VisualizeGraph(graph);

        var greedyTour = GreedyTour.Build(graph);

        Viewer.ShowArray(greedyTour.ToArray());

        TwoOpt.Optimize(greedyTour, graph);

        Viewer.ShowArray(greedyTour.ToArray());
    }
}

