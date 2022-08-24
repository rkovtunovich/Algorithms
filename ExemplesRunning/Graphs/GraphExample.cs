using Graphs;
using Graphs.Search;
using View;

namespace ExemplesRunning.Graphs;

internal class GraphExample
{
    private static readonly string _workingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    internal static void RunUndirectedExample()
    {
        var graph = GraphGenerator.GenerateNonOriented(10);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        //BFS.FindStronglyConnectedComponents(graph);
        //var connected = BFS.MarkPaths(graph, origin);
        var siplePathTree = BFS.GetSimpleShortestPathTree(graph, origin, out _);
        var fullPathTree = BFS.GetFullShortestPathTree(graph, origin, out _);

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

        BFS.CalculateBetweeness(graph);

        var dotSerializer = new DOTSerializer(graph);
        dotSerializer.AddImportantVertice(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Seralize();

        var dotFileName = $"{_workingDirectory}\\dot_undirected.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        DOTVisualizer.VisualizeDotString(dotFileName, "output_undirected.svg");

        DOTVisualizer.VisualizeGraph(siplePathTree);
        DOTVisualizer.VisualizeGraph(fullPathTree);

        var graphVarLength = GraphGenerator.GenerateUndirectedVariableEdgeLength(7);
        DijkstrasAlgorithm.Search(graphVarLength, graphVarLength.First());
        DOTVisualizer.VisualizeGraph(graphVarLength);
    }

    internal static void RunOrientedExample()
    {
        var graph = GraphGenerator.GenerateOrientedAcyclic( "oriended_acyclic", 6);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        //BFS.FindingConnectedComponents(graph);
        //var connected = BFS.MarkPaths(graph, origin);

        DFS.SortTopologicaly(graph);
        //DOTVisualizer.VisualizeGraph(graph);

        var dotSerializer = new DOTSerializer(graph);
        dotSerializer.AddImportantVertice(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Seralize();

        var dotFileName = $"{_workingDirectory}\\dot_oriented.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        DOTVisualizer.VisualizeDotString(dotFileName, "output_oriented.svg");

        var graph2 = GraphGenerator.GenerateOriented("Kasaraju",8);
        DOTVisualizer.VisualizeGraph(graph2);
        DFS.KosarajuSharirSearch(graph2);
        DOTVisualizer.VisualizeGraph(graph2);
    }
}
