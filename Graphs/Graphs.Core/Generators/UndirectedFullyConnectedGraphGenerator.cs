using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Generators;

public class UndirectedFullyConnectedGraphGenerator : IGraphGenerator
{
    private readonly Random _random = new();
    private readonly int _vertexCount;

    public UndirectedFullyConnectedGraphGenerator(int vertexCount)
    {
        _vertexCount = vertexCount;
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
            GenerateMutualConnections(graph, vertex);
        }

        return graph;
    }

    #region Service methods

    private void GenerateMutualConnections(UndirectedGraph graph, Vertex owner)
    {
        var alreadyAdded = new HashSet<Vertex>();

        for (int i = 1; i <= _vertexCount; i++)
        {
            if (i == owner.Index)
                continue;

            var newConnection = graph.GetVertexByIndex(i) ?? throw new Exception($"graph doesn't contain vertex with index {i}");

            if (alreadyAdded.Contains<Vertex>(newConnection))
                continue;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            var length = Math.Round(_random.NextDouble() * 10, 2);
            graph.SetEdgeLength(owner, newConnection, length);
            graph.SetEdgeLength(newConnection, owner, length);

            alreadyAdded.Add(newConnection);
        }
    }

    #endregion
}
