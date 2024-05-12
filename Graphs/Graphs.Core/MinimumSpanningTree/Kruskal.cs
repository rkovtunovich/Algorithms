using DataStructures.Common.UnionFinds;
using Graphs.Core.Model.Graphs;

namespace Graphs.Core.MinimumSpanningTree;

// Kruskal's algorithm is greedy method for finding the minimum spanning tree (MST) of a connected, undirected graph with weighted edges.
// The main idea behind Kruskal's algorithm is to sort all the edges in the graph by weight and then add edges to the MST one by one, as long as they don't create cycles.
// 
// Here's a step-by-step description of Kruskal's algorithm:
// 
// 1. Create a list of all edges in the graph.
// 2. Sort the edges in non-decreasing order by their weights.
// 3. Initialize an empty set (or forest) to store the edges of the MST.
// 4. Iterate through the sorted edges:
//      a. Pick the edge with the smallest weight that hasn't been considered yet.
//      b. Check if adding this edge to the MST would create a cycle.
//          If adding the edge does not create a cycle, add it to the MST.
//          If adding the edge would create a cycle, discard it and move to the next edge in the sorted list.
//5. Continue iterating through the edges until the MST includes all the vertices or all the edges have been considered.
//
// To efficiently check if adding an edge would create a cycle, Kruskal's algorithm typically employs a disjoint-set data structure (also known as a union-find data structure).
// This data structure allows for efficiently checking if two vertices belong to the same set and for merging two sets.
// 
// The time complexity of Kruskal's algorithm mainly depends on the time it takes to sort the edges.
// If a comparison-based sorting algorithm like quicksort or mergesort is used, the time complexity is O(E log E), where E is the number of edges in the graph.
// Since E can be at most V^2 (where V is the number of vertices), the complexity can also be expressed as O(E log V).
// The union-find operations required to check for cycles and merge sets are usually quite efficient and do not significantly impact the overall time complexity.
// 
// In summary, Kruskal's algorithm is a greedy method for finding the MST of a connected, undirected graph by iteratively adding edges in non-decreasing order of their weights,
// as long as they don't create cycles. The algorithm has a time complexity of O(E log E) or O(E log V), depending on the graph's structure.
// Kruskal’s algorithm is suitable for sparse graph
//
//Also it is possible using raising clustering 

public static class Kruskal
{
    public static (GraphBase tree, double length) GetMinimumST(UndirectedVariableEdgeLengthGraph graph)
    {
        var tree = new UndirectedVariableEdgeLengthGraph("MST_Kruskal_min");
        var unionFind = new UnionFind<Vertex>([.. graph]);
        var heapMin = new HeapMin<double, (Vertex, Vertex)>();
        double length = 0;

        foreach (var edge in graph.GetEdgesLength())
        {
            heapMin.Insert(edge.Value, edge.Key);
        }

        while (!heapMin.Empty())
        {
            var edge = heapMin.ExtractNode();

            var first = unionFind.Find(edge.Value.Item1);
            var second = unionFind.Find(edge.Value.Item2);

            if (first.ParentIndex != second.ParentIndex)
            {
                tree.AddConnection(edge.Value.Item1, edge.Value.Item2);
                tree.SetEdgeLength(edge.Value.Item1, edge.Value.Item2, edge.Key);
                tree.SetEdgeLength(edge.Value.Item2, edge.Value.Item1, edge.Key);

                length += edge.Key;
            }

            unionFind.Union(edge.Value.Item1, edge.Value.Item2);
        }

        return (tree, length);
    }

    public static (GraphBase tree, double length) GetMaximumST(UndirectedVariableEdgeLengthGraph graph)
    {
        var tree = new UndirectedVariableEdgeLengthGraph("MST_Kruskal_max");
        var unionFind = new UnionFind<Vertex>([.. graph]);
        var heapMax = new HeapMax<double, (Vertex, Vertex)>();
        double length = 0;

        foreach (var edge in graph.GetEdgesLength())
        {
            heapMax.Insert(edge.Value, edge.Key);
        }

        while (!heapMax.Empty())
        {
            var edge = heapMax.ExtractNode();

            var first = unionFind.Find(edge.Value.Item1);
            var second = unionFind.Find(edge.Value.Item2);

            if (first.ParentIndex != second.ParentIndex)
            {
                tree.AddConnection(edge.Value.Item1, edge.Value.Item2);
                tree.SetEdgeLength(edge.Value.Item1, edge.Value.Item2, edge.Key);
                tree.SetEdgeLength(edge.Value.Item2, edge.Value.Item1, edge.Key);

                length += edge.Key;
            }

            unionFind.Union(edge.Value.Item1, edge.Value.Item2);
        }

        return (tree, length);
    }
}
