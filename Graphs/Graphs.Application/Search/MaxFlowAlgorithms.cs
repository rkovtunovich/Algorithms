using Graphs.Core;

namespace Graphs.Application.Search;

public static class MaxFlowAlgorithms
{
    /// <summary>
    /// Computes the maximum s‑t flow using the Edmonds‑Karp variant of the
    /// Ford–Fulkerson method (BFS augmenting paths).
    /// </summary>
    /// <param name="graph">Capacity graph (non‑negative edge lengths).</param>
    /// <param name="source">Source vertex <c>s</c>.</param>
    /// <param name="sink">Sink vertex <c>t</c>.</param>
    /// <param name="visualize">If <see langword="true"/> renders the residual graph on every augmentation.</param>
    /// <returns>The value of the maximum flow from <paramref name="source"/> to <paramref name="sink"/>.</returns>
    public static double EdmondsKarp(
        GraphBase graph,
        Vertex source,
        Vertex sink,
        bool visualize = false)
    {
        ArgumentNullException.ThrowIfNull(graph);

        var residual = BuildInitialResidual(graph);
        double maxFlow = 0;

        const double EPS = 1e-9;

        while (TryFindAugmentingPath(residual, source, sink, out var parent))
        {
            // 1. Bottleneck capacity
            var pathFlow = double.PositiveInfinity;
            for (var v = sink; v != source; v = parent[v.ArrayIndex()])
            {
                var u = parent[v.ArrayIndex()];
                pathFlow = Math.Min(pathFlow, residual.GetEdgeLength(u, v));
            }

            // 2. Augment residual network
            for (var v = sink; v != source; v = parent[v.ArrayIndex()])
            {
                var u = parent[v.ArrayIndex()];
                residual.ChangeEdgeLength(u, v, -pathFlow);
                residual.ChangeEdgeLength(v, u, pathFlow);
            }

            if (pathFlow < EPS)
                break;          // Numerical guard
            maxFlow += pathFlow;

            if (visualize)
                DOTVisualizer.VisualizeGraph(residual);
        }

        return maxFlow;
    }

    /// <summary>
    /// Computes the maximum s‑t flow using the *original* Ford‑Fulkerson
    /// approach: repeatedly find any augmenting path with DFS.
    /// </summary>
    /// <remarks>
    /// • **Termination** is guaranteed only when every capacity is an integer
    ///   because each augmentation increases the flow by at least 1.<br/>
    /// • If you need a polynomial bound regardless of capacities, use
    ///   <see cref="EdmondsKarp"/> instead.
    /// </remarks>
    /// <param name="graph">Capacity graph (non‑negative edge lengths).</param>
    /// <param name="source">Source vertex <c>s</c>.</param>
    /// <param name="sink">Sink vertex <c>t</c>.</param>
    /// <param name="visualize">Visualize residual graph after each augmentation.</param>
    /// <returns>The value of the maximum flow.</returns>
    public static double FordFulkerson(
        GraphBase graph,
        Vertex source,
        Vertex sink,
        bool visualize = false)
    {
        ArgumentNullException.ThrowIfNull(graph);

        var residual = BuildInitialResidual(graph);
        double maxFlow = 0;

        // Try to find augmenting paths until none remain.
        while (TryFindAugmentingPathDfs(residual, source, sink, out var parent, out var bottleneck))
        {
            // 1. Augment residual network along the discovered path.
            for (var v = sink; v != source; v = parent[v.ArrayIndex()])
            {
                var u = parent[v.ArrayIndex()];
                residual.ChangeEdgeLength(u, v, -bottleneck);
                residual.ChangeEdgeLength(v, u, bottleneck);
            }

            maxFlow += bottleneck;

            if (visualize)
                DOTVisualizer.VisualizeGraph(residual);
        }

        return maxFlow;
    }

    /// <summary>
    /// Computes max s‑t flow using the Ford–Fulkerson capacity‑scaling technique.
    /// In each phase δ, we only use residual edges with capacity ≥ δ (δ starts
    /// from the largest power of two ≤ max capacity and halves each phase).
    /// </summary>
    /// <remarks>
    /// • For **integer capacities**, runs in O(E² log U), where U is max capacity.<br/>
    /// • With fractional capacities, use a tolerance EPS; termination and bounds
    ///   still behave well if capacities are near‑integral.
    /// </remarks>
    public static double FordFulkersonScaling(
        GraphBase graph,
        Vertex source,
        Vertex sink,
        bool visualize = false)
    {
        ArgumentNullException.ThrowIfNull(graph);

        const double EPS = 1e-9;

        var residual = BuildInitialResidual(graph);
        double maxFlow = 0;

        double delta = InitialDelta(residual);
        if (delta <= EPS) 
            return 0;

        while (delta > EPS)
        {
            // Keep augmenting while there is a δ‑path.
            while (TryFindAugmentingPathDfsWithDelta(residual, source, sink, delta, out var parent, out var bottleneck))
            {
                // Augment along the path.
                for (var v = sink; v != source; v = parent[v.ArrayIndex()])
                {
                    var u = parent[v.ArrayIndex()];
                    residual.ChangeEdgeLength(u, v, -bottleneck);
                    residual.ChangeEdgeLength(v, u, bottleneck);
                }

                maxFlow += bottleneck;

                if (visualize)
                    DOTVisualizer.VisualizeGraph(residual);
            }

            delta *= 0.5; // next scaling phase
        }

        return maxFlow;
    }

