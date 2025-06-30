namespace Graphs.Application.SteinerTrees;

/// <summary>
/// Implements a fixed-parameter dynamic programming algorithm for the Steiner Tree problem
/// on general undirected graphs with non-negative edge weights (satisfying triangle inequality).
/// 
/// Given a set of k terminal vertices, it runs in O(3^k * |V| + 2^k * |V|^2) time,
/// which is practical for small k (number of terminals) even if |V| is large.
/// 
/// Use cases:
///   Network design where only a few nodes must be connected (e.g. multicast routing).
///   Phylogenetic tree reconstruction with a small set of species of interest.
///   VLSI layout: connecting a small set of pins with minimum wiring.
/// Performance: Exponential in k, polynomial in |V|.
///   For k << |V| this is considered fixed-parameter tractable (FPT).
/// </summary>
public static class SteinerTreeSearch
{
    /// <summary>
    /// Computes the minimum total weight of any Steiner tree spanning the given terminals.
    /// </summary>
    /// <param name="graph">
    /// An undirected graph with non-negative (variable) edge lengths.
    /// Graph must allow querying GetEdgeLength(u,v) in O(1).
    /// </param>
    /// <param name="terminals">Set of vertices that must all be connected.</param>
    /// <returns>
    /// The weight of the minimum Steiner tree connecting all terminals (may include extra Steiner nodes).
    /// </returns>
    /// <remarks>
    /// <para>The DP uses a bitmask over subsets of terminals (size 2^k),
    /// and for each subset stores the cost to connect exactly that subset
    /// ending at each graph vertex.  It then combines smaller subsets
    /// and performs a relaxation over edges to propagate distances.</para>
    /// <para>Time complexity: O(2^k * 3^k * |V| + 2^k * |V|^2) in the worst case, but typically dominated by O(2^k * |V|^2).
    /// Space complexity: O(2^k * |V|).</para>
    /// <para>Requires k up to ~15–20 in practice due to the 2^k factor.</para>
    /// </remarks>
    public static double FindMinimumSteinerTreeWeight(
        UndirectedVariableEdgeLengthGraph graph,
        IReadOnlyCollection<Vertex> terminals)
    {
        int k = terminals.Count;
        if (k <= 1)
            return 0;

        // List of all vertices and map each to an index 0..|V|-1
        var vertices = graph.ToList();
        int n = vertices.Count;
        var indexMap = new Dictionary<Vertex, int>(n);
        for (int i = 0; i < n; i++)
            indexMap[vertices[i]] = i;

        var terms = terminals.ToArray();

        int maskCount = 1 << k;
        const double INF = double.MaxValue / 4;

        // dp[mask, v] = minimum cost to connect terminal subset 'mask' ending at vertex v
        var dp = new double[maskCount, n];
        for (int mask = 0; mask < maskCount; mask++)
            for (int v = 0; v < n; v++)
                dp[mask, v] = INF;

        // Initialize singleton subsets: cost to connect terminal i at its own node is zero;
        // or if we "end" at a different node, we pay the distance from term to that node
        for (int i = 0; i < k; i++)
        {
            int m = 1 << i;
            var src = terms[i];
            int srcIdx = indexMap[src];
            for (int v = 0; v < n; v++)
            {
                // Direct edge length or multi‐hop distance: here assume triangle inequality
                // and use direct GetEdgeLength to approximate distance if preprocessed
                dp[m, v] = (v == srcIdx)
                    ? 0
                    : graph.GetEdgeLength(src, vertices[v]);
            }
        }

        // Build up DP for all masks
        for (int mask = 1; mask < maskCount; mask++)
        {
            // skip masks with only one bit (already init)
            if ((mask & (mask - 1)) is 0)
                continue;

            // 1) Combine two submasks
            // For each proper nonempty submask 'sub', let other = mask ^ sub
            for (int sub = (mask - 1) & mask; sub > 0; sub = (sub - 1) & mask)
            {
                int other = mask ^ sub;
                if (other is 0)
                    continue;

                for (int v = 0; v < n; v++)
                {
                    double cost = dp[sub, v] + dp[other, v];
                    if (cost < dp[mask, v])
                        dp[mask, v] = cost;
                }
            }

            // 2) "Steiner" relaxation: propagate via one more edge
            // We do one pass of all‐pairs relaxation: dp[mask,u] + edge(u,v)
            for (int u = 0; u < n; u++)
            {
                double baseCost = dp[mask, u];
                if (baseCost >= INF) 
                    continue;

                foreach (var w in graph.GetAdjacentEdges(vertices[u]))
                {
                    int v = indexMap[w];
                    double newCost = baseCost + graph.GetEdgeLength(vertices[u], w);
                    if (newCost < dp[mask, v])
                        dp[mask, v] = newCost;
                }
            }
        }

        // Finally, the answer is the minimum over all end‐vertices v for the full mask
        double best = INF;
        int full = maskCount - 1;
        for (int v = 0; v < n; v++)
            if (dp[full, v] < best)
                best = dp[full, v];

        return best;
    }
}
