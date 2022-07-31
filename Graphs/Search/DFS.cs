using Graphs.Abstraction;

namespace Graphs.Search;

public static class DFS
{
    public static HashSet<Vertice> SearchConnected(Graph graph, Vertice originVertice)
    {
        var visited = new HashSet<Vertice>();

        var stack = new Stack<Vertice>();
        stack.Push(originVertice);

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

    public static HashSet<Vertice> SearchConnectedRec(Graph graph, Vertice originVertice)
    {
        var visited = new HashSet<Vertice>();

        Search(graph, originVertice, visited);

        return visited;
    }

    private static void Search(Graph graph, Vertice current, HashSet<Vertice> visited)
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

    public static void GetTopologicalOrdering(Graph graph)
    {
        int curLabel = graph.Count();

        var visited = new HashSet<Vertice>();

        foreach (var item in graph)
        {
            if (visited.Contains(item))
                continue;

            SearchTopo(graph, item, visited, ref curLabel);
        }
    }

    private static void SearchTopo(Graph graph, Vertice current, HashSet<Vertice> visited, ref int curLabel)
    {
        visited.Add(current);

        var edges = graph.GetEdges(current);

        foreach (var edge in edges)
        {
            if (!visited.Contains(edge))
                SearchTopo(graph, edge, visited, ref curLabel);
        }
        current.Distance = curLabel--;
    }
}
