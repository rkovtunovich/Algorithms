namespace Graphs;

public static class GraphGenerator<T> where T : INumber<T>
{
    private static readonly Random _random = new();

    public static Graph<T> GenerateNonOriented(int countVertices)
    {
        var graph = new Graph<T>();

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(new Vertice<T> { Index = i });
        }

        foreach (var vertice in graph)
        {
            GenerateConnections(graph, countVertices, vertice);
        }

        return graph;
    }

    private static void GenerateConnections(Graph<T> graph, int countVertices, Vertice<T> owner)
    {
        int numberConnections = _random.Next(0, (countVertices - 1) / 3);

        numberConnections -= graph.GetDegree(owner);

        var alreadyAdded = new HashSet<Vertice<T>>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            var newConnection = new Vertice<T>() { Index = newIndex };

            if (alreadyAdded.Contains<Vertice<T>>(newConnection))
                continue;

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }
}
