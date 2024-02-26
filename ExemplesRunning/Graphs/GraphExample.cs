using DataStructures.Lists;
using Graphs.Core;
using Graphs.Core.Coloring;
using Graphs.Core.GraphImplementation;
using Graphs.Core.MinimumArborescencesTree;
using Graphs.Core.MinimumSpanningTree;
using Graphs.Core.MWIS;
using Graphs.Core.Search;
using Graphs.Core.Generators;
using Graphs.Core.Model;
using View;
using Models.Scheduling;

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
        //var generator = new UndirectedVariableEdgeLengthGenerator(7);
        //var graph = generator.Generate("Dijkstra_heap_graph");

        var orientedGenerator = new OrientedVariableEdgeLengthGenerator(8, 0.575, trackIncomeEdges: true);
        if (orientedGenerator.Generate("Dijkstra_heap_graph") is not OrientedGraph graph)
            return;

        DOTVisualizer.VisualizeGraph(graph);

        var source = graph.GetSource() ?? graph.First();
        var sink = graph.GetSink() ?? graph.Last();
        var path = DijkstraHeapAlgorithm.Search(graph, source, sink);

        DOTVisualizer.VisualizeGraph(graph);

        foreach (var vertex in path)
        {
            Console.WriteLine(vertex.Index);
        }
    }

    internal static void RunEdmondsAlgorithm()
    {
        //var path = $"{DOTVisualizer.WorkingDirectory}\\serializations\\Edmonds_algorithm_oriented.txt";
        //var serializedGraph = GraphFileManager.ReadFromFile(path);
        //var serializer = new DOTSerializer();
        //var graph = serializer.Deserialize(serializedGraph) as OrientedGraph;
        //graph?.FillIncomeEdges(true);

        var orientedGenerator = new OrientedVariableEdgeLengthGenerator(8, 0.575);
        var graph = orientedGenerator.Generate("Edmonds_algorithm_oriented") as OrientedGraph;
        graph?.FillIncomeEdges(true);

        DOTVisualizer.VisualizeGraph(graph);

        (var tree, var cost) = EdmondsAlgorithm.FindArborescencesTree(graph);

        DOTVisualizer.VisualizeGraph(tree);

        Console.WriteLine($"Minimum cost: {cost}");
    }

    internal static void RunTimeDependentShortestPathProblem()
    {
        var orientedGenerator = new OrientedVariableEdgeLengthGenerator(8, 0.575);
        var graph = orientedGenerator.Generate("Time_dependent_shortest_path_problem") as OrientedGraph;
        graph?.FillIncomeEdges(true);

        //var path = $"{DOTVisualizer.WorkingDirectory}\\serializations\\Time_dependent_shortest_path_problem.txt";
        //var serializedGraph = GraphFileManager.ReadFromFile(path);
        //var serializer = new DOTSerializer();
        //var graph = serializer.Deserialize(serializedGraph) as OrientedGraph;
        //graph?.FillIncomeEdges(true);

        DOTVisualizer.VisualizeGraph(graph);

        var source = graph.GetSource() ?? graph.First();
        var sink = graph.GetSink() ?? graph.Last();
        var edges = graph.GetAllEdges();
        var timeConditions = new Dictionary<(Vertex, Vertex), Dictionary<DailyInterval, int>>();
        foreach (var edge in edges)
        {
            var conditionsCount = new Random().Next(0, 4);

            var start = 0;
            for (var i = 0; i < conditionsCount; i++)
            {
                var end = new Random().Next(start, 25);
                var interval = new DailyInterval(start, end);
                var weight = new Random().Next(1, 10);
                if (!timeConditions.ContainsKey(edge))
                    timeConditions[edge] = [];

                timeConditions[edge][interval] = weight;

                start = end;

                if(start >= DailyInterval.HoursInDay)
                    break;
            }
        }

        var shortestPath = TimeDependentShortestPathProblem.Search(graph, source, sink, 0, timeConditions);

        DOTVisualizer.VisualizeGraph(graph);

        foreach (var vertex in shortestPath)
        {
            Console.WriteLine(vertex.Index);
        }
    }
}

