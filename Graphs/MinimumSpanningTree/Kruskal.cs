using DataStructures;
using DataStructures.Heaps;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.MinimumSpanningTree;

// так же можно использвать немного измененную версию для восходящей кластеризации
public static class Kruskal
{
    public static (Graph tree, double length) GetMST(UndirectedVariableEdgeLengthGraph graph)
    {
        var tree = new UndirectedVariableEdgeLengthGraph("MST_Kraskal");
        var unionFind = new UnionFind<Vertex>(graph.ToArray());
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

            if(first.ParentIndex != second.ParentIndex)
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
