using Graphs.Abstraction;
using Graphs.Model;

namespace Graphs.Search;

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
