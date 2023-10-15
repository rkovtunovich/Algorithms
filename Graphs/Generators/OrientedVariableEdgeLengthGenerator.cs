using Graphs.GraphImplementation;

namespace Graphs.Generators;

public class OrientedVariableEdgeLengthGenerator : IGraphGenerator
{
    private static readonly Random _random = new();
    private readonly int _countVertices;
    private readonly double _saturation;

    // if it needs vertex without incoming edges
    private readonly int? _originIndex;

    public OrientedVariableEdgeLengthGenerator(int countVertices, double saturation, int? originIndex = null)
    {
        _countVertices = countVertices;
        _originIndex = originIndex;
        _saturation = saturation;
    }

    public Graph Generate(string name)
    {
        var graph = new OrientedGraph(name);

        for (int i = 1; i <= _countVertices; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var vertex in graph)
        {
            GenerateDirectedConnections(graph, _countVertices, vertex);
        }

        var random = new Random();

        foreach (var vertex in graph)
        {
            var edges = graph.GetEdges(vertex);

            foreach (var edge in edges)
            {
                int length = random.Next(1, 10);
                graph.SetEdgeLength(vertex, edge, length);
            }
        }

        return graph;
    }


    private void GenerateDirectedConnections(OrientedGraph graph, int countVertices, Vertex owner)
    {
        int numberConnections = _random.Next(0, (int)(countVertices * _saturation));

        var alreadyAdded = new HashSet<Vertex>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            if (_originIndex == newIndex)
                continue;

            var newConnection = graph.GetVertexByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertex with index {newIndex}");

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }
}

