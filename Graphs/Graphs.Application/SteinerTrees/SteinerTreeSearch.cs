using Graphs.Core.Model.Graphs;

namespace Graphs.Application.SteinerTrees;

public static class SteinerTreeSearch
{
    public static double FindMinimumSteinerTreeWeight(
        UndirectedVariableEdgeLengthGraph graph,
        IReadOnlyCollection<Vertex> terminals)
    {
        if (terminals.Count <= 1)
            return 0;

        var vertices = graph.ToList();
        var indexMap = new Dictionary<Vertex, int>();
        for (int i = 0; i < vertices.Count; i++)
            indexMap[vertices[i]] = i;

        var terms = terminals.ToArray();
        int k = terms.Length;
        int maskCount = 1 << k;
        const double INF = double.MaxValue / 4;
        var dp = new double[maskCount, vertices.Count];

        for (int m = 0; m < maskCount; m++)
            for (int v = 0; v < vertices.Count; v++)
                dp[m, v] = INF;

        for (int i = 0; i < k; i++)
        {
            int mask = 1 << i;
            for (int v = 0; v < vertices.Count; v++)
            {
                var from = terms[i];
                var to = vertices[v];
                dp[mask, v] = from == to ? 0 : graph.GetEdgeLength(from, to);
            }
        }

        for (int m = 1; m < maskCount; m++)
        {
            if ((m & (m - 1)) == 0)
                continue; // skip singletons

            for (int sub = (m - 1) & m; sub > 0; sub = (sub - 1) & m)
            {
                int other = m ^ sub;
                if (other == 0) continue;
                for (int v = 0; v < vertices.Count; v++)
                {
                    double candidate = dp[sub, v] + dp[other, v];
                    if (candidate < dp[m, v])
                        dp[m, v] = candidate;
                }
            }

            for (int u = 0; u < vertices.Count; u++)
            {
                if (dp[m, u] >= INF) continue;
                for (int v = 0; v < vertices.Count; v++)
                {
                    double candidate = dp[m, u] + graph.GetEdgeLength(vertices[u], vertices[v]);
                    if (candidate < dp[m, v])
                        dp[m, v] = candidate;
                }
            }
        }

        double best = INF;
        int fullMask = maskCount - 1;
        for (int v = 0; v < vertices.Count; v++)
            best = Math.Min(best, dp[fullMask, v]);

        return best;
    }
}

