using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.Generators;

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
            var vertice = new Vertex(i);
            vertice.CopyFrom(_prototype);

            graph.AddVertice(vertice);
        }

        foreach (var vertice in graph)
        {
            GenerateMutualConnections(graph, vertice, 0, (int)(graph.Count() / 1.7));
        }

        foreach (var vertice in graph)
        {
            var edges = graph.GetEdges(vertice);

            foreach (var edge in edges)
            {
                double leangth = Math.Round(_random.NextDouble() * 10, 2);
                graph.SetEdgeLength(vertice, edge, leangth);
                graph.SetEdgeLength(edge, vertice, leangth);
            }
        }

        return graph;
    }

    #region Service methods

    private void GenerateMutualConnections(UndirectedGraph graph, Vertex owner, int minConnections, int maxConnections)
    {
        int numberConnections = _random.Next(minConnections, maxConnections);

        // for potential non one compomen graphs 
        if (minConnections == 0)
            numberConnections -= graph.GetDegree(owner);

        var alreadyAdded = new HashSet<Vertex>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, graph.Count() + 1);

            if (newIndex == owner.Index)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

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
