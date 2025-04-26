using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;

namespace ResourceOptimization.Scheduling;

/// <summary>
/// Manages scheduling of phone calls in a hierarchical call tree
/// to minimize the number of rounds required to notify everyone.
/// </summary>
public static class CallTreeSchedulingProblem
{
    /// <summary>
    /// Given a call tree hierarchy (an oriented graph that is tree-shaped and rooted at the ranking officer),
    /// calculates the minimum number of rounds required to notify all subordinates and returns a
    /// sequence of calls achieving this minimum number of rounds.
    /// </summary>
    /// <param name="hierarchy">OrientedGraph representing the call tree hierarchy.</param>
    /// <returns>A list of edges (u -> v) in the order the calls should be made.</returns>
    public static List<(Vertex from, Vertex to)> GetOptimalSchedule(OrientedGraph hierarchy)
    {
        // Step 1: Identify the root (ranking officer) of this hierarchy
        //   We assume the graph is a tree (or at least has a single root).
        var root = hierarchy.FindRoot();

        // Step 2: Compute the height (number of rounds to notify all sub-tree) of each node
        //   We'll store the height in a dictionary. 
        var height = new Dictionary<Vertex, int>();
        ComputeHeight(root, hierarchy, height);

        // Step 3: We produce the calling order by doing a DFS from the root
        //   scheduling children in descending order of their 'height'.
        var callOrder = new List<(Vertex from, Vertex to)>();
        ConstructSchedule(root, hierarchy, height, callOrder);

        return callOrder;
    }

    /// <summary>
    /// Recursively computes the height (time needed to notify the sub-tree) of each node.
    /// </summary>
    /// <param name="node">Current node in the recursion.</param>
    /// <param name="hierarchy">The oriented graph representing the call tree.</param>
    /// <param name="height">Dictionary storing the computed height for each node.</param>
    /// <returns>The height of the 'node' sub-tree.</returns>
    private static int ComputeHeight(Vertex node, OrientedGraph hierarchy, Dictionary<Vertex, int> height)
    {
        // If node is already computed, return it
        if (height.TryGetValue(node, out int stored))
            return stored;

        // Gather all children (direct subordinates)
        var children = hierarchy.GetAdjacentEdges(node);

        // If no children, height is 0
        if (children.Count is 0)
        {
            height[node] = 0;
            return 0;
        }

        // Recursively compute children's heights
        var childrenAndHeights = new List<(Vertex child, int h)>();
        foreach (var child in children)
        {
            int hsub = ComputeHeight(child, hierarchy, height);
            childrenAndHeights.Add((child, hsub));
        }

        // Sort children in descending order by h
        childrenAndHeights.Sort((a, b) => b.h.CompareTo(a.h));

        // The finishing time: 1 + max( i + h(child_i) ) for i from 0..k-1
        int maxTime = 0;
        int iIndex = 0;
        foreach (var (child, hValue) in childrenAndHeights)
        {
            maxTime = Math.Max(maxTime, iIndex + hValue);
            iIndex++;
        }

        // Add 1 for the call from 'node' to first child
        height[node] = 1 + maxTime;
        return height[node];
    }

    /// <summary>
    /// Constructs the actual sequence of calls in an optimal order.
    /// The node calls its children in descending order of sub-tree height.
    /// </summary>
    /// <param name="node">The node whose calls we are scheduling.</param>
    /// <param name="hierarchy">The oriented graph representing the call tree.</param>
    /// <param name="height">A dictionary containing each node's sub-tree height.</param>
    /// <param name="schedule">The resulting list of calls (from -> to) is appended here.</param>
    private static void ConstructSchedule(Vertex node, OrientedGraph hierarchy, Dictionary<Vertex, int> height, List<(Vertex from, Vertex to)> schedule)
    {
        // If no children, nothing to do
        var children = hierarchy.GetAdjacentEdges(node);
        if (children.Count is 0)
            return;

        // Sort children in descending order of sub-tree height
        var childrenSorted = new List<Vertex>(children);
        childrenSorted.Sort((a, b) => height[b].CompareTo(height[a]));

        // For each child in sorted order, schedule the call, then recurse
        foreach (var child in childrenSorted)
        {
            schedule.Add((node, child));
            ConstructSchedule(child, hierarchy, height, schedule);
        }
    }
}
