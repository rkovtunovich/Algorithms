using Graphs.Core.GraphImplementation;
using Graphs.Core.Model;

namespace Graphs.Core;

public static class GraphGenerators
{
    private static readonly Random _random = new();

    #region NonOriented

    public static UndirectedGraph GenerateNonOriented(int countVertices)
    {
        var graph = new UndirectedGraph("undirected");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var vertex in graph)
        {
            GenerateMutualConnections(graph, vertex, 0, graph.Count() / 2);
        }

        return graph;
    }

    public static UndirectedGraph GenerateNonOrientedOneComponent(int countVertices)
    {
        var graph = new UndirectedGraph("undirected");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var vertex in graph)
        {
            GenerateMutualConnections(graph, vertex, 1, graph.Count() / 2);
        }

        return graph;
    }

    #region Service methods

    private static void GenerateMutualConnections(UndirectedGraph graph, Vertex owner, int minConnections, int maxConnections)
    {
        int numberConnections = _random.Next(minConnections, maxConnections);

        // for potential non one component graphs 
        if (minConnections == 0)
            numberConnections -= graph.GetDegree(owner);

        var alreadyAdded = new HashSet<Vertex>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, graph.Count() + 1);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVertexByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertex with index {newIndex}");

            if (alreadyAdded.Contains<Vertex>(newConnection))
                continue;

            if (graph.IsConnected(newConnection, owner))
                continue;

            numberConnections--;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }

    #endregion

    #endregion

    #region Oriented

    public static OrientedGraph GenerateOrientedAcyclic(string name, int countVertices)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertex(new(i));
        }

        var haveOutgoings = new HashSet<Vertex>();

        foreach (var vertex in graph)
        {
            GenerateDirectedAcyclicConnections(graph, countVertices, vertex, haveOutgoings);
        }

        return graph;
    }

    public static OrientedGraph GenerateOrientedFlow(string name, int countVertices)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var vertex in graph)
        {
            GenerateDirectedConnections(graph, countVertices, vertex);
        }

        var random = new Random();

        foreach (var vertex in graph)
        {
            var edges = graph.GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                int length = random.Next(1, 10);
                graph.SetEdgeLength(vertex, edge, length);
            }
        }

        return graph;
    }

    private static void GenerateDirectedConnections(OrientedGraph graph, int countVertices, Vertex owner)
    {
        int numberConnections = _random.Next(0, countVertices / 2);

        var alreadyAdded = new HashSet<Vertex>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVertexByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertex with index {newIndex}");

            if (alreadyAdded.Contains<Vertex>(newConnection))
                continue;

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }

    private static void GenerateDirectedAcyclicConnections(OrientedGraph graph, int countVertices, Vertex owner, HashSet<Vertex> haveOutgoins)
    {
        int numberConnections = _random.Next(1, (countVertices - 1) / 2);

        var alreadyAdded = new HashSet<Vertex>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVertexByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertex with index {newIndex}");

            if (haveOutgoins.Contains<Vertex>(newConnection))
                continue;

            if (alreadyAdded.Contains<Vertex>(newConnection))
                continue;

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }

    #endregion
}
