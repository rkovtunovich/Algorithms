namespace Graphs.Search;

// Dijkstra's algorithm is a graph search algorithm used to find the shortest path between two nodes in a graph with non-negative edge weights.
// It is a greedy algorithm that constructs the shortest path tree starting from the source node,
// iteratively selecting the node with the smallest known distance from the source and updating the distances of its neighboring nodes.
// 
// Here's a step-by-step description of Dijkstra's algorithm:
// 
// 1. Create a set of unvisited nodes and initialize the distance to the source node as 0 and the distance to all other nodes as infinity.
// 2. While there are unvisited nodes:
//      a. Select the node (let's call it 'current') with the smallest known distance from the source that hasn't been visited yet.
//      b. Mark the current node as visited.
//      c. For each neighbor of the current node:
//          * Calculate the distance from the source to the neighbor through the current node.
//          * If this new distance is smaller than the previously known distance to the neighbor, update the neighbor's distance and store the current node as the predecessor of the neighbor on the shortest path.
// 3. After all nodes have been visited or the target node has been visited, the shortest path can be reconstructed by following the predecessor pointers from the target node to the source node.
//
// The time complexity of Dijkstra's algorithm depends on the data structures used to implement it.
// Using an adjacency list representation and a priority queue (such as a binary heap) to manage the unvisited nodes, the time complexity is O((V + E) log V),
// where V is the number of vertices and E is the number of edges in the graph.
// More advanced data structures, like Fibonacci heaps, can further improve the time complexity to O(V log V + E).
// 
// In summary, Dijkstra's algorithm is a greedy algorithm used for finding the shortest path between two nodes in a graph with non-negative edge weights.
// It works by iteratively selecting the node with the smallest known distance from the source and updating the distances of its neighbors.
// The algorithm's time complexity depends on the data structures used,
// with a typical implementation using adjacency lists and priority queues having a time complexity of O((V + E) log V).

public static class DijkstraAlgorithm
{
    public static void Search(Graph graph, Vertex origin)
    {
        int marked = 0;
        marked++;

        origin.Distance = 0;
        origin.Mark = true;
        origin.Label = marked.ToString(); ;
        MarkClosestNeighbors(graph, origin);

        while (marked < graph.Count())
        {
            Vertex? closest = GetNextClosestVertex(graph);

            if (closest is null)
            {
                marked++;
                continue;
            }

            marked++;
            closest.Mark = true;
            closest.Label = marked.ToString();

            MarkClosestNeighbors(graph, closest);
        }
    }

    private static Vertex? GetNextClosestVertex(Graph graph)
    {
        Vertex? closest = null;

        foreach (var vertex in graph)
        {
            if (!(vertex?.Mark ?? false))
                continue;

            var edges = graph.GetEdges(vertex);

            foreach (var edge in edges)
            {
                if (vertex.Mark && edge.Mark)
                    continue;

                closest ??= edge;

                if (closest.Distance > edge.Distance)
                    closest = edge;
            }
        }

        return closest;
    }

    private static void MarkClosestNeighbors(Graph graph, Vertex vertex)
    {
        var edgesClosestVertex = graph.GetEdges(vertex);

        foreach (var edge in edgesClosestVertex)
        {
            if (edge.Mark)
                continue;

            double dist = (vertex.Distance ?? 0) + graph.GetEdgeLength(vertex, edge);

            if (edge.Distance is null || edge.Distance > dist)
                edge.Distance = dist;
        }
    }
}
