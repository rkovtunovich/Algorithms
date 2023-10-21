using DataStructures.Lists;
using Graphs.Core.Abstraction;
using Graphs.Core.Model;

namespace Graphs.Core.Search;

public static class DFS
{
    public static HashSet<Vertex> SearchConnected(Graph graph, Vertex originVertex)
    {
        var visited = new HashSet<Vertex>();

        var stack = new Stack<Vertex>();
        stack.Push(originVertex);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (visited.Contains(current))
                continue;

            visited.Add(current);

            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                stack.Push(edge);
            }
        }

        return visited;
    }

    public static HashSet<Vertex> SearchConnectedRec(Graph graph, Vertex originVertex)
    {
        var visited = new HashSet<Vertex>();

        Search(graph, originVertex, visited);

        return visited;
    }

    private static void Search(Graph graph, Vertex current, HashSet<Vertex> visited)
    {
        if (visited.Contains(current))
            return;

        visited.Add(current);

        var edges = graph.GetEdges(current);

        foreach (var edge in edges)
        {
            Search(graph, edge, visited);
        }
    }

    #region Cycle

    /// <summary>
    /// Searches for a cycle in the graph starting from the specified origin vertex using Depth-first search (DFS).
    /// </summary>
    /// <param name="graph">Graph to search the cycle in.</param>
    /// <returns>A hash set of vertices that form a cycle starting from the origin vertex.</returns>
    public static SequentialList<Vertex> SearchCycle(Graph graph)
    {
        // we got to key vertex from value vertex
        var path = new Dictionary<Vertex, Vertex>();
        var visited = new HashSet<Vertex>();

        foreach (var vertex in graph)
        {
            if (visited.Contains(vertex))
                continue;

            var stack = new Stack<Vertex>();
            stack.Push(vertex);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                // If current vertex is already visited, then we have a cycle.
                if (visited.Contains(current))
                    return BackTrackPath(graph, current, path);

                visited.Add(current);

                var edges = graph.GetEdges(current);

                foreach (var edge in edges)
                {
                    if (visited.Contains(edge))
                        continue;

                    stack.Push(edge);
                    path[edge] = current;
                }
            }
        }

        visited.Clear();

        // If no cycle is found after traversing the entire graph, return an empty list.
        return new();
    }

    private static SequentialList<Vertex> BackTrackPath(Graph graph, Vertex current, Dictionary<Vertex, Vertex> path)
    {
        var cycle = new SequentialList<Vertex>
        {
            current
        };

        while (true)
        {
            current = path[current];
            cycle.Add(current);
            if (cycle.Count > 2 && graph.IsConnected(current, cycle[0]))
                break;
        }

        return cycle;
    }

    #endregion

    #region StronglyConenctedComponents

    public static void KosarajuSharirSearch(Graph graph)
    {
        var transposed = graph.Transpose();
        var vertices = TopologicalOrdering.SortTopologically(transposed);

        var visited = new HashSet<Vertex>();
        int numSCC = 0;

        foreach (var vertex in vertices)
        {
            if (visited.Contains(vertex))
                continue;

            numSCC++;

            SearchSCC(graph, vertex, visited, ref numSCC);
        }
    }

    private static void SearchSCC(Graph graph, Vertex current, HashSet<Vertex> visited, ref int numSCC)
    {
        if (visited.Contains(current))
            return;

        visited.Add(current);
        current.Component = numSCC;

        var edges = graph.GetEdges(current);

        foreach (var edge in edges)
        {
            SearchSCC(graph, edge, visited, ref numSCC);
        }
    }

    #endregion
}
