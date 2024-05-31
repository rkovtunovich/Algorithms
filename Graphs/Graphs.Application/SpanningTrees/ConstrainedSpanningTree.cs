using DataStructures.Common.UnionFinds;
using DataStructures.Lists;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.SpanningTrees;

public static class ConstrainedSpanningTree
{
    // tree is a spanning tree of G with exactly k edges labeled X
    // and n − k − 1 edges labeled Y, or reports that no such tree exists.
    // edge length as labels X - 1, Y - 2
    // X - k, Y - ( n - k - 1 )
    public static (bool isExist, UndirectedVariableEdgeLengthGraph? tree) GetConstrainedSpanningTree(UndirectedVariableEdgeLengthGraph graph, int k)
    {
        if(k <= 0)
            return (false, null);

        var tree = new UndirectedVariableEdgeLengthGraph("ConstrainedSpanningTree_output");
        var unionFind = new UnionFind<Vertex>([.. graph]);
        var edgesX = new Dictionary<(Vertex vertex1, Vertex vertex2), double>();
        var edgesY = new Dictionary<(Vertex vertex1, Vertex vertex2), double>();

        foreach (var edge in graph.GetEdgesLength())
        {
            if (edge.Value is 1)
                edgesX.Add(edge.Key, edge.Value);
            else
                edgesY.Add(edge.Key, edge.Value);
        }

        // edges are doubled because of undirected graph
        // so we need to divide by 2
        var countX = edgesX.Count / 2;
        var countY = edgesY.Count / 2;

        if (countX < k || countY < graph.Count - k - 1)
            return (false, null);

        var redundancyX = countX - k;
        var redundancyY = countY - (graph.Count - k - 1);

        foreach (var vertex in graph)
        {
            if(countX is 0 && countY is 0)
                break;

            var vertexEdges = graph.GetAdjacentEdges(vertex);

            var first = unionFind.Find(vertex);
            var currentX = new SequentialList<Vertex>();
            var currentY = new SequentialList<Vertex>();

            foreach (var edge in vertexEdges)
            {
                var second = unionFind.Find(edge);

                if (first.ParentIndex == second.ParentIndex)
                    continue;

                if (edgesX.ContainsKey((vertex, edge)))              
                    currentX.Add(edge);          
                else if (edgesY.ContainsKey((vertex, edge)))                
                    currentY.Add(edge);              
            }

            if (redundancyX is 0 && currentX.Count > 0)
            {
                var edge = currentX[0];
                tree.AddEdge(vertex, edge);
                tree.SetEdgeLength(vertex, edge, 1);
                tree.SetEdgeLength(edge, vertex, 1);

                unionFind.Union(vertex, edge);

                countX--;

            }
            else if (redundancyY is 0 && currentY.Count > 0)
            {
                var edge = currentY[0];
                tree.AddEdge(vertex, edge);
                tree.SetEdgeLength(vertex, edge, 2);
                tree.SetEdgeLength(edge, vertex, 2);

                unionFind.Union(vertex, edge);

                countY--;
            }
            else if (currentX.Count > currentY.Count)
            {
                var edge = currentX[0];
                tree.AddEdge(vertex, edge);
                tree.SetEdgeLength(vertex, edge, 1);
                tree.SetEdgeLength(edge, vertex, 1);

                unionFind.Union(vertex, edge);

                redundancyX--;
                countX--;
            }
            else if (currentY.Count >= currentX.Count && currentY.Count > 0)
            {
                var edge = currentY[0];
                tree.AddEdge(vertex, edge);
                tree.SetEdgeLength(vertex, edge, 2);
                tree.SetEdgeLength(edge, vertex, 2);

                unionFind.Union(vertex, edge);

                redundancyY--;
                countY--;
            }
        }

        // check if all vertices are in the same set
        // if not, then there is no spanning tree
        if(unionFind.DistinctSetsCount() > 1)
            return (false, null);
        

        return (true, tree);
    }

    #region Helper methods

    // 1 - X, 2 - Y
    public static void GenerateLabels(GraphBase graph)
    {
        var random = new Random();

        foreach (var vertex in graph)
        {
            foreach (var edge in graph.GetAdjacentEdges(vertex))
            {
                var label = random.Next(1, 3);

                graph.SetEdgeLength(vertex, edge, label);
                graph.SetEdgeLength(edge, vertex, label);
            }
        }
    }

    #endregion
}
