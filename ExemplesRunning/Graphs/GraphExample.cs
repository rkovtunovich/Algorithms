using Graphs;
using Graphs.Search;
using View;

namespace ExemplesRunning.Graphs;

internal class GraphExample
{
    private static readonly string _workingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    internal static void RunUndirectedExample()
    {
        var graph = GraphGenerator<double>.GenerateNonOrientedOneComponent(5);

        var origin = graph.First();
        //var connected = BFS<int>.SearchConnected(graph, origin);
        var connected = DFS<double>.SearchConnectedRec(graph, origin);
        //BFS<int>.FindingConnectedComponents(graph);
        //var connected = BFS<int>.MarkPaths(graph, origin);
        var siplePathTree = BFS<double>.GetSimpleShortestPathTree(graph, origin, out _);
        var fullPathTree = BFS<double>.GetFullShortestPathTree(graph, origin, out _);

        var deegreDistributionsCount = graph.GetDedreeDistributionsCount();
        Viewer.ShowArray(deegreDistributionsCount);

        var deegreDistributionsFraction = graph.GetDedreeDistributionsFraction();
        Viewer.ShowArray(deegreDistributionsFraction);

        var deegreDistributionsCumulative = graph.GetDegreeDistributionsCumulative();
        Viewer.ShowArray(deegreDistributionsCumulative);

        var correlationCoefficient = graph.GetCorrelationCoefficient();
        Console.WriteLine($"correlation coefficient: {correlationCoefficient}");

        graph.CalculateLocalClusteringCoefficient();
        var clustCoeff = graph.CalculateOverallClusteringCoefficient();
        Console.WriteLine($"clustering coefficient: {clustCoeff}");

        BFS<double>.CalculateBetweeness(graph);
        //BFS<int>.CalculateBetweenessSimple(graph);

        var dotSerializer = new DOTSerializer<double>(graph);
        dotSerializer.AddImportantVertice(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Seralize();

        var dotFileName = $"{_workingDirectory}\\dot_undirected.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        DOTVisualizer.VisualizeDotString(dotFileName, "output_undirected.svg");

        DOTVisualizer.VisualizeGraph(siplePathTree);
        DOTVisualizer.VisualizeGraph(fullPathTree);
    }

    internal static void RunOrientedExample()
    {
        var graph = GraphGenerator<int>.GenerateOrientedAcyclic(5);

        var origin = graph.First();
        //var connected = BFS<int>.SearchConnected(graph, origin);
        var connected = DFS<int>.SearchConnectedRec(graph, origin);
        //BFS<int>.FindingConnectedComponents(graph);
        //var connected = BFS<int>.MarkPaths(graph, origin);

        DFS<int>.GetTopologicalOrdering(graph);

        var dotSerializer = new DOTSerializer<int>(graph);
        dotSerializer.AddImportantVertice(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Seralize();

        var dotFileName = $"{_workingDirectory}\\dot_oriented.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        DOTVisualizer.VisualizeDotString(dotFileName,"output_oriented.svg");
    }
}
