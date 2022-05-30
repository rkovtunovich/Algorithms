using Graphs;

namespace Helpers;

public static class GraphHelper
{
    private static readonly Random _random = new();

    public static Graph<int> GenerateNonOriented(int countVertices)
    {
        var graph = new Graph<int>();

        for (int i = 1; i <= countVertices; i++)
        {
            graph.AddVertice(i);
        }

        for (int i = 1; i <= countVertices; i++)
        {
            GenerateConnections(graph, countVertices, i);
        }

        return graph;
    }

    private static void GenerateConnections(Graph<int> graph, int countVertices, int owner)
    {
        int numberConnections = _random.Next(0, (countVertices - 1) / 3);

        numberConnections -= graph.GetDigree(owner);

        var alreadyAdded = new HashSet<int>();

        while (numberConnections > 0)
        {
            int connection = _random.Next(1, countVertices);

            if (connection == owner)
                continue;

            if (alreadyAdded.Contains<int>(connection))
                continue;

            numberConnections--;

            if (graph.IsConnected(connection, owner))
                continue;

            graph.AddEdge(owner, connection);

            alreadyAdded.Add(connection);
        }
    }
}
