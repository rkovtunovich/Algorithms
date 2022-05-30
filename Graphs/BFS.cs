namespace Graphs;
public  static class BFS<T> where T : notnull
{
    public static HashSet<T> SearchConnected(Graph<T> graph, T originVertice)
    {
        var visited = new HashSet<T>();

        var queue = new Queue<T>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue(); 
            
            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if(visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);
            }
        }

        return visited;
    }
}
