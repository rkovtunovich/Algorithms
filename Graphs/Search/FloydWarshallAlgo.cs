using Graphs.GraphImplementation;

namespace Graphs.Search;

public static class FloydWarshallAlgo
{
    private const int MaxPossibleLength = 100;

    public static void Search(OrientedGraph graph)
    {
        var span = new int[graph.Count + 1][][];

        for (int v = 0; v < graph.Count; v++)
        {
            span[v] = new int[graph.Count][];

            for (int w = 0; w < graph.Count; w++)
            {
                span[v][w] = new int[graph.Count];

                var vertex_V = graph.GetVertexByIndex(v + 1);
                var vertex_W = graph.GetVertexByIndex(w + 1);

                if (v == w)
                    span[0][v][w] = 0;
                else if (graph.IsConnected(vertex_V, vertex_W))
                    span[0][v][w] = (int)graph.GetEdgeLength(vertex_V, vertex_W);
                else
                    span[0][v][w] = MaxPossibleLength;
            }
        }

        span[graph.Count] = new int[graph.Count][];
        for (int i = 0; i < graph.Count; i++)
        {
            span[graph.Count][i] = new int[graph.Count];
        }

        for (int k = 1; k <= graph.Count; k++)
        {
            for (int v = 0; v < graph.Count; v++)
            {
                for (int w = 0; w < graph.Count; w++)
                {
                    span[k][v][w] = Math.Min(span[k - 1][v][w], span[k - 1][v][k - 1] + span[k - 1][k - 1][w]);
                }
            }
        }

        for (int v = 0; v < graph.Count; v++)
        {
            if (span[graph.Count][v][v] < 0)
                Console.WriteLine("Graph has a negative cycle");
        }
    }
}

