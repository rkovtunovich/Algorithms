using System.Linq;

namespace Graphs;
public static class BFS<T>
{
    public static HashSet<Vertice<T>> SearchConnected(Graph<T> graph, Vertice<T> originVertice)
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

    public static HashSet<Vertice<T>> MarkPaths(Graph<T> graph, Vertice<T> originVertice)
    {
        int level = 0;

        originVertice.Value = level;

        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            level++;

            var current = queue.Dequeue();

            var edges = graph.GetEdges(current);

            //foreach (var edge in edges)
            //{
            //    if (visited.Contains(edge))
            //        continue;

            //    visited.Add(edge);
            //    queue.Enqueue(edge);
            //}

            if (edges.Count == 0)
                continue;

            var edgeNode = edges.First;
            if (edgeNode is null)
                continue;

            while (true)
            {
                var edge = edgeNode.Value;

                if (visited.Contains(edge))
                    continue;

                //edge.Value = level;

                visited.Add(edge);
                queue.Enqueue(edge);

                if (edgeNode?.Next is null)
                    break;
            }


        }

        return visited;
    }
}
