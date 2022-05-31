namespace Graphs;
public  static class BFS<T> where T : INumber<T>
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
                if(visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);
            }
        }

        return visited;
    }
}
