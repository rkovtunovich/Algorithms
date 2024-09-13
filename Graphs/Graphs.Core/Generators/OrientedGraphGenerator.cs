using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Generators;

public class OrientedGraphGenerator(OrientedGraphGeneratorOptions options) : IGraphGenerator
{
    private static readonly Random _random = new();

    // if it needs vertex without incoming edges
    private readonly int? _originIndex;

    public GraphBase Generate(string name)
    {
        var graph = new OrientedGraph(name, options.TrackIncomeEdges);

        for (int i = 1; i <= options.CountVertices; i++)
            graph.AddVertex(new(i));

        foreach (var vertex in graph)
            GenerateDirectedConnections(graph, vertex);

        return graph;
    }

    private void GenerateDirectedConnections(OrientedGraph graph, Vertex owner)
    {
        var maxConnections = GetMaximumConnections(owner.Index, options.IsOrdered, options.CountVertices);
        var lowerIndexBound = GetLoverIndexBound(owner.Index, options.IsOrdered, options.CountVertices);
        var minimumConnections = GetMinimumConnections(owner.Index, options.IsOrdered, options.CountVertices);
        int numberConnections = _random.Next(minimumConnections, maxConnections);

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(lowerIndexBound, options.CountVertices);

            if (newIndex == owner.Index)
                continue;

            if (_originIndex == newIndex)
                continue;

            var newConnection = graph.GetVertexByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertex with index {newIndex}");

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);
        }
    }

    private static int GetLoverIndexBound(int ownerIndex, bool isOrdered, int countVertices)
    {
        if (isOrdered)
        {
            var result = ownerIndex + 1;

            return result > countVertices ? countVertices : result;
        }

        return 1;
    }

    private static int GetMinimumConnections(int ownerIndex, bool isOrdered, int countVertices)
    {
        if (!isOrdered)
            return 0;

        // all vertices except the last one have at least one connection
        return ownerIndex == countVertices ? 0 : 1;
    }

    private int GetMaximumConnections(int ownerIndex, bool isOrdered, int countVertices)
    {
        int count;
        if (!isOrdered)
            count = countVertices - 1;
        else
            // all vertices with index bigger than owner
            count = countVertices - ownerIndex;

        // multiply be saturation coefficient and round up
        return (int)Math.Ceiling(count * options.Saturation);
    }
}