    #region Helpers

    private static GraphBase BuildInitialResidual(GraphBase original)
    {
        var g = original.Clone();

        // Ensure every forward edge has a zero‑capacity reverse edge.
        foreach (var u in g)
            foreach (var v in g.GetAdjacentEdges(u))
                if (!g.IsConnected(v, u))
                    g.AddEdgeWithLength(v, u, 0);

        return g;
    }

    private static bool TryFindAugmentingPath(
        GraphBase residual,
        Vertex s,
        Vertex t,
        out Vertex[] parent)
    {
        parent = new Vertex[residual.Count()];
        var visited = new HashSet<Vertex> { s };
        var q = new Queue<Vertex>();
        q.Enqueue(s);

        while (q.Count > 0)
        {
            var u = q.Dequeue();
            foreach (var v in residual.GetAdjacentEdges(u))
            {
                if (visited.Contains(v)) 
                    continue;

                var capacity = residual.GetEdgeLength(u, v);
                if (capacity <= 0) 
                    continue;          // <‑‑ critical fix

                parent[v.ArrayIndex()] = u;
                if (v == t)
                    return true;

                visited.Add(v);
                q.Enqueue(v);
            }
        }

        return false;
    }

    /// <summary>
    /// Depth‑first search for any s‑t path with positive residual capacity.
    /// Returns <c>true</c> and fills <paramref name="parent"/> / <paramref name="bottleneck"/>
    /// if an augmenting path exists.
    /// </summary>
    private static bool TryFindAugmentingPathDfs(
        GraphBase residual,
        Vertex s,
        Vertex t,
        out Vertex[] parent,
        out double bottleneck)
    {
        var visited = new HashSet<Vertex>();
        var currParent = new Vertex[residual.Count];
        bottleneck = double.PositiveInfinity;

        double pathBottleneck = double.PositiveInfinity;

        bool Dfs(Vertex u, double pathMin)
        {
            if (u == t)
            {
                pathBottleneck = pathMin;
                return true;
            }

            visited.Add(u);

            foreach (var v in residual.GetAdjacentEdges(u))
            {
                if (visited.Contains(v)) 
                    continue;

                var cap = residual.GetEdgeLength(u, v);
                if (cap <= 0) 
                    continue; // no residual capacity

                currParent[v.ArrayIndex()] = u;
                if (Dfs(v, Math.Min(pathMin, cap))) 
                    return true;
            }

            return false;
        }

        parent = currParent;
        bool foundPath = Dfs(s, double.PositiveInfinity);
        if (foundPath)
            bottleneck = pathBottleneck;

        return foundPath;
    }

    /// <summary>
    /// Highest power of two ≤ max residual capacity in the graph (≥ 1).
    /// </summary>
    private static double InitialDelta(GraphBase residual)
    {
        double maxCap = 0;
        foreach (var u in residual)
        {
            foreach (var v in residual.GetAdjacentEdges(u))
            {
                var c = residual.GetEdgeLength(u, v);
                if (c > maxCap) maxCap = c;
            }
        }

        if (maxCap <= 1) return Math.Max(0, maxCap); // 0 or 1

        // If capacities are integers (even if stored as double), this is exact.
        var pow = Math.Pow(2, Math.Floor(Math.Log(maxCap, 2)));
        return pow < 1 ? 1 : pow;
    }

    /// <summary>
    /// DFS that searches only edges with residual capacity ≥ delta.
    /// Returns true and provides parent[] / bottleneck if a δ‑augmenting path exists.
    /// </summary>
    private static bool TryFindAugmentingPathDfsWithDelta(
        GraphBase residual,
        Vertex s,
        Vertex t,
        double delta,
        out Vertex[] parent,
        out double bottleneck)
    {
        const double EPS = 1e-9;
        var visited = new HashSet<Vertex>();
        var par = new Vertex[residual.Count];
        bottleneck = double.PositiveInfinity;

        double pathMin = double.PositiveInfinity;

        bool Dfs(Vertex u, double minSoFar)
        {
            if (u == t)
            {
                pathMin = minSoFar;
                return true;
            }

            visited.Add(u);

            foreach (var v in residual.GetAdjacentEdges(u))
            {
                if (visited.Contains(v)) continue;

                var cap = residual.GetEdgeLength(u, v);
                if (cap + EPS < delta) continue; // critical: only edges with cap ≥ δ

                par[v.ArrayIndex()] = u;
                if (Dfs(v, Math.Min(minSoFar, cap))) return true;
            }

            return false;
        }

        var found = Dfs(s, double.PositiveInfinity);
        parent = par;
        if (found) bottleneck = pathMin;
        return found;
    }


    #endregion
}