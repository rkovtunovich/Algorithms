using Graphs.GraphImplementation;

namespace Graphs.Search;
public static class BFS<T>
{
    public static HashSet<Vertice<T>> SearchConnected(UndirectedGraph<T> graph, Vertice<T> originVertice)
    {
        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);
            }
        }

        return visited;
    }

    public static HashSet<Vertice<T>> MarkPaths(UndirectedGraph<T> graph, Vertice<T> originVertice)
    {
        int level = 0;

        originVertice.Value = level;

        var visited = new HashSet<Vertice<T>>
        {
            originVertice
        };

        var queue = new Queue<Vertice<T>>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            level = (int)(current.Value ?? 0) + 1;

            var edges = graph.GetEdges(current);

            if (edges.Count == 0)
                continue;

            var edgeNode = edges.First;
            if (edgeNode is null)
                continue;

            while (edgeNode is not null)
            {
                var edge = edgeNode.Value;

                if (visited.Contains(edge))
                {
                    edgeNode = edgeNode.Next;
                    continue;
                }

                edge.Value = level;

                visited.Add(edge);
                queue.Enqueue(edge);

                edgeNode = edgeNode.Next;
            }
        }

        return visited;
    }

    public static void FindingConnectedComponents(UndirectedGraph<T> graph)
    {
        int component = 0;
        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();

        foreach (var vertive in graph)
        {
            if (visited.Contains(vertive))
                continue;

            component++;

            vertive.Component = component;
            queue.Enqueue(vertive);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var edges = graph.GetEdges(current);

                foreach (var edge in edges)
                {
                    if (visited.Contains(edge))
                        continue;

                    edge.Component = component;

                    visited.Add(edge);
                    queue.Enqueue(edge);
                }
            }
        }
    }

    public static OrientedGraph<T> GetSimpleShortestPathTree(UndirectedGraph<T> graph, Vertice<T> originVertice)
    {
        var tree = new OrientedGraph<T>("simple_tree");
        graph.CopyVerticesTo(tree);

        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);

                if(!edge.Equals(originVertice))
                    tree.AddEdge(edge, current);
            }
        }

        return tree;
    }

    public static OrientedGraph<T> GetFullShortestPathTree(UndirectedGraph<T> graph, Vertice<T> originVertice)
    {
        var tree = new OrientedGraph<T>("full_tree");
        graph.CopyVerticesTo(tree);

        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);

                if (!edge.Equals(originVertice))
                    tree.AddEdge(edge, current);
            }
        }

        return tree;
    }
}
