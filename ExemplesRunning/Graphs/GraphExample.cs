﻿using Graphs;
using Graphs.Generators;
using Graphs.GraphImplementation;
using Graphs.MinimumSpanningTree;
using Graphs.MWIS;
using Graphs.Search;
using Helpers;
using View;

namespace ExamplesRunning.Graphs;

internal class GraphExample
{
    private static readonly string _workingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    internal static void RunUndirectedExample()
    {
        var graph = GraphGenerators.GenerateNonOriented(10);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        //BFS.FindStronglyConnectedComponents(graph);
        //var connected = BFS.MarkPaths(graph, origin);
        var simplePathTree = BFS.GetSimpleShortestPathTree(graph, origin, out _);
        var fullPathTree = BFS.GetFullShortestPathTree(graph, origin, out _);

        var degreeDistributionsCount = graph.GetDedreeDistributionsCount();
        Viewer.ShowArray(degreeDistributionsCount);

        var degreeDistributionsFraction = graph.GetDedreeDistributionsFraction();
        Viewer.ShowArray(degreeDistributionsFraction);

        var deegreDistributionsCumulative = graph.GetDegreeDistributionsCumulative();
        Viewer.ShowArray(deegreDistributionsCumulative);

        var correlationCoefficient = graph.GetCorrelationCoefficient();
        Console.WriteLine($"correlation coefficient: {correlationCoefficient}");

        graph.CalculateLocalClusteringCoefficient();
        var clusterCoeff = graph.CalculateOverallClusteringCoefficient();
        Console.WriteLine($"clustering coefficient: {clusterCoeff}");

        BFS.CalculateBetweeness(graph);

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
        //DijkstrasAlgorithm.Search(graphVarLength, graphVarLength.First());
        DijkstrasHeapAlgorithm.Search(graphVarLength, graphVarLength.First());
        DOTVisualizer.VisualizeGraph(graphVarLength);
    }

    internal static void RunOrientedExample()
    {
        var graph = GraphGenerators.GenerateOrientedAcyclic( "oriented_acyclic", 6);

        var origin = graph.First();
        //var connected = BFS.SearchConnected(graph, origin);
        var connected = DFS.SearchConnectedRec(graph, origin);
        //BFS.FindingConnectedComponents(graph);
        //var connected = BFS.MarkPaths(graph, origin);

        DFS.SortTopologicaly(graph);
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
        Console.WriteLine($"DJP total lenth: {result.length:0.00}");

        //graph = generator.Generate("Kraskal_graph_original");
        //DOTVisualizer.VisualizeGraph(graph);

        result = Kruskal.GetMST(graph as UndirectedVariableEdgeLengthGraph);
        DOTVisualizer.VisualizeGraph(result.tree);
        Console.WriteLine($"Kraskal total length: {result.length:0.00}");
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

        var result = BelmanFordAlgo.Search(graph as OrientedGraph, graph.First());

        MatrixHelper.Show(result);
    }

    internal static void RunFloydWarshall()
    {
        var generator = new OrientedVariableEdgeLengthGenerator(7, 1);
        var graph = generator.Generate("oriented_floyd_warshall");
        DOTVisualizer.VisualizeGraph(graph);

        FloydWarshallAlgo.Search(graph as OrientedGraph);

        //MatrixHelper.Show(result);
    }
}

