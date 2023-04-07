﻿using Graphs.Abstraction;
using Graphs.Model;

namespace Graphs.Search;

public static class DijkstrasAlgorithm
{
    public static void Search(Graph graph, Vertex origin)
    {
        int marcked = 0;
        marcked++;

        origin.Distance = 0;
        origin.Mark = true;
        origin.Label = marcked.ToString(); ;
        MarckClosestNeighbors(graph, origin);

        while (marcked < graph.Count())
        {
            Vertex? closest = GetNextClosestVertice(graph);

            if (closest is null)
            {
                marcked++;
                continue;
            }

            marcked++;
            closest.Mark = true;
            closest.Label = marcked.ToString();

            MarckClosestNeighbors(graph, closest);
        }
    }

    private static Vertex? GetNextClosestVertice(Graph graph)
    {
        Vertex? closest = null;

        foreach (var vertice in graph)
        {
            if (!(vertice?.Mark ?? false))
                continue;

            var edges = graph.GetEdges(vertice);

            foreach (var edge in edges)
            {
                if (vertice.Mark && edge.Mark)
                    continue;

                closest ??= edge;

                if (closest.Distance > edge.Distance)
                    closest = edge;
            }
        }

        return closest;
    }

    private static void MarckClosestNeighbors(Graph graph, Vertex vertice)
    {
        var edgesClosestVertice = graph.GetEdges(vertice);

        foreach (var edge in edgesClosestVertice)
        {
            if (edge.Mark)
                continue;

            double dist = (vertice.Distance ?? 0) + graph.GetEdgeLength(vertice, edge);

            if (edge.Distance is null || edge.Distance > dist)
                edge.Distance = dist;
        }
    }
}