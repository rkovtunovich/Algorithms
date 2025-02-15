﻿using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

public static class BellmanFordAlgorithm
{
    #region Shortest Paths

    /// <summary>
    /// Executes the Bellman-Ford algorithm to find the shortest paths from the start vertex
    /// to all other vertices in the graph.
    /// </summary>
    /// <param name="graph">The graph on which to perform the search.</param>
    /// <param name="start">The starting vertex for the shortest paths.</param>
    /// <returns>
    /// An array where each element is a tuple containing the shortest distance from the start vertex
    /// and the predecessor vertex used to reach the current vertex.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if the graph contains a negative weight cycle.</exception>
    public static (double distance, Vertex? predecessor)[] FindShortestPaths(OrientedGraph graph, Vertex start)
    {
        if(!graph.TrackIncomeEdges)
            throw new InvalidOperationException("Graph must track income edges");

        var vertexCount = graph.Count;
        var distances = new (double distance, Vertex? predecessor)[vertexCount];

        // Initialize distances: set all distances to infinity and predecessors to null
        for (int i = 0; i < vertexCount; i++)
            distances[i].distance = double.MaxValue;

        // The distance to the start vertex is zero
        distances[start.Index - 1].distance = 0;

        // Relax all edges up to V-1 times, where V is the number of vertices
        for (int i = 1; i <= vertexCount - 1; i++)
        {
            foreach (var vertex in graph)
            {
                // Get the minimum distance to the current vertex from its incoming edges
                double minLength = GetMinimum(graph, vertex, distances);

                // Update the distance if a shorter path is found
                distances[vertex.Index - 1].distance = Math.Min(distances[vertex.Index - 1].distance, minLength);
            }
        }

        // Check for the existence of negative weight cycles in the graph
        if (HasNegativeCycle(graph, distances))
            throw new InvalidOperationException("Graph contains a negative cycle");

        return distances;
    }

    /// <summary>
    /// Computes the minimum distance to a vertex from its incoming edges and updates the predecessor.
    /// </summary>
    /// <param name="graph">The graph containing the vertices and edges.</param>
    /// <param name="vertex">The vertex whose incoming edges are being considered.</param>
    /// <param name="distances">The array of distances and predecessors.</param>
    /// <returns>The minimum distance to the vertex from any of its incoming edges.</returns>
    private static double GetMinimum(OrientedGraph graph, Vertex vertex, (double distance, Vertex? predecessor)[] distances)
    {
        var edges = graph.GetIncomeEdges(vertex);

        double min = double.MaxValue;
        foreach (var incomeEdge in edges)
        {
            // Calculate the potential distance via the incoming edge
            var distance = distances[incomeEdge.Index - 1].distance + graph.GetEdgeLength(incomeEdge, vertex);
            if (distance >= min)
                continue;

            // Update the minimum distance and the predecessor
            min = distance;
            distances[vertex.Index - 1].predecessor = incomeEdge;
        }

        return min;
    }

    /// <summary>
    /// Checks if the graph contains a negative weight cycle by seeing if further relaxation is possible.
    /// </summary>
    /// <param name="graph">The graph to check for negative cycles.</param>
    /// <param name="distances">The array of distances and predecessors.</param>
    /// <returns>True if a negative cycle is detected, otherwise false.</returns>
    private static bool HasNegativeCycle(OrientedGraph graph, (double distance, Vertex? predecessor)[] distances)
    {
        foreach (var vertex in graph)
        {
            foreach (var incomeEdge in graph.GetIncomeEdges(vertex))
            {
                // If further relaxation is possible, there is a negative weight cycle
                var distance = distances[incomeEdge.Index - 1].distance + graph.GetEdgeLength(incomeEdge, vertex);
                if (distance < distances[vertex.Index - 1].distance)
                    return true; // Negative cycle detected
            }
        }

        return false; // No negative cycle found
    }

