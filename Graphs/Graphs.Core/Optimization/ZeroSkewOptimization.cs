using DataStructures.Lists;

namespace Graphs.Core.Optimization;

public static class ZeroSkewOptimization
{
    // Zero Skew with Minimal Total Edge Length
    // 
    // 1. Tree Structure Assumptions: Assume a complete binary tree. Nodes are labeled systematically from the root down to the leaves.
    // 
    // 2. Calculate Distances**:
    //    - DFS or BFS Traversal: Use Depth-First Search (DFS) or Breadth-First Search (BFS) to traverse from the root to all leaves.
    //    - Distance Calculation: During traversal, calculate the total distance (path length) from the root to each leaf and store these distances.
    // 
    // 3. Identify Maximum Leaf Distance**:
    //    - After calculating the distances for all leaves, identify the maximum distance from the root to any leaf. This will be the target distance for all leaves to ensure zero skew.
    // 
    // 4. Adjust Edge Lengths:
    //    - Traverse the tree again. For each path from the root to a leaf, compare the path's total distance to the maximum distance.
    //    - Increment Edge Lengths: If a path's distance is less than the maximum, increase the length of the edges on this path. It's crucial to distribute this increase along the edges of the path in a manner that minimizes the total edge length increase across the tree.
    //    - Optimal Edge Length Increase: Instead of arbitrarily increasing the first edge or any single edge, distribute the required increase to potentially adjust edges shared by multiple paths first. This is because increasing the length of an edge closer to the root affects more paths, potentially minimizing the total increase required.
    // 
    // 5. Minimize Total Increase:
    //    - Shared Edges: Focus on increasing edges higher in the tree that impact multiple leaf paths simultaneously, thus minimizing the total increase in edge lengths required to achieve zero skew.
    //    - Re-calculate if Needed: After increasing edge lengths, it may be necessary to recalculate distances from the root to all leaves to ensure all are equal to the maximum distance.
    // 
    // 6. Edge Increase Strategy:
    //    - Ideally, the increments to edge lengths should be made in such a way that edges closer to the root are considered for increment first, as their adjustment affects multiple leaf distances. This approach leverages the inherent structure of the binary tree to minimize total edge length additions.
    // 
    // 7. Implementation Considerations**:
    //    - Data Structures: Utilize a data structure (like a list or array) to store the path distances and a tree structure that allows easy modification of edge values.
    //    - Efficiency: Ensure that the traversal and modification steps are efficient, considering the potential size of the tree.

    // 1. Mark Maximum Length Edges: Start by finding the leaf with the longest path. As you backtrack to the root from this leaf, mark the edges on this path as having the maximum length.
    // 2. Select Next Longest Path: Find the next longest path that is not already marked as maximum.
    // 3. Find Closest Common Ancestor: For the next longest unmarked path, identify the closest common ancestor with any of the previously marked maximum paths.
    //    This is the key part of the strategy, as incrementing the path length from this common ancestor will efficiently increase the length of all paths that share this portion of the tree.
    // 4. Increment Edges Efficiently: Increase the length of the edges on this path up to the common ancestor to match the longest path.
    //    Any edges from the common ancestor towards the root that were already at maximum should not be incremented further.
    // 5. Recalculate and Repeat: After incrementing the edges for the current path, recalculate the lengths of all unmarked paths.
    //    Repeat the process by selecting the next longest path and incrementing as necessary until all leaf nodes have paths of equal length to the longest path.
    // 6. Final Check for Zero Skew: Once all leaves have the same path length from the root, ensure that no further increments are possible without increasing the overall length unnecessarily.

    public static void Optimize(Graph graph, Vertex root, bool useDebugMode = false)
    {
        root.Label = "0: MAX";
        int maxDistance = -1;

        var distances = new Dictionary<Vertex, (Vertex parent, int distance, bool isMaxLength)>
        {
            { root, (root, 0, true) }
        };

        // Initialize distances
        foreach (var vertex in graph)
        {
            if (vertex == root)
                continue;

            distances.Add(vertex, (root, -1, false));
        }

        var needCalculation = true;
        var leaves = new HeapMax<int, Vertex>();

        while (true)
        {
            // 1. Calculate Distances
            if (needCalculation)
            {
                CalculateDistances(graph, root, ref distances, out leaves);

                if (useDebugMode)
                    DOTVisualizer.VisualizeGraph(graph);
            }

            if (leaves.Length is 0)
                break;

            // 2. Adjust Edge Lengths
            var node = leaves.ExtractNode();
            var leaf = node.Value;

            if (maxDistance is -1)
                maxDistance = node.Key;

            if (node.Key == maxDistance)
            {
                needCalculation = false;
            }
            else
            {
                IncreaseDistanceToLeaf(graph, distances, leaf, maxDistance - node.Key);
                needCalculation = true;

                if (useDebugMode)
                    DOTVisualizer.VisualizeGraph(graph);
            }

            // 3. Mark Maximum Length Edges
            MarkMaximumLengthEdges(leaf, distances);

            if (useDebugMode)
                DOTVisualizer.VisualizeGraph(graph);
        }
    }

    private static void CalculateDistances(Graph graph, Vertex root, ref Dictionary<Vertex, (Vertex parent, int distance, bool isMaxLength)> distances, out HeapMax<int, Vertex> leaves)
    {
        leaves = new();

        var visited = new HashSet<Vertex>();
        var stack = new SequentialStack<Vertex>(graph.Count);
        stack.Push(root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            visited.Add(current);

            var edges = graph.GetAdjacentEdges(current);

            if (edges.Count is 1 && !IsMaxLength(distances, current))
            {
                leaves.Insert(distances[current].distance, current);
                continue;
            }

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue;

                var distance = distances[current].distance + (int)graph.GetEdgeLength(current, edge);

                edge.Label = distance.ToString();
                distances[edge] = (current, distance, false);

                stack.Push(edge);
            }
        }
    }

    private static void IncreaseDistanceToLeaf(Graph graph, Dictionary<Vertex, (Vertex parent, int distance, bool isMaxLength)> distances, Vertex leaf, int difference)
    {
        var firstForIncreasing = leaf;
        Vertex? parent;

        // Backtrack to the first node with maxLength
        while (true)
        {
            parent = distances[firstForIncreasing].parent;

            if (distances[parent].isMaxLength)
                break;

            firstForIncreasing = parent;
        }

        var edgeLength = graph.GetEdgeLength(parent, firstForIncreasing);
        graph.SetEdgeLength(parent, firstForIncreasing, edgeLength + difference);
        graph.SetEdgeLength(firstForIncreasing, parent, edgeLength + difference);

        var distance = distances[firstForIncreasing].distance + difference;
        distances[firstForIncreasing] = (parent, distance, true);
        firstForIncreasing.Label = $"{distance}: MAX";
    }

    private static void MarkMaximumLengthEdges(Vertex? leaf, Dictionary<Vertex, (Vertex parent, int distance, bool isMaxLength)> distances)
    {
        var current = leaf;
        while (true)
        {
            var node = distances[current];
            if (node.isMaxLength)
                break;

            distances[current] = (node.parent, node.distance, true);
            current.Label = $"{node.distance}: MAX";
            current = distances[current].parent;
        }
    }

    private static bool IsMaxLength(Dictionary<Vertex, (Vertex parent, int distance, bool isMaxLength)> distances, Vertex vertex)
    {
        return distances[vertex].isMaxLength;
    }
}