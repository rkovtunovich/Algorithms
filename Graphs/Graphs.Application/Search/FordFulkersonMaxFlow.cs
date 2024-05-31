using Graphs.Core;
using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

public static class FordFulkersonMaxFlow
{
    // The Ford-Fulkerson algorithm calculates the maximum flow in a flow network.
    // It iteratively looks for augmenting paths from the source to the target in the residual graph and adds the flow along these paths to the total flow.
    // The process continues until no augmenting paths can be found.
    public static double AugmentingPathSearch(GraphBase graph, Vertex source, Vertex target)
    {
        double maxFlow = 0; // Initialize the max flow to 0.

        var residualGraph = graph.Clone(); // Create a copy of the graph as a residual graph.

        while (true)
        {
            // Search for an augmenting path in the residual graph.
            var (isExist, path) = SearchPath(residualGraph, source, target);

            if (!isExist)
                break; // If no augmenting path is found, exit the loop.

            double curFlow = 0; // Initialize the current flow for this path.
            var current = target;

            // Traverse the path backward to find the bottleneck value.
            while (true)
            {
                var parent = path[current.ArrayIndex()];
                var currLength = residualGraph.GetEdgeLength(parent, current);
                if (curFlow == 0)
                    curFlow = currLength;
                else
                    curFlow = Math.Min(curFlow, currLength);

                current = parent;
                if (current == source)
                    break;
            }

            current = target;

            // Update the residual graph based on the bottleneck value.
            while (true)
            {
                var parent = path[current.ArrayIndex()];

                // Update forward and backward edges.
                residualGraph.ChangeEdgeLength(parent, current, -curFlow);
                residualGraph.ChangeEdgeLength(current, parent, curFlow);

                current = parent;
                if (current == source)
                    break;
            }

            maxFlow += curFlow; // Increment the max flow with the flow of the current path.

            DOTVisualizer.VisualizeGraph(residualGraph); // Visualize the updated residual graph.
        }

        return maxFlow; // Return the computed max flow.
    }

    public static (bool isExist, Vertex[] path) SearchPath(GraphBase graph, Vertex source, Vertex target)
    {
        var parents = new Vertex[graph.Count()]; // To keep track of the parent of each vertex.

        var visited = new HashSet<Vertex>
        {
            source // Mark the source as visited.
        };

        var queue = new Queue<Vertex>(); // Queue for BFS traversal.
        queue.Enqueue(source);

        // BFS loop.
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var edges = graph.GetAdjacentEdges(current);

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue; // Skip already visited vertices.

                parents[edge.ArrayIndex()] = current; // Record the parent of the current vertex.

                if (edge == target)
                    return (true, parents); // Path found, return the result.

                visited.Add(edge); // Mark the vertex as visited.
                queue.Enqueue(edge); // Add the vertex to the queue for further traversal.
            }
        }

        return (false, parents); // Return false if no path is found.
    }

}
