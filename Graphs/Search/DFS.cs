using Graphs.Abstraction;
using Graphs.Model;

namespace Graphs.Search;

public static class DFS
{
    public static HashSet<Vertex> SearchConnected(Graph graph, Vertex originVertice)
    {
        var visited = new HashSet<Vertex>();

        var stack = new Stack<Vertex>();
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

    public static HashSet<Vertex> SearchConnectedRec(Graph graph, Vertex originVertice)
    {
        var visited = new HashSet<Vertex>();

        Search(graph, originVertice, visited);

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

    public static Vertex[] SortTopologicaly(Graph graph)
    {
        int curLabel = graph.Count();

        var vertices = new Vertex[curLabel]; 

        var visited = new HashSet<Vertex>();

        foreach (var item in graph)
        {
            if (visited.Contains(item))
                continue;

            TopoSort(graph, item, visited, ref curLabel, vertices);
        }

        return vertices;
    }

    private static void TopoSort(Graph graph, Vertex current, HashSet<Vertex> visited, ref int curLabel, Vertex[] vertices)
    {
        visited.Add(current);

        var edges = graph.GetEdges(current);

        foreach (var edge in edges)
        {
            if (!visited.Contains(edge))
                TopoSort(graph, edge, visited, ref curLabel, vertices);
        }

        current.Distance = curLabel--;
        vertices[curLabel] = current;
    }

    #region StronglyConenctedComponents

    public static void KosarajuSharirSearch(Graph graph)
    {
        var transposed = graph.Transpose();       
        var vertices = SortTopologicaly(transposed);

        var visited = new HashSet<Vertex>();
        int numSCC = 0;

        foreach (var vertice in vertices)
        {
            if (visited.Contains(vertice))
                continue;

            numSCC++;

            SearchSCC(graph, vertice, visited, ref numSCC);
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
