using Graphs;
using Graphs.Coloring;
using Graphs.Generators;
using Graphs.GraphImplementation;
using Graphs.MinimumSpanningTree;
using Graphs.Model;
using Graphs.MWIS;
using Graphs.Search;
using View;

namespace ExamplesRunning.Graphs;

internal class GraphExample
{
    private static readonly string _workingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    internal static void RunUndirectedExample()
    {
        var graph = GraphGenerators.GenerateNonOriented(10);
        //var graph = CreateTestUndirectedGraph();
        DOTVisualizer.VisualizeGraph(graph);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        //BFS.FindStronglyConnectedComponents(graph);
        //var connected = BFS.MarkPaths(graph, origin);
        var simplePathTree = BFS.GetSimpleShortestPathTree(graph, origin, out _);
        DOTVisualizer.VisualizeGraph(simplePathTree);
        var fullPathTree = BFS.GetFullShortestPathTree(graph, origin, out _);

        var degreeDistributionsCount = graph.GetDedreeDistributionsCount();
        Viewer.ShowArray(degreeDistributionsCount);

        var degreeDistributionsFraction = graph.GetDedreeDistributionsFraction();
        Viewer.ShowArray(degreeDistributionsFraction);

        var degreeDistributionsCumulative = graph.GetDegreeDistributionsCumulative();
        Viewer.ShowArray(degreeDistributionsCumulative);

        var correlationCoefficient = graph.GetCorrelationCoefficient();
        Console.WriteLine($"correlation coefficient: {correlationCoefficient}");

        graph.CalculateLocalClusteringCoefficient();
        var clusterCoeff = graph.CalculateOverallClusteringCoefficient();
        Console.WriteLine($"clustering coefficient: {clusterCoeff}");

        BFS.CalculateBetweenness(graph);

        var dotSerializer = new DOTSerializer(graph);
        dotSerializer.AddImportantVertex(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Serialize();

        var dotFileName = $"{_workingDirectory}\\dot_undirected.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        DOTVisualizer.VisualizeDotString(dotFileName, "output_undirected.svg");

        DOTVisualizer.VisualizeGraph(simplePathTree);
        DOTVisualizer.VisualizeGraph(fullPathTree);

        var generator = new UndirectedVariableEdgeLengthGenerator(7, new(1)
        {
            Distance = int.MaxValue,
        });
        var graphVarLength = generator.Generate();
        //DijkstraAlgorithm.Search(graphVarLength, graphVarLength.First());
        DijkstraHeapAlgorithm.Search(graphVarLength, graphVarLength.First());
        DOTVisualizer.VisualizeGraph(graphVarLength);
    }

    internal static void RunOrientedExample()
    {
        var graph = GraphGenerators.GenerateOrientedAcyclic( "oriented_acyclic", 6);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        TopologicalOrdering.SortTopologically(graph);
        //DOTVisualizer.VisualizeGraph(graph);

        var dotSerializer = new DOTSerializer(graph);
        dotSerializer.AddImportantVertex(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Serialize();

        var dotFileName = $"{_workingDirectory}\\dot_oriented.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        DOTVisualizer.VisualizeDotString(dotFileName, "output_oriented.svg");

        var orientedGenerator = new OrientedGraphGenerator(8, 0.5);
        var graph2 = orientedGenerator.Generate("Kasaraju");
        DOTVisualizer.VisualizeGraph(graph2);
        DFS.KosarajuSharirSearch(graph2);
        DOTVisualizer.VisualizeGraph(graph2);

        var graphMaxFlow = GraphGenerators.GenerateOrientedFlow("Oriented_flow", 8);
        DOTVisualizer.VisualizeGraph(graphMaxFlow);
        BFS.AugmentingPathSearch(graphMaxFlow, graphMaxFlow.First(), graphMaxFlow.Last());       
    }

    internal static void RunMST()
    {
        var generator = new UndirectedVariableEdgeLengthGenerator(7, new(1));     
        var graph = generator.Generate("MST_graph_original");
        DOTVisualizer.VisualizeGraph(graph);

        var result = DJP.GetMST(graph);     
        DOTVisualizer.VisualizeGraph(result.tree);
        Console.WriteLine($"DJP total length: {result.length:0.00}");

        //graph = generator.Generate("Kruskal_graph_original");
        //DOTVisualizer.VisualizeGraph(graph);

        result = Kruskal.GetMST(graph as UndirectedVariableEdgeLengthGraph);
        DOTVisualizer.VisualizeGraph(result.tree);
        Console.WriteLine($"Kruskal total length: {result.length:0.00}");
    }

    internal static void RunMWIS() 
    {
        var generator = new PathGraphGenerator(5, 4);
        var graph = generator.Generate("Path_graph");

        DOTVisualizer.VisualizeGraph(graph);

        var MaxWeight = PathGraphMWISSearch.Find(graph);  
      
    }

    internal static void RunBellmanFord()
    {
        var generator = new OrientedVariableEdgeLengthGenerator(7, 1);
        var graph = generator.Generate("oriented_bellman_ford");
        DOTVisualizer.VisualizeGraph(graph);

        var result = BellmanFordAlgo.Search(graph as OrientedGraph, graph.First());

        Viewer.ShowMatrix(result);
    }

    internal static void RunFloydWarshall()
    {
        var generator = new OrientedVariableEdgeLengthGenerator(7, 1);
        var graph = generator.Generate("oriented_floyd_warshall");
        DOTVisualizer.VisualizeGraph(graph);

        FloydWarshallAlgo.Search(graph as OrientedGraph);

        //MatrixHelper.Show(result);
    }

    internal static void RunWelshPowell()
    {
        var graph = GraphGenerators.GenerateNonOriented(10);

        WelshPowell.Colorize(graph);

        DOTVisualizer.VisualizeGraph(graph);
    }

    internal static void RunColorCoding()
    {
        var generator = new UndirectedVariableEdgeLengthGenerator(6, new(1));
        var graph = generator.Generate("undirected_color_coding");

        var result = ColorCoding.FindMinimumLengthColorfulPath(graph, 3, 0.1);

        DOTVisualizer.VisualizeGraph(graph);
    }

    internal static void RunCycleSearching()
    {
        var graph = GraphGenerators.GenerateNonOriented(10);
        DOTVisualizer.VisualizeGraph(graph);

        var result = DFS.SearchCycle(graph);

        foreach (var vertex in result)
        {
            Console.WriteLine(vertex);
        }
    }

    private static UndirectedGraph CreateTestUndirectedGraph()
    {
        var graph = new UndirectedGraph("test_undirected");

        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        var v5 = new Vertex(5);
        var v6 = new Vertex(6);
        var v7 = new Vertex(7);
        var v8 = new Vertex(8);
        var v9 = new Vertex(9);
        var v10 = new Vertex(10);

        graph.AddVertex(v1);
        graph.AddVertex(v2);
        graph.AddVertex(v3);
        graph.AddVertex(v4);
        graph.AddVertex(v5);
        graph.AddVertex(v6);
        graph.AddVertex(v7);
        graph.AddVertex(v8);
        graph.AddVertex(v9);
        graph.AddVertex(v10);

        graph.AddEdge(v1, v2);
        graph.AddEdge(v1, v5);
        graph.AddEdge(v2, v6);
        graph.AddEdge(v2, v10);
        graph.AddEdge(v2, v8);
        graph.AddEdge(v3, v10);
        graph.AddEdge(v3, v6);
        graph.AddEdge(v5, v8);
        graph.AddEdge(v7, v8);
        graph.AddEdge(v7, v6);
        graph.AddEdge(v8, v9);

        return graph;
    }

}

