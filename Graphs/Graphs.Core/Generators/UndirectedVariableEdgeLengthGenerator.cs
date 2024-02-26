using Graphs.Core.Abstraction;
using Graphs.Core.GraphImplementation;
using Graphs.Core.Model;

namespace Graphs.Core.Generators;

public class UndirectedVariableEdgeLengthGenerator : IGraphGenerator
{
    private readonly Random _random = new();
    private readonly int _countVertices;
    private readonly Vertex? _prototype;

    public UndirectedVariableEdgeLengthGenerator(int countVertices, Vertex? prototype = null)
    {
        _countVertices = countVertices;
        _prototype = prototype;
    }

    public Graph Generate(string name = "undirected_var_length")
    {
        var graph = new UndirectedVariableEdgeLengthGraph(name);

        for (int i = 1; i <= _countVertices; i++)
        {
            var vertex = new Vertex(i);
            vertex.CopyFrom(_prototype);

            graph.AddVertex(vertex);
        }

        foreach (var vertex in graph)
        {
            GenerateMutualConnections(graph, vertex, 0, (int)(graph.Count() / 1.7));
        }

        foreach (var vertex in graph)
        {
            var edges = graph.GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                double length = Math.Round(_random.NextDouble() * 10, 2);
                graph.SetEdgeLength(vertex, edge, length);
                graph.SetEdgeLength(edge, vertex, length);
            }
        }

        return graph;
    }

    #region Service methods

    private void GenerateMutualConnections(UndirectedGraph graph, Vertex owner, int minConnections, int maxConnections)
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
