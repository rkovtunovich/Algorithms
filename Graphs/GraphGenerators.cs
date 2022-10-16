using Graphs.GraphImplementation;

namespace Graphs;

public static class GraphGenerators
{
    private static readonly Random _random = new();

    #region NonOriented

    public static UndirectedGraph GenerateNonOriented(int countVertices)
    {
        var graph = new UndirectedGraph("undirected");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateMutualConnections(graph, vertice, 0, graph.Count() / 2);
        }

        return graph;
    }

    public static UndirectedGraph GenerateNonOrientedOneComponent(int countVertices)
    {
        var graph = new UndirectedGraph("undirected");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateMutualConnections(graph, vertice, 1, graph.Count() / 2);
        }

        return graph;
    }

    public static UndirectedGraph GenerateUndirectedVariableEdgeLength(int countVertices)
    {
        var graph = new UndirectedVariableEdgeLengthGraph("undirected_var_length");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateMutualConnections(graph, vertice, 0, graph.Count() / 2);
        }

        var random = new Random();

        foreach (var vertice in graph)
        {
            var edges = graph.GetEdges(vertice);

            foreach (var edge in edges)
            {
                double leangth = Math.Round(random.NextDouble() * 10, 2);
                graph.SetEdgeLength(vertice, edge, leangth);
                graph.SetEdgeLength(edge, vertice, leangth);
            }
        }

        return graph;
    }

    #region Service methods

    private static void GenerateMutualConnections(UndirectedGraph graph, Vertice owner, int minConnections, int maxConnections)
    {
        int numberConnections = _random.Next(minConnections, maxConnections);

        // for potential non one compomen graphs 
        if (minConnections == 0)
            numberConnections -= graph.GetDegree(owner);

        var alreadyAdded = new HashSet<Vertice>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, graph.Count() + 1);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            if (alreadyAdded.Contains<Vertice>(newConnection))
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

    public static OrientedGraph GenerateOriented(string name, int countVertices)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateDirecredConnections(graph, countVertices, vertice);
        }

        return graph;
    }

    public static OrientedGraph GenerateOrientedAcyclic(string name, int countVertices)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        var haveOutgoins = new HashSet<Vertice>();

        foreach (var vertice in graph)
        {
            GenerateDirecredAcyclicConnections(graph, countVertices, vertice, haveOutgoins);
        }

        return graph;
    }

    public static OrientedGraph GenerateOrientedFlow(string name, int countVertices)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateDirecredConnections(graph, countVertices, vertice);
        }

        var random = new Random();

        foreach (var vertice in graph)
        {
            var edges = graph.GetEdges(vertice);

            foreach (var edge in edges)
            {
                int leangth = random.Next(1, 10);
                graph.SetEdgeLength(vertice, edge, leangth);
            }
        }

        return graph;
    }

    private static void GenerateDirecredConnections(OrientedGraph graph, int countVertices, Vertice owner)
    {
        int numberConnections = _random.Next(0, countVertices  / 2);

        var alreadyAdded = new HashSet<Vertice>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            if (alreadyAdded.Contains<Vertice>(newConnection))
                continue;

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }

    private static void GenerateDirecredAcyclicConnections(OrientedGraph graph, int countVertices, Vertice owner, HashSet<Vertice> haveOutgoins)
    {
        int numberConnections = _random.Next(1, (countVertices - 1) / 2);

        var alreadyAdded = new HashSet<Vertice>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            if (haveOutgoins.Contains<Vertice>(newConnection))
                continue;

            if (alreadyAdded.Contains<Vertice>(newConnection))
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
