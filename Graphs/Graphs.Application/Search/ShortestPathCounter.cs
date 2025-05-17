namespace Graphs.Application.Search;

public static class ShortestPathCounter
{
    /// <summary>
    /// Counts how many distinct shortest paths there are from `source` to `target`
    /// in `graph`, under the assumption that all cycles in `graph` have strictly
    /// positive cost (so Bellman–Ford applies and the “shortest‐path subgraph” is a DAG).
    /// </summary>
    public static long CountShortestPaths(OrientedGraph graph, Vertex source, Vertex target)
    {
        if (!graph.TrackIncomeEdges)
            throw new InvalidOperationException("Graph must have TrackIncomeEdges enabled.");

        // 1) Compute distFrom[u] = shortest‐path cost from source → u
        var fromInfo = BellmanFordAlgorithm.FindShortestPaths(graph, source);
        var distFrom = fromInfo.Select(t => t.distance).ToArray();

        // 2) Compute distTo[u]   = shortest‐path cost from u → target
        //    by running on the TRANSPOSED graph from `target`
        var transposed = graph.Transpose() as OrientedGraph
                         ?? throw new InvalidOperationException("Transpose failed.");
        transposed.FillIncomeEdges(true);
        var toInfo = BellmanFordAlgorithm.FindShortestPaths(transposed, target);
        var distTo = toInfo.Select(t => t.distance).ToArray();

        double best = distFrom[target.Index - 1];
        if (double.IsInfinity(best))
            return 0;    // no path at all

        // 3) Build the “shortest‐path DAG” H by keeping only edges (u→v)
        //    for which distFrom[u] + cost(u,v) + distTo[v] == distFrom[target]
        var H = new Dictionary<Vertex, List<Vertex>>();
        foreach (var u in graph)
            H[u] = [];

        foreach (var u in graph)
        {
            foreach (var v in graph.GetAdjacentEdges(u))
            {
                double w = graph.GetEdgeLength(u, v);
                if (distFrom[u.Index - 1] + w + distTo[v.Index - 1] == best)
                {
                    // this edge lies on *some* shortest source→target path
                    H[u].Add(v);
                }
            }
        }

        // 4) Topologically sort H (it’s a DAG because no zero‐cost cycles can exist)
        var indegree = new Dictionary<Vertex, int>();
        foreach (var u in H.Keys) indegree[u] = 0;
        foreach (var u in H.Keys)
            foreach (var v in H[u])
                indegree[v]++;

        var queue = new Queue<Vertex>();
        foreach (var u in H.Keys)
            if (indegree[u] is 0)
                queue.Enqueue(u);

        var topo = new List<Vertex>();
        while (queue.Count > 0)
        {
            var u = queue.Dequeue();
            topo.Add(u);
            foreach (var v in H[u])
                if (--indegree[v] is 0)
                    queue.Enqueue(v);
        }

        // 5) Count paths by DP in topological order
        var count = new Dictionary<Vertex, long>();
        foreach (var u in H.Keys) count[u] = 0;
        count[source] = 1;

        foreach (var u in topo)
        {
            long cu = count[u];
            if (cu is 0) 
                continue;

            foreach (var v in H[u])
                count[v] += cu;
        }

        return count[target];
    }
}
