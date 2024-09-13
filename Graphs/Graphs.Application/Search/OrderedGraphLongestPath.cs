
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

public class OrderedGraphLongestPath
{
    /// <summary>
    /// Finds the longest path from v1 to vn in an ordered graph.
    /// </summary>
    /// <param name="graph">The ordered graph.</param>
    /// <returns>A list of vertices representing the longest path from v1 to vn.</returns>
    public static List<Vertex> Find(OrientedGraph graph)
    {
        // dp[i] stores the length of the longest path from v1 to vi
        var dp = new int[graph.Count + 1];
        var prev = new Dictionary<Vertex, Vertex?>();

        // Initialize dp[1] for vertex v1
        dp[1] = 0; // The path from v1 to v1 has length 0

        // Process vertices from v2 to vn
        for (int i = 2; i <= graph.Count; i++)
        {
            var vertex = graph.GetVertexByIndex(i) ?? throw new InvalidOperationException($"Vertex with index {i} not found.");

            // Get incoming edges (from vertices with indices less than i)
            var incomingEdges = graph.GetIncomeEdges(vertex);

            // Initialize dp[i] to 0 (in case there are no incoming edges)
            dp[i] = 0;

            // Find the maximum dp value among predecessors
            foreach (var incomingVertex in incomingEdges)
            {
                if (dp[incomingVertex.Index] + 1 > dp[i])
                {
                    dp[i] = dp[incomingVertex.Index] + 1;
                    prev[vertex] = incomingVertex;
                }
            }
        }

        // Backtrack from vn to reconstruct the longest path
        var path = new List<Vertex>();
        var maxVertex = graph.GetVertexByIndex(graph.Count) ?? throw new InvalidOperationException($"Vertex with index {graph.Count} not found.");

        while (maxVertex is not null)
        {
            path.Add(maxVertex);

            // Move to the predecessor of the current vertex
            prev.TryGetValue(maxVertex, out var previousVertex);
            maxVertex = previousVertex;
        }

        // Reverse the path to get it from v1 to vn
        path.Reverse();

        return path;
    }
}