    /// <summary>
    /// Reconstructs the shortest path from the start vertex to the end vertex using the predecessor array.
    /// </summary>
    /// <param name="start">The starting vertex of the path.</param>
    /// <param name="end">The ending vertex of the path.</param>
    /// <param name="distances">The array of distances and predecessors.</param>
    /// <returns>A list of vertices representing the shortest path from start to end.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no path exists between the start and end vertices.</exception>
    public static List<Vertex> ReconstructPath(Vertex start, Vertex end, (double distance, Vertex? predecessor)[] distances)
    {
        var path = new List<Vertex>();

        var current = end;
        while (current != start)
        {
            // Add the current vertex to the path and move to its predecessor
            path.Add(current);
            current = distances[current.Index - 1].predecessor ?? throw new InvalidOperationException("No path found");
        }

        // Add the start vertex and reverse the path to get the correct order
        path.Add(start);
        path.Reverse();

        return path;
    }

    #endregion

    #region Opportunity cycle

    /// <summary>
    /// Executes the Bellman-Ford algorithm to find negative weight cycles, indicating an opportunity cycle.
    /// </summary>
    /// <param name="graph">The graph with logarithmic transformed weights.</param>
    /// <param name="start">The starting vertex for the search.</param>
    /// <returns>
    /// A list of vertices representing an opportunity cycle if one exists; otherwise, null.
    /// </returns>
    public static List<Vertex>? FindOpportunityCycle(OrientedGraph graph)
    {
        if (!graph.TrackIncomeEdges)
            throw new InvalidOperationException("Graph must track income edges");

        var vertexCount = graph.Count;
        var distances = new (double distance, Vertex? predecessor)[vertexCount];

        // Initialize distances and predecessors
        for (int i = 0; i < vertexCount; i++)
            distances[i].distance = double.MaxValue;

        distances[0].distance = 0; // Start from the first vertex (arbitrary)

        // Relax edges up to V-1 times
        for (int i = 1; i <= vertexCount - 1; i++)        
            foreach (var vertex in graph)         
                UpdateDistances(graph, vertex, distances);
            
        // Detect and extract the negative cycle
        return ExtractNegativeCycle(graph, distances);
    }

    private static void UpdateDistances(OrientedGraph graph, Vertex vertex, (double distance, Vertex? predecessor)[] distances)
    {
        foreach (var edge in graph.GetIncomeEdges(vertex))
        {
            var newDistance = distances[edge.Index - 1].distance + graph.GetEdgeLength(edge, vertex);
            if (newDistance < distances[vertex.Index - 1].distance)
            {
                distances[vertex.Index - 1].distance = newDistance;
                distances[vertex.Index - 1].predecessor = edge;
            }
        }
    }

    private static List<Vertex>? ExtractNegativeCycle(OrientedGraph graph, (double distance, Vertex? predecessor)[] distances)
    {
        for (int i = 0; i < graph.Count; i++)
        {
            var current = graph.GetVertexByIndex(i);

            foreach (var edge in graph.GetIncomeEdges(current!))
            {
                if (distances[edge.Index - 1].distance + graph.GetEdgeLength(edge, current!) >= distances[i].distance)                
                    continue;
                
                // Cycle detected, backtrack to find the cycle path
                var cycle = new List<Vertex>();
                var visited = new HashSet<int>();

                while (!visited.Contains(current!.Index))
                {
                    visited.Add(current.Index);
                    cycle.Add(current);

                    current = distances[current.Index - 1].predecessor;

                    if (current is null)
                        break;
                }

                cycle.Reverse();

                return cycle;
            }
        }

        return null; // No negative cycle found
    }

    /// <summary>
    /// Converts a matrix of trade ratios to a graph with logarithmic transformed weights.
    /// </summary>
    /// <param name="tradeRatios"></param>
    /// <returns>Oriented graph with logarithmic transformed weights.</returns>
    public static OrientedGraph RatiosToGraph(double[,] tradeRatios)
    {
        var graph = new OrientedGraph("ratios");

        for (int i = 0; i < tradeRatios.GetLength(0); i++)
            graph.AddVertex(new Vertex(i + 1));

        for (int i = 0; i < tradeRatios.GetLength(0); i++)
        {
            for (int j = 0; j < tradeRatios.GetLength(1); j++)
            {
                if (i == j || tradeRatios[i, j] <= 0)               
                    continue;
                
                double weight = -Math.Log(tradeRatios[i, j]);
                var vertexA = graph.GetVertexByIndex(i + 1);
                var vertexB = graph.GetVertexByIndex(j + 1);
                graph.AddEdgeWithLength(vertexA!, vertexB!, weight);
            }
        }

        return graph;
    }

    #endregion
}
