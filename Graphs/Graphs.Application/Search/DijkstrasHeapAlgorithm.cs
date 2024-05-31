using DataStructures.Heaps;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

public static class DijkstraHeapAlgorithm
{
    public static List<Vertex> Search(GraphBase graph, Vertex origin, Vertex source)
    {
        int marked = 0;

        var predecessors = new Dictionary<Vertex, Vertex>();
        var visited = new HashSet<Vertex>();

        origin.Distance = 0;
        visited.Add(origin);
        origin.Label = marked.ToString();

        var heap = new HeapMin<double, Vertex>();
        foreach (var vertex in graph)
        {
            vertex.Distance ??= int.MaxValue;
            heap.Insert(vertex.Distance.Value, vertex);
        }

        Vertex? predecessor = null;

        while (heap.Length > 0)
        {
            Vertex closest = heap.ExtractValue();

            if (closest.Distance is int.MaxValue)
                continue;

            if (predecessor is not null)
                predecessors[closest] = predecessor;

            marked++;
            visited.Add(closest);
            closest.Label = $"({marked})";

            MarkClosestNeighbors(graph, closest, heap, visited);

            predecessor = closest;
        }

        var path = ReconstructShortestPath(predecessors, origin, source);

        return path;
    }

    private static void MarkClosestNeighbors(GraphBase graph, Vertex vertex, HeapMin<double, Vertex> heap, HashSet<Vertex> visited)
    {
        var edgesClosestVertex = graph.GetAdjacentEdges(vertex);

        foreach (var edge in edgesClosestVertex)
        {
            if (visited.Contains(edge))
                continue;

            double dist = (vertex.Distance ?? 0) + graph.GetEdgeLength(vertex, edge);

            if (edge.Distance is null || edge.Distance > dist)
            {
                edge.Distance = dist;
                heap.ReplaceKeyByValue(edge, dist);
            }
        }
    }

    private static List<Vertex> ReconstructShortestPath(Dictionary<Vertex, Vertex> predecessors, Vertex start, Vertex end)
    {
        var path = new List<Vertex>();

        if (predecessors.Count is 0)
            return path;

        Vertex? current = end;

        if (end is null)
            return path;

        while (current != start)
        {
            path.Add(current);
            current = predecessors[current];
        }

        path.Add(start);
        path.Reverse();

        return path;
    }
}
