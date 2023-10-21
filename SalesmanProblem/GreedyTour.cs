using DataStructures.HashTables;
using DataStructures.Lists;
using Graphs.Core.Abstraction;
using Graphs.Core.Model;

namespace SalesmanProblem;

public static class GreedyTour
{
    public static SequentialList<Vertex> Build(Graph graph)
    {
        var visited = new SimpleHashSet<Vertex>();
        var tour = new SequentialList<Vertex>(graph.Count);

        var current = graph.First();
        tour.Add(current);
        visited.Add(current);

        for(int i = 0; i < graph.Count; i++)
        {
            var closest = GetClosestNonVisitedNeighbor(graph, current, visited);
            
            if(closest is not null)
            {
                tour.Add(closest);
                visited.Add(closest);
                current = closest;
            }           
        }

        return tour;
    }

    public static Vertex? GetClosestNonVisitedNeighbor(Graph graph, Vertex vertex, SimpleHashSet<Vertex> visited)
    {
        Vertex? closest = null;

        var edges = graph.GetEdges(vertex);

        double bestLength = double.MaxValue;

        foreach (var edge in edges)
        {
            if (visited.Contains(edge))
                continue;

            var length = graph.GetEdgeLength(vertex, edge);

            if (length < bestLength)
            {
                bestLength = length;
                closest = edge;
            }
        }

        return closest;
    }
}
