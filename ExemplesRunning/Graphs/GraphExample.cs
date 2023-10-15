using DataStructures.Lists;
using Graphs;
using Graphs.Coloring;
using Graphs.Generators;
using Graphs.GraphImplementation;
using Graphs.MinimumArborescencesTree;
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
        //var connected = DFS.SearchConnectedRec(graph, origin);
        //BFS.FindStronglyConnectedComponents(graph);
        var connected = BFS.MarkPaths(graph, origin);
        DOTVisualizer.VisualizeGraph(graph);
        var simplePathTree = BFS.GetSimpleShortestPathTree(graph, origin, out _);
        DOTVisualizer.VisualizeGraph(simplePathTree);
        var fullPathTree = BFS.GetFullShortestPathTree(graph, origin, out _);
        DOTVisualizer.VisualizeGraph(fullPathTree);

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
        DOTVisualizer.VisualizeGraph(graph, origin, connected);

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
        var graph = GraphGenerators.GenerateOrientedAcyclic("oriented_acyclic", 6);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        DOTVisualizer.VisualizeGraph(graph, origin, connected);

        var orientedGenerator = new OrientedGraphGenerator(8, 0.5);
        var graph2 = orientedGenerator.Generate("Kasaraju");
        DOTVisualizer.VisualizeGraph(graph2);
        DFS.KosarajuSharirSearch(graph2);
        DOTVisualizer.VisualizeGraph(graph2);

        //var graphMaxFlow = GraphGenerators.GenerateOrientedFlow("Oriented_flow", 8);
        //DOTVisualizer.VisualizeGraph(graphMaxFlow);
        //var patLength = BFS.AugmentingPathSearch(graphMaxFlow, graphMaxFlow.First(), graphMaxFlow.Last());       
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
        var generator = new OrientedVariableEdgeLengthGenerator(7, 0.5, 1);
        var graph = generator.Generate("oriented_bellman_ford");
        DOTVisualizer.VisualizeGraph(graph);

        var result = BellmanFordAlgo.Search(graph as OrientedGraph, graph.First());

        Viewer.ShowMatrix(result);
    }

    internal static void RunFloydWarshall()
    {
        var generator = new OrientedVariableEdgeLengthGenerator(7, 0.5, 1);
        var graph = generator.Generate("oriented_floyd_warshall");
        DOTVisualizer.VisualizeGraph(graph);

        FloydWarshallAlgo.Search(graph as OrientedGraph);
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

    internal static void RunTopologicalOrdering()
    {
        var graph = GraphGenerators.GenerateOrientedAcyclic("oriented_acyclic", 6);
        DOTVisualizer.VisualizeGraph(graph);

        var result = TopologicalOrdering.SortTopologically(graph);

        foreach (var vertex in result)
        {
            Console.WriteLine(vertex);
        }
    }

    internal static void RunTopologicalOrderingOrCycleSearching()
    {
        var orientedGenerator = new OrientedGraphGenerator(8, saturation: 0.55);
        var graph = orientedGenerator.Generate("oriented");
        DOTVisualizer.VisualizeGraph(graph);

        var (isCycle, vertices) = TopologicalOrdering.TrySortTopologicallyOrGetCycle(graph);

        if (isCycle)
            Console.WriteLine("The graph contains a cycle:");
        else
            Console.WriteLine("The graph is acyclic and sorted topologically:");

        foreach (var vertex in vertices)
        {
            Console.WriteLine(vertex);
        }
    }

    internal static void RunStatementsConsistency()
    {
        var terms = new SequentialList<int>
        {
            1, 2, 3, 4, 5
        };

        var statements = new Dictionary<(int, int), string>
        {
            {(1, 2), "same"},
            {(1, 3), "same"},
            {(2, 3), "same"},
            {(2, 5), "different"},
            {(4, 5), "same"},
            //{(5, 1), "same" }
            {(5, 1), "different" }
        };

        var result = StatementsConsistency.IsConsistent(statements, terms.Count);

        Console.WriteLine(result);
    }

    internal static void RunFordFulkerson()
    {
        var graph = GraphGenerators.GenerateOrientedFlow("oriented_flow", 8);
        DOTVisualizer.VisualizeGraph(graph);

        var result = FordFulkersonMaxFlow.AugmentingPathSearch(graph, graph.First(), graph.Last());

        Console.WriteLine(result);
    }

    internal static void RunDijkstra()
    {
        var generator = new UndirectedVariableEdgeLengthGenerator(7, new(1));
        var graph = generator.Generate("Dijkstra_graph");

        DijkstraAlgorithm.Search(graph, graph.First());

        DOTVisualizer.VisualizeGraph(graph);
    }

    internal static void RunDijkstraHeap()
    {
        var generator = new UndirectedVariableEdgeLengthGenerator(7, new(1));
        var graph = generator.Generate("Dijkstra_heap_graph");

        DijkstraHeapAlgorithm.Search(graph, graph.First());

        DOTVisualizer.VisualizeGraph(graph);
    }

    internal static void RunEdmondsAlgorithm()
    {
        var serializedGraph = DOTVisualizer.ReadFromFile("Edmonds_algorithm_oriented");
        var serializer = new DOTSerializer();
        var graph = serializer.Deserialize(serializedGraph) as OrientedGraph;

        //var orientedGenerator = new OrientedVariableEdgeLengthGenerator(8, 0.575);
        //var graph = orientedGenerator.Generate("Edmonds_algorithm_oriented") as OrientedGraph;

        //DOTVisualizer.VisualizeGraph(graph);

        (var tree, var cost) = EdmondsAlgorithm.FindArborescencesTree(graph);

        DOTVisualizer.VisualizeGraph(tree);

        Console.WriteLine($"Minimum cost: {cost}");
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

