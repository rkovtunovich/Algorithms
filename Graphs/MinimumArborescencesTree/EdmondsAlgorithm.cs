using Graphs.GraphImplementation;

namespace Graphs.MinimumArborescencesTree;

public static class EdmondsAlgorithm
{
    public static (OrientedGraph tree, double minimumCost) FindArborescencesTree(OrientedGraph graph)
    {
        var minimumCost = 0.0;

        var tree = new OrientedGraph("arborescences_Tree");

        // Step 1: Initialize. Add all vertices to the tree
        foreach (var vertex in graph)
        {
            tree.AddVertex(vertex);
        }

        // Step 2: Find the root of the graph
        var root = graph.FindRoot();

        // Step 3: Contract Phase
        var cycles = new List<List<Vertex>>();

        var cycle = graph.SearchCycle();

        foreach (var vertex in graph)
        {
            if (vertex == root) continue;

            var closestNeighbor = graph.GetClosestNeighbor(vertex);
            if (closestNeighbor is not null)
            {
                tree.AddEdge(closestNeighbor, vertex);
                minimumCost += graph.GetEdgeLength(closestNeighbor, vertex);
            }
        }

        return (tree, minimumCost);
    }
    
    private static List<List<Vertex>> DetectCycles(OrientedGraph graph)
    {
        var cycles = new List<List<Vertex>>();

        foreach (var vertex in graph)
        {
            var cycle = new List<Vertex>();
            var current = vertex;

            while (true)
            {
                cycle.Add(current);
                var belowNeighbors = graph.GetBelowNeighbors(current);

                if (belowNeighbors.Count == 0) break;

                current = belowNeighbors[0];
                if (cycle.Contains(current))
                {
                    var cycleStartIndex = cycle.IndexOf(current);
                    var cycleLength = cycle.Count - cycleStartIndex;
                    var cycleVertices = cycle.GetRange(cycleStartIndex, cycleLength);
                    cycles.Add(cycleVertices);
                    break;
                }
            }
        }

        return cycles;
    }
}
