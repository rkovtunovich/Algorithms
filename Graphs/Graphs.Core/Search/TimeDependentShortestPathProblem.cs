using Graphs.Core.Model.Graphs;
using Models.Scheduling;

namespace Graphs.Core.Search;

public class TimeDependentShortestPathProblem
{
    // Time-Dependent Shortest Path Problem (TDSP) Algorithm Description
    // Problem Overview
    // The Time-Dependent Shortest Path Problem addresses finding the fastest route through a directed graph where edge weights (travel times) vary depending on the departure time.
    // This variation accounts for real-world conditions like traffic, which can significantly affect travel times.
    // The goal is to determine the minimum travel time from a given origin to a destination, starting at a specified time, with all predictions made by a web service considered accurate.
    // 
    // Algorithm Summary
    // This algorithm adapts Dijkstra's shortest path method to accommodate time-dependent edge weights.
    // It iteratively explores the graph, starting from the origin, to find the shortest path to all other vertices,
    // taking into account the time-varying nature of each edge's travel time.
    // 
    // Main Steps
    // Initialization: Set the starting vertex's distance to the initial start time and all other vertices to infinity. Mark the starting vertex as visited.
    // Heap Construction: Use a min-heap (priority queue) to manage vertices based on their current shortest known travel time from the origin.
    // Exploration and Relaxation:
    // Extract the vertex with the minimum distance from the heap.
    // For each adjacent vertex not yet finalized, calculate the new distance considering the departure time from the current vertex and the travel time for the edge connecting them.
    // If this new distance is shorter than the currently known distance, update the adjacent vertex's distance and its predecessor.
    // Repeat this process until the heap is empty or the destination's shortest distance is finalized.
    // Path Reconstruction: Once the algorithm completes, backtrack from the destination vertex using the predecessor links to reconstruct the shortest path.
    // Time-Dependent Edge Weights
    // The edge weights are determined by querying a provided function for each edge that predicts the arrival time based on a given departure time from the source vertex of the edge.
    // This function ensures that f_e(t) >= t and is monotone increasing, meaning later departure times will not result in earlier arrival times.
    // 
    // Complexity Analysis
    // Time Complexity: The algorithm's time complexity primarily depends on the number of vertices V and edges E in the graph, similar to Dijkstra's algorithm.
    // The use of a min-heap for selecting the next vertex to process gives a time complexity of O((V + E) log V) for the algorithm under typical conditions.
    // However, the need to dynamically calculate edge weights based on departure times may introduce additional computational steps, depending on the complexity of the time-dependent functions.
    // Space Complexity: The space complexity is O(V + E) due to storing the graph structure, plus additional structures for tracking distances, predecessors, and heap elements.
    // Additional Considerations
    // Dynamic Edge Weights: Efficient handling and caching of dynamic edge weight calculations can significantly impact performance, especially for graphs with a large number of edges or when the edge weight function is computationally intensive.
    // Heap Updates: Proper implementation of the heap to allow decrease-key operations is crucial for efficiently updating distances and maintaining the min-heap property.
    public static List<Vertex> Search(OrientedGraph graph, Vertex origin, Vertex source, int startTime, Dictionary<(Vertex, Vertex), Dictionary<DailyInterval, int>> timeConditions)
    {
        int marked = 0;

        var predecessors = new Dictionary<Vertex, Vertex>();
        var visited = new HashSet<Vertex>();

        origin.Distance = startTime;
        visited.Add(origin);
        origin.Label = marked.ToString();

        var heap = new HeapMin<double, Vertex>();
        heap.Insert(origin.Distance.Value, origin);
        foreach (var vertex in graph)
        {
            if (vertex == origin)
                continue;

            vertex.Distance = int.MaxValue;
            heap.Insert(vertex.Distance.Value, vertex);
        }

        while (heap.Length > 0)
        {
            Vertex closest = heap.ExtractValue();

            if (closest.Distance is int.MaxValue)
                continue;

            marked++;
            visited.Add(closest);
            closest.Label = $"({marked})";

            MarkClosestNeighbors(graph, closest, heap, visited, timeConditions, predecessors);
        }

        var path = ReconstructShortestPath(predecessors, origin, source);

        return path;
    }

    private static void MarkClosestNeighbors(OrientedGraph graph,
                                             Vertex vertex,
                                             HeapMin<double, Vertex> heap,
                                             HashSet<Vertex> visited,
                                             Dictionary<(Vertex, Vertex), Dictionary<DailyInterval, int>> timeWeights,
                                             Dictionary<Vertex, Vertex> predecessors)
    {
        var edgesClosestVertex = graph.GetAdjacentEdges(vertex);

        foreach (var edge in edgesClosestVertex)
        {
            if (visited.Contains(edge))
                continue;

            double dist = (vertex.Distance ?? 0) + graph.GetEdgeLength(vertex, edge);

            if (timeWeights.TryGetValue((vertex, edge), out var weights))
                dist += GetCondition(weights, (int)vertex.Distance.Value);

            if (dist < edge.Distance)
            {
                edge.Distance = dist;
                heap.ReplaceKeyByValue(edge, dist);

                predecessors[edge] = vertex;
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

    private static int GetCondition(Dictionary<DailyInterval, int> weights, int time)
    {
        var timeOfDay = time % DailyInterval.HoursInDay;

        foreach (var (interval, weight) in weights)
        {
            if (interval.Belongs(timeOfDay))
                return weight;
        }

        return 0;
    }
}