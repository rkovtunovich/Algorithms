using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.Search;

public static class BelmanFordAlgo
{
    private const int MaxPosibleLength = 1000;

    public static double?[][] Search(OrientedGraph graph, Vertice start)
    {
        var span = new double?[graph.Count + 1][];

        var incomingEdges = new Dictionary<Vertice, List<Vertice>>();

        for (int i = 0; i <= graph.Count; i++)
        {
            span[i] = new double?[graph.Count];

            if (i == start.Index - 1)
                span[i][0] = 0;
            else
                span[i][0] = MaxPosibleLength;

            if (i is 0)
                continue;

            var vertice = graph.GetVerticeByIndex(i);
            var edges = graph.GetEdges(vertice);
            if (!incomingEdges.ContainsKey(vertice))
                incomingEdges.Add(vertice, new());
            foreach (var edge in edges)
            {
                incomingEdges.TryGetValue(edge, out var incoming);
                if (incoming is null)
                    incoming = new();

                incoming.Add(vertice);
                incomingEdges[edge] = incoming;
            }
        }

        int verticeCount = graph.Count;

        for (int i = 1; i <= verticeCount; i++)
        {
            bool stable = true;

            foreach (var vertice in graph)
            {
                double minLength = GetMinimum(incomingEdges, graph, vertice, span, i);

                span[i][vertice.Index - 1] = Math.Min((double)(span[i - 1][vertice.Index - 1] ?? MaxPosibleLength), minLength);

                if (span[i][vertice.Index - 1] != span[i - 1][vertice.Index - 1])
                    stable = false;
            }

            if (stable)
                return span;
        }

        return span;
    }

    private static double GetMinimum(Dictionary<Vertice, List<Vertice>> incomingEdges, OrientedGraph graph, Vertice vertice, double?[][] span, int i)
    {
        var edges = incomingEdges[vertice];

        double min = MaxPosibleLength;
        foreach (var incomeEdge in edges)
        {
            double distance = (double)(span[i - 1][incomeEdge.Index - 1] ?? MaxPosibleLength) + graph.GetEdgeLength(incomeEdge, vertice);
            if (distance < min)
                min = distance;
        }

        return min;
    }
}

