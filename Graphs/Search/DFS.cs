using Graphs.Abstraction;

namespace Graphs.Search;

public static class DFS<T>
{
    public static HashSet<Vertice<T>> SearchConnected(Graph<T> graph, Vertice<T> originVertice)
    {
        var visited = new HashSet<Vertice<T>>();

        var stack = new Stack<Vertice<T>>();
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

    public static HashSet<Vertice<T>> SearchConnectedRec(Graph<T> graph, Vertice<T> originVertice)
    {
        var visited = new HashSet<Vertice<T>>();

        Search(graph, originVertice, visited);

        return visited;
    }

    private static void Search(Graph<T> graph, Vertice<T> current, HashSet<Vertice<T>> visited)
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

    public static void GetTopologicalOrdering(Graph<T> graph)
    {
        int curLabel = graph.Count();

        var visited = new HashSet<Vertice<T>>();

        foreach (var item in graph)
        {
            if (visited.Contains(item))
                continue;

            SearchTopo(graph, item, visited, ref curLabel);
        }
    }

    private static void SearchTopo(Graph<T> graph, Vertice<T> current, HashSet<Vertice<T>> visited, ref int curLabel)
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
