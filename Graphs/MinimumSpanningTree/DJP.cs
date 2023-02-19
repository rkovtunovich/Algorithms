using DataStructures.HashTables;
using DataStructures.Heaps;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.MinimumSpanningTree;

// Dijkstra - Jarník's - Prim algorithm
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

        tree.AddVertice(start);
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

    //public static Graph GetMST(Graph graph)
    //{
    //    var complited = new SimpleHashSet<Vertice>();
    //    var minHeap = new HeapMin<double, (Vertice begin, Vertice end)>();
    //    var tree = new UndirectedVariableEdgeLengthGraph("MST");

    //    var start = graph.First();

    //    tree.AddVertice(start);
    //    complited.Add(start);

    //    AddEdgesToHeap(minHeap, graph, start, complited);

    //    while (!minHeap.Empty())
    //    {
    //        var cheapestEdge = minHeap.ExtractNode();
    //        if (complited.Contains(cheapestEdge.Value.end))
    //            continue;

    //        tree.AddConnection(cheapestEdge.Value.begin, cheapestEdge.Value.end);
    //        tree.SetEdgeLength(cheapestEdge.Value.begin, cheapestEdge.Value.end, cheapestEdge.Key);
    //        tree.SetEdgeLength(cheapestEdge.Value.end, cheapestEdge.Value.begin, cheapestEdge.Key);
    //        complited.Add(cheapestEdge.Value.end);

    //        AddEdgesToHeap(minHeap, graph, cheapestEdge.Value.end, complited);
    //    }

    //    return tree;
    //}

    private static void AddEdgesToHeap(HeapMin<double, (Vertex, Vertex)> heapMin, Graph graph, Vertex vertice, SimpleHashSet<Vertex> complited)
    {
        foreach (var edge in graph.GetEdges(vertice))
        {
            if (complited.Contains(edge))
                continue;

            heapMin.Insert(graph.GetEdgeLength(vertice, edge), (vertice, edge));
        }
    }
}
