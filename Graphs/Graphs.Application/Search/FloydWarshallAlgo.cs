using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

// The Floyd-Warshall algorithm is an all-pairs shortest path algorithm used to find the shortest path between every pair of nodes in a graph.
// It is a dynamic programming algorithm that works for both directed and undirected graphs with positive or negative edge weights, as long as there are no negative cycles.
// 
// The main idea behind the Floyd-Warshall algorithm is to iteratively update the distance between every pair of nodes (i, j)
// considering the possibility of going through an intermediate node k. The algorithm uses a matrix D to store the distances between all pairs of nodes,
// which is initialized with the direct edge weights (or infinity if there is no direct edge).
// 
// Here's a step-by-step description of the Floyd-Warshall algorithm:
// 
// 1. Initialize the distance matrix D with the direct edge weights, and set the diagonal elements (i, i) to 0, as the distance from a node to itself is always 0.
// 2. For each intermediate node k from 1 to the number of nodes (n):
//      a. For each pair of nodes (i, j):
//          * Calculate the distance between nodes i and j through the intermediate node k (i.e., D[i][k] + D[k][j]).
//          * If this new distance is smaller than the current distance D[i][j], update the distance D[i][j] with the new value.
// 3. After the outer loop completes, the distance matrix D contains the shortest path distances between all pairs of nodes.
//
// The time complexity of the Floyd-Warshall algorithm is O(V^3), where V is the number of vertices in the graph.
// This is because the algorithm has three nested loops, each iterating over all the nodes in the graph. The space complexity is O(V^2),
// as the algorithm needs to store the distance matrix D.
// 
// One limitation of the Floyd-Warshall algorithm is that it cannot handle graphs with negative cycles,
// as the presence of a negative cycle would cause the algorithm to enter an infinite loop, continually updating the distances.
// However, it can handle graphs with negative edge weights as long as there are no negative cycles.
// 
// In summary, the Floyd-Warshall algorithm is an all-pairs shortest path algorithm used to find the shortest paths between every pair of nodes in a graph.
// It is a dynamic programming algorithm that works for both directed and undirected graphs with positive or negative edge weights, as long as there are no negative cycles.
// The time complexity of the Floyd-Warshall algorithm is O(V^3), and the space complexity is O(V^2).


public static class FloydWarshallAlgo
{
    private const int MaxPossibleLength = 100;

    public static void Search(OrientedGraph graph)
    {
        var span = new int[graph.Count + 1][][];

        for (int v = 0; v < graph.Count; v++)
        {
            span[v] = new int[graph.Count][];

            for (int w = 0; w < graph.Count; w++)
            {
                span[v][w] = new int[graph.Count];

                var vertex_V = graph.GetVertexByIndex(v + 1);
                var vertex_W = graph.GetVertexByIndex(w + 1);

                if (v == w)
                    span[0][v][w] = 0;
                else if (graph.IsConnected(vertex_V, vertex_W))
                    span[0][v][w] = (int)graph.GetEdgeLength(vertex_V, vertex_W);
                else
                    span[0][v][w] = MaxPossibleLength;
            }
        }

        span[graph.Count] = new int[graph.Count][];
        for (int i = 0; i < graph.Count; i++)
        {
            span[graph.Count][i] = new int[graph.Count];
        }

        for (int k = 1; k <= graph.Count; k++)
        {
            for (int v = 0; v < graph.Count; v++)
            {
                for (int w = 0; w < graph.Count; w++)
                {
                    span[k][v][w] = Math.Min(span[k - 1][v][w], span[k - 1][v][k - 1] + span[k - 1][k - 1][w]);
                }
            }
        }

        for (int v = 0; v < graph.Count; v++)
        {
            if (span[graph.Count][v][v] < 0)
                Console.WriteLine("Graph has a negative cycle");
        }
    }
}

