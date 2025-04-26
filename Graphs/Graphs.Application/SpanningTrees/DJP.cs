using DataStructures.HashTables.OpenAddressing;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.SpanningTrees;

// Prim's (Jarník's) (Dijkstra - Jarník's - Prim) algorithm is a greedy algorithm that constructs a minimum spanning tree
// by iteratively selecting the edge with the smallest weight that connects a vertex in the current MST to a vertex not yet included in the MST.
// Here's a step-by-step description of the algorithm:
// 
// 1. Start with an arbitrary vertex as the current MST.
// 2. Initialize a set of visited vertices, initially containing only the starting vertex.
// 3. While not all vertices are in the visited set:
//      a. Find the edge with the minimum weight connecting a vertex in the visited set to a vertex outside the visited set.
//      b. Add the selected edge to the MST.
//      c. Add the newly connected vertex to the visited set.
// 4. When all vertices are in the visited set, the MST construction is complete.
//
// The time complexity of Prim's algorithm depends on the data structures used to implement it.
// Using adjacency lists and binary heaps, the time complexity is O(E log V), where E is the number of edges and V is the number of vertices in the graph.
// Other data structures, like Fibonacci heaps, can further improve the time complexity to O(E + V log V).
// Prim’s algorithm algorithm is suitable for dense graph

public static class DJP
{
    public static (GraphBase tree, double length) GetMST(GraphBase graph)
    {
        var completed = new SimpleHashSet<Vertex>();
        var minHeap = new HeapMin<double, Vertex>();
        var tree = new UndirectedVariableEdgeLengthGraph("MST");
        var treeEdges = new Dictionary<Vertex, (Vertex vertice, double length)>();
        double totalLength = 0;

        var start = graph.First();

        tree.AddVertex(start);
        completed.Add(start);

        var edges = new SimpleHashSet<Vertex>();
        edges.Load(graph.GetAdjacentEdges(start));
        foreach (var vertex in graph)
        {
            if (vertex.Equals(start))
                continue;

            if (edges.Contains(vertex))
            {
                var length = graph.GetEdgeLength(start, vertex);
                minHeap.Insert(length, vertex);
                treeEdges.Add(vertex, (start, length));
            }
            else
            {
                minHeap.Insert(int.MaxValue, vertex);
            }
        }

        while (!minHeap.Empty)
        {
            var closest = minHeap.ExtractNode();
            completed.Add(closest.Value);

            if (closest.Key is int.MaxValue)
                break;

            tree.AddConnection(treeEdges[closest.Value].vertice, closest.Value);
            tree.SetEdgeLength(treeEdges[closest.Value].vertice, closest.Value, closest.Key);
            tree.SetEdgeLength(closest.Value, treeEdges[closest.Value].vertice, closest.Key);
            totalLength += closest.Key;

            foreach (var vertex in graph.GetAdjacentEdges(closest.Value))
            {
                if (completed.Contains(vertex))
                    continue;

                var length = graph.GetEdgeLength(closest.Value, vertex);

                if (treeEdges.TryGetValue(vertex, out var edge))
                {
                    if (edge.length > length)
                    {
                        treeEdges[vertex] = (closest.Value, length);
                        minHeap.ReplaceKeyByValue(vertex, length);
                    }
                }
                else
                {
                    treeEdges.Add(vertex, (closest.Value, length));
                    minHeap.ReplaceKeyByValue(vertex, length);
                }
            }
        }

        return (tree, totalLength);
    }
}
