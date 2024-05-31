using DataStructures.Lists;
using Graphs.Core;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.MinimumArborescencesTree;

public static class EdmondsAlgorithm
{
    // An arborescence is a directed graph (a directed spanning tree of minimum cost) in which,
    // for a vertex u (called the root), there is exactly one directed path from u to any other vertex in the graph.
    // This concept is closely related to trees in undirected graphs, but it applies to directed graphs. 
    public static (OrientedGraph tree, double minimumCost) FindArborescencesTree(OrientedGraph graph)
    {
        var modifiedGraph = graph.Clone() as OrientedGraph ?? throw new NullReferenceException();
        var tree = new OrientedGraph("arborescences_Tree", true);

        // Step 1: Initialize. Add all vertices to the tree
        foreach (var vertex in modifiedGraph)
        {
            tree.AddVertex(vertex);
        }

        // Step 2: Find the root of the graph
        var root = modifiedGraph.FindRoot();

        // for visualization
        DOTVisualizer.VisualizeGraph(tree);

        // Step 4: Contract Phase
        var cycles = new Dictionary<Vertex, SequentialList<Vertex>>();
        var deletedVertices = new SequentialList<Vertex>();
        var deletedEdges = new SequentialList<(Vertex, Vertex, double)>();

        double minimumCost;
        while (true)
        {
            // Step 3: For each subtree, keep 1 incoming edge with the minimum weight.
            minimumCost = TryGetMinimumCostTree(modifiedGraph, tree, root);
            var cycle = tree.SearchCycle();

            DOTVisualizer.VisualizeGraph(tree);

            if (cycle is [])
                break;

            var contractedVertex = modifiedGraph.CreateVertex();
            tree.AddVertex(contractedVertex);
            ContractCycle(modifiedGraph, cycle, deletedVertices, deletedEdges, contractedVertex);
            cycles.Add(contractedVertex, cycle);
        }

        // Step 4: Expansion Phase
        // Expand from the last contracted cycle to the first
        foreach (var entry in cycles.Reverse())
        {
            ExpandCycle(modifiedGraph, tree, entry, deletedVertices, deletedEdges, ref minimumCost);
        }

        return (tree, minimumCost);
    }

    private static double TryGetMinimumCostTree(OrientedGraph graph, OrientedGraph tree, Vertex root)
    {
        tree.CleanEdges();
        var minimumCost = 0d;

        foreach (var vertex in graph)
        {
            if (vertex == root)
                continue;

            (var source, var minWeight) = FindMinimumWeightIncomingEdge(graph, vertex);

            if (source is not null)
            {
                tree.AddEdgeWithLength(source, vertex, minWeight);
                minimumCost += minWeight;
            }
        }

        return minimumCost;
    }

    private static void ContractCycle(OrientedGraph graph,
                                      SequentialList<Vertex> cycle,
                                      SequentialList<Vertex> deletedVertices,
                                      SequentialList<(Vertex, Vertex, double)> deletedEdges,
                                      Vertex contractedVertex)
    {
        graph.AddVertex(contractedVertex);

        foreach (var vertex in cycle)
        {
            if (graph.IncomeEdges.TryGetValue(vertex, out var incomeEdges))
            {
                foreach (var edge in incomeEdges)
                {
                    var length = graph.GetEdgeLength(edge, vertex);
                    deletedEdges.Add((edge, vertex, length));

                    if (cycle.Contains(edge))
                        continue;

                    graph.AddEdge(edge, contractedVertex);

                    var currentLength = graph.GetEdgeLength(edge, contractedVertex);
                    if (currentLength is 0 || currentLength > length)
                        graph.SetEdgeLength(edge, contractedVertex, length);
                }
            }

            var outcomeEdges = graph.GetAdjacentEdges(vertex);
            foreach (var edge in outcomeEdges)
            {
                var length = graph.GetEdgeLength(vertex, edge);
                deletedEdges.Add((vertex, edge, length));

                if (cycle.Contains(edge))
                    continue;

                graph.AddEdge(contractedVertex, edge);

                var currentLength = graph.GetEdgeLength(contractedVertex, edge);
                if (currentLength is 0 || currentLength > length)
                    graph.SetEdgeLength(contractedVertex, edge, length);
            }

            graph.RemoveVertex(vertex);
            deletedVertices.Add(vertex);
        }
    }

    private static void ExpandCycle(OrientedGraph graph,
                                     OrientedGraph tree,
                                     KeyValuePair<Vertex, SequentialList<Vertex>> cycle,
                                     SequentialList<Vertex> deletedVertices,
                                     SequentialList<(Vertex, Vertex, double)> deletedEdges,
                                     ref double minimumCost)
    {
        var contractedVertex = cycle.Key;
        var originalCycle = cycle.Value;

        var current = tree.IncomeEdges[contractedVertex].First();

        while (originalCycle.Count > 0)
        {
            var (destination, weight) = GetMinimumDeletedEdge(current, deletedEdges, originalCycle);
            if (destination is null)
            {
                originalCycle.Remove(current);
                continue;
            }

            tree.AddEdgeWithLength(current, destination, weight);

            // Update the minimum cost
            minimumCost += weight;

            // Remove the edge from the deleted edges list
            deletedEdges.Remove((current, destination, weight));

            // Remove the vertex from the cycle
            originalCycle.Remove(current);

            // Move to the next vertex in the cycle
            current = destination;

            // for visualization
            DOTVisualizer.VisualizeGraph(tree);
        }

        // Remove the contracted vertex from the graph and the tree
        tree.RemoveVertex(contractedVertex);

        foreach (var edge in deletedEdges)
        {
            if (tree.IncomeEdges[edge.Item2].Count > 0)
                continue;

            tree.AddEdgeWithLength(edge.Item1, edge.Item2, edge.Item3);
        }

        DOTVisualizer.VisualizeGraph(tree);
    }

    private static (Vertex? source, double minWeight) FindMinimumWeightIncomingEdge(OrientedGraph graph, Vertex vertex)
    {
        Vertex? source = null;
        var minWeight = double.MaxValue;
        var incomeEdges = graph.IncomeEdges[vertex];

        foreach (var incomingVertex in incomeEdges)
        {
            var weight = graph.GetEdgeLength(incomingVertex, vertex);
            if (weight < minWeight)
            {
                minWeight = weight;
                source = incomingVertex;
            }
        }

        return (source, minWeight);
    }

    private static (Vertex destination, double weight) GetMinimumDeletedEdge(Vertex source, SequentialList<(Vertex, Vertex, double)> deletedEdges, SequentialList<Vertex> cycle)
    {
        var minWeight = double.MaxValue;
        Vertex minWeightVertex = null;

        foreach (var (src, dest, weight) in deletedEdges)
        {
            if (src != source)
                continue;

            if (!cycle.Contains(dest))
                continue;

            if (weight < minWeight)
            {
                minWeight = weight;
                minWeightVertex = dest;
            }
        }

        return (minWeightVertex, minWeight);
    }
}
