using DataStructures.HashTables;
using DataStructures.Heaps;
using Graphs.GraphImplementation;

namespace Graphs.MinimumSpanningTree;

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
    public static (Graph tree, double length) GetMST(Graph graph)
    {
        var complited = new SimpleHashSet<Vertex>();
        var minHeap = new HeapMin<double, Vertex>();
        var tree = new UndirectedVariableEdgeLengthGraph("MST");
        var treeEdges = new Dictionary<Vertex, (Vertex vertice, double length)>();
        double totalLength = 0;

        var start = graph.First();

        tree.AddVertex(start);
        complited.Add(start);

        var edges = new SimpleHashSet<Vertex>();
        edges.Load(graph.GetEdges(start));
        foreach (var vertice in graph)
        {
            if (vertice.Equals(start))
                continue;

            if (edges.Contains(vertice))
            {
                var length = graph.GetEdgeLength(start, vertice);
                minHeap.Insert(length, vertice);
                treeEdges.Add(vertice, (start, length));
            }
            else
            {
                minHeap.Insert(int.MaxValue, vertice);
            }
        }

        while (!minHeap.Empty())
        {
            var closest = minHeap.ExtractNode();
            complited.Add(closest.Value);

            if (closest.Key is int.MaxValue)
                break;

            tree.AddConnection(treeEdges[closest.Value].vertice, closest.Value);
            tree.SetEdgeLength(treeEdges[closest.Value].vertice, closest.Value, closest.Key);
            tree.SetEdgeLength(closest.Value, treeEdges[closest.Value].vertice, closest.Key);
            totalLength += closest.Key;

            foreach (var vertice in graph.GetEdges(closest.Value))
            {
                if (complited.Contains(vertice))
                    continue;

                var length = graph.GetEdgeLength(closest.Value, vertice);

                if (treeEdges.TryGetValue(vertice, out var edge))
                {
                    if(edge.length > length)
                    {
                        treeEdges[vertice] = (closest.Value, length);
                        minHeap.ReplaceKeyByValue(vertice, length);
                    }
                }
                else
                {               
                    treeEdges.Add(vertice, (closest.Value, length));
                    minHeap.ReplaceKeyByValue(vertice, length);
                }           
            }
        }

        return (tree, totalLength);
    }
}
