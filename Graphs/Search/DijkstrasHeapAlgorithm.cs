using DataStructures.Heaps;

namespace Graphs.Search;

public static class DijkstraHeapAlgorithm
{
    public static void Search(Graph graph, Vertex origin)
    {
        int marked = 0;

        origin.Distance = 0;
        origin.Mark = true;
        origin.Label = marked.ToString();

        var heap = new HeapMin<double, Vertex>();
        foreach (var vertex in graph)
        {
            heap.Insert(vertex.Distance ?? throw new ArgumentNullException($"Distance isn't initialized for vertex {vertex.Index}"), vertex);
        }

        while (heap.Length > 0)
        {
            Vertex closest = heap.ExtractValue();

            if (closest.Distance is int.MaxValue)
                continue;

            marked++;
            closest.Mark = true;
            closest.Label = $"-={marked}=-";

            MarkClosestNeighbors(graph, closest, heap);
        }
    }

    private static void MarkClosestNeighbors(Graph graph, Vertex vertex, HeapMin<double, Vertex> heap)
    {
        var edgesClosestVertex = graph.GetEdges(vertex);

        foreach (var edge in edgesClosestVertex)
        {
            if (edge.Mark)
                continue;

            double dist = (vertex.Distance ?? 0) + graph.GetEdgeLength(vertex, edge);

            if (edge.Distance is null || edge.Distance > dist)
            {
                edge.Distance = dist;
                heap.ReplaceKeyByValue(edge, dist);
            }             
        }
    }
}
