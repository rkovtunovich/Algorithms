using Graphs.GraphImplementation;

namespace Graphs;

public static class GraphGenerator<T>
{
    private static readonly Random _random = new();

    #region NonOriented

    public static UndirectedGraph<T> GenerateNonOriented(int countVertices)
    {
        var graph = new UndirectedGraph<T>("undirected");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateMutualConnections(graph, countVertices, vertice);
        }

        return graph;
    }

    private static void GenerateMutualConnections(UndirectedGraph<T> graph, int countVertices, Vertice<T> owner)
    {
        int numberConnections = _random.Next(0, (countVertices - 1) / 2);

        numberConnections -= graph.GetDegree(owner);

        var alreadyAdded = new HashSet<Vertice<T>>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            if (alreadyAdded.Contains<Vertice<T>>(newConnection))
                continue;

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }

    #endregion

    #region Oriented

    public static OrientedGraph<T> GenerateOriented(int countVertices)
    {
        var graph = new OrientedGraph<T>("oriended");

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

    public static OrientedGraph<T> GenerateOrientedAcyclic(int countVertices)
    {
        var graph = new OrientedGraph<T>("oriended_acyclic");

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        var haveOutgoins = new HashSet<Vertice<T>>();

        foreach (var vertice in graph)
        {
            GenerateDirecredAcyclicConnections(graph, countVertices, vertice, haveOutgoins);
        }

        return graph;
    }

    private static void GenerateDirecredConnections(OrientedGraph<T> graph, int countVertices, Vertice<T> owner)
    {
        int numberConnections = _random.Next(0, (countVertices - 1) / 4);

        var alreadyAdded = new HashSet<Vertice<T>>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            if (alreadyAdded.Contains<Vertice<T>>(newConnection))
                continue;

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }

    private static void GenerateDirecredAcyclicConnections(OrientedGraph<T> graph, int countVertices, Vertice<T> owner, HashSet<Vertice<T>> haveOutgoins)
    {
        int numberConnections = _random.Next(1, (countVertices - 1) / 2);

        var alreadyAdded = new HashSet<Vertice<T>>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            if (haveOutgoins.Contains<Vertice<T>>(newConnection))
                continue;

            if (alreadyAdded.Contains<Vertice<T>>(newConnection))
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
