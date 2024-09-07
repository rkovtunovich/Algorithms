using DataStructures.HashTables;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.MWIS;

/// <summary>
/// This class implements the algorithm to find the Maximum Weight Independent Set (MWIS)
/// in a path graph. A path graph is a graph where the nodes form a straight line and
/// are connected only to their immediate neighbors.
/// 
/// An independent set is a set of vertices such that no two vertices are adjacent.
/// The goal of this algorithm is to find an independent set in a path graph such that
/// the sum of the weights of the vertices in the set is maximized.
/// </summary>
public static class PathGraphMWISSearch
{
    /// <summary>
    /// Finds the Maximum Weight Independent Set (MWIS) in a path graph.
    /// </summary>
    /// <param name="pathGraph">The input path graph represented as a GraphBase.</param>
    /// <returns>
    /// A tuple containing:
    /// - A SimpleHashSet<Vertex> representing the vertices in the maximum weight independent set.
    /// - The total weight of the vertices in this set.
    /// </returns>
    public static (SimpleHashSet<Vertex> set, double total) Find(GraphBase pathGraph)
    {
        var set = new SimpleHashSet<Vertex>();
        var vertices = pathGraph.ToList();

        // Handle edge case: if the graph is empty, return an empty set with total weight 0
        if (vertices.Count == 0)
            return (set, 0);

        // Step 1: Compute the maximum weight for each subproblem
        var dp = Calculate(vertices);
        var total = dp[vertices.Count]; // Total maximum weight

        // Step 2: Reconstruct the set of vertices that form the MWIS
        ReconstructionSet(vertices, dp, set);

        return (set, total);
    }

    /// <summary>
    /// Calculates the maximum weight that can be achieved for each subproblem
    /// using dynamic programming. The idea is that for each vertex, we either:
    /// 1. Exclude it and take the maximum weight up to the previous vertex.
    /// 2. Include it, skip the previous vertex, and add the current vertex's weight.
    /// 
    /// dp[i] stores the maximum weight of an independent set for vertices up to i.
    /// </summary>
    /// <param name="vertices">List of vertices in the path graph.</param>
    /// <returns>A double array where each entry represents the maximum weight for the subproblem.</returns>
    private static double[] Calculate(List<Vertex> vertices)
    {
        var calculations = new double[vertices.Count + 1];

        // Base case: No vertices, no weight
        calculations[0] = 0;

        // Base case: Only the first vertex is considered
        calculations[1] = vertices[0].Weight ?? 0;

        // Dynamic programming: calculate the maximum weight for each subproblem
        for (int i = 2; i < calculations.Length; i++)
        {
            // Either take the previous vertex's weight (exclude current vertex)
            // or take the current vertex's weight and the maximum weight 2 steps back
            calculations[i] = Math.Max(calculations[i - 1], calculations[i - 2] + (vertices[i - 1].Weight ?? 0));
        }

        return calculations;
    }

    /// <summary>
    /// Reconstructs the set of vertices that form the Maximum Weight Independent Set (MWIS).
    /// This method uses the dp array to backtrack and figure out which vertices
    /// were part of the optimal solution.
    /// </summary>
    /// <param name="vertices">The list of vertices in the path graph.</param>
    /// <param name="dp">The array of maximum weights computed for each subproblem.</param>
    /// <param name="set">The set where the vertices of the MWIS will be stored.</param>
    private static void ReconstructionSet(List<Vertex> vertices, double[] dp, SimpleHashSet<Vertex> set)
    {
        int i = dp.Length - 1;

        // Backtrack to find which vertices were included in the optimal solution
        while (i >= 2)
        {
            // If excluding the current vertex leads to a higher or equal weight, move to the previous vertex
            if (dp[i - 1] >= (dp[i - 2] + (vertices[i - 1].Weight ?? 0)))
                i--;
            else
            {
                // Otherwise, include the current vertex and move 2 steps back
                set.Add(vertices[i - 1]);
                i -= 2;
            }
        }

        // Handle the case where the first vertex might be included
        if (i is 1)
            set.Add(vertices[i - 1]);
    }
}
