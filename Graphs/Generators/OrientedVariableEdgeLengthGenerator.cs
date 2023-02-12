using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.Generators;

public class OrientedVariableEdgeLengthGenerator : IGraphGenerator
{
    private static readonly Random _random = new();
    private readonly int _countVertices;

    // if it needs vertice without incoming edges
    private readonly int? _originIndex;

    public OrientedVariableEdgeLengthGenerator(int countVertices, int? originIndex = null)
    {
        _countVertices = countVertices;
        _originIndex = originIndex;
    }

    public Graph Generate(string name)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= _countVertices; i++)
        {
            graph.AddVertice(new(i));
        }

        foreach (var vertice in graph)
        {
            GenerateDirecredConnections(graph, _countVertices, vertice);
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


    private void GenerateDirecredConnections(OrientedGraph graph, int countVertices, Vertice owner)
    {
        int numberConnections = _random.Next(0, countVertices / 2);

        var alreadyAdded = new HashSet<Vertice>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            if (_originIndex == newIndex)
                continue;

            var newConnection = graph.GetVerticeByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertive with index {newIndex}");

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }
}

