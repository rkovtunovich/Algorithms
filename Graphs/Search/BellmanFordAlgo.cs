using Graphs.GraphImplementation;

namespace Graphs.Search;

public static class BellmanFordAlgo
{
    private const int MaxPossibleLength = 1000;

    public static double?[][] Search(OrientedGraph graph, Vertex start)
    {
        var span = new double?[graph.Count + 1][];

        var incomingEdges = new Dictionary<Vertex, List<Vertex>>();

        for (int i = 0; i <= graph.Count; i++)
        {
            span[i] = new double?[graph.Count];

            if (i == start.Index - 1)
                span[i][0] = 0;
            else
                span[i][0] = MaxPossibleLength;

            if (i is 0)
                continue;

            var vertex = graph.GetVertexByIndex(i);
            var edges = graph.GetEdges(vertex);
            if (!incomingEdges.ContainsKey(vertex))
                incomingEdges.Add(vertex, new());
            foreach (var edge in edges)
            {
                incomingEdges.TryGetValue(edge, out var incoming);
                if (incoming is null)
                    incoming = new();

                incoming.Add(vertex);
                incomingEdges[edge] = incoming;
            }
        }

        int vertexCount = graph.Count;

        for (int i = 1; i <= vertexCount; i++)
        {
            bool stable = true;

            foreach (var vertex in graph)
            {
                double minLength = GetMinimum(incomingEdges, graph, vertex, span, i);

                span[i][vertex.Index - 1] = Math.Min((double)(span[i - 1][vertex.Index - 1] ?? MaxPossibleLength), minLength);

                if (span[i][vertex.Index - 1] != span[i - 1][vertex.Index - 1])
                    stable = false;
            }

            if (stable)
                return span;
        }

        return span;
    }

    private static double GetMinimum(Dictionary<Vertex, List<Vertex>> incomingEdges, OrientedGraph graph, Vertex vertex, double?[][] span, int i)
    {
        var edges = incomingEdges[vertex];

        double min = MaxPossibleLength;
        foreach (var incomeEdge in edges)
        {
            double distance = (double)(span[i - 1][incomeEdge.Index - 1] ?? MaxPossibleLength) + graph.GetEdgeLength(incomeEdge, vertex);
            if (distance < min)
                min = distance;
        }

        return min;
    }
}

