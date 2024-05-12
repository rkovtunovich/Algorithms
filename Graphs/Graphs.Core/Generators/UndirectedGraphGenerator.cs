using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Generators;

public class UndirectedGraphGenerator : IGraphGenerator
{
    private static readonly Random _random = new();
    private readonly int _vertexCount;
    private readonly double _saturation;

    public UndirectedGraphGenerator(int vertexCount, double saturation)
    {
        _vertexCount = vertexCount;
        _saturation = saturation;
    }

    public GraphBase Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        for (int i = 1; i <= _vertexCount; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var vertex in graph)
        {
            GenerateMutualConnections(graph, vertex, 0, (int)(graph.Count() * _saturation));
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
}
