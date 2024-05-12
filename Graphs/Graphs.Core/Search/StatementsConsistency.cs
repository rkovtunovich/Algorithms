using DataStructures.HashTables;
using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Search;

public static class StatementsConsistency
{
    // We have n objects and m judgments (same or different). 
    // Objects can be divided into two classes (A and B).
    // Each judgment is between two objects.
    // We want to know if the judgments are consistent.

    // 1. Create a Graph: Create a graph with 2n vertices, representing both classes A and B for each object. Use two vertices for each object, one for class A and one for class B.
    //  For each judgment:       
    //      a. If the judgment between i and j is "same", add edges between Ai and Aj, also Bi and Bj.
    //      b. If the judgment between i and j is "different", add edges between Ai and Bj, also Bi and Aj.
    // 2. Initialize Visited collection: Initialize as empty.
    // 3. DFS Traversal: For each vertex in the graph, if it is not visited, perform a DFS traversal.
    //      a. If the current vertex is already visited, return true.
    //      b. Add the current vertex to the visited collection.
    //      c. For each neighbor of the current vertex, perform a DFS traversal.
    // 4. Check Consistency: If the graph is consistent, return true. Otherwise, return false.
    public static bool IsConsistent(Dictionary<(int, int), string> judgments, int count)
    {
        var graph = new UndirectedGraph("judgments");

        // Step 1: Create Graph
        for (int i = 1; i <= count; i++)
        {
            graph.AddVertex(new(i));
            graph.AddVertex(new(i + count));
        }

        foreach (var judgment in judgments)
        {
            var i = judgment.Key.Item1;
            var j = judgment.Key.Item2;
            var label = judgment.Value;

            if (label == "same")
            {
                graph.AddEdge(new(i), new(j));
                graph.AddEdge(new(i + count), new(j + count));
            }
            else // different
            {
                graph.AddEdge(new(i), new(j + count));
                graph.AddEdge(new(i + count), new(j));
            }
        }

        // Step 2: Initialize Visited Array
        var visited = new SimpleHashSet<Vertex>();

        // Step 3: DFS Traversal
        bool dfs(Vertex node)
        {

            if (visited.Contains(node))
                return true;

            visited.Add(node);
            foreach (var neighbor in graph.GetAdjacentEdges(node))
            {
                // Check for inconsistency
                if (node.Index % 2 == neighbor.Index % 2 && judgments.ContainsKey((node.Index / 2, neighbor.Index / 2)) && judgments[(node.Index / 2, neighbor.Index / 2)] == "different")
                    return false;

                if (!dfs(neighbor))
                    return false;
            }
            return true;
        }

        // Step 4: Check Consistency
        foreach (var vertex in graph)
        {
            if (!visited.Contains(vertex) && !dfs(vertex))
                return false; // Inconsistent          
        }

        return true; // Consistent
    }
}
