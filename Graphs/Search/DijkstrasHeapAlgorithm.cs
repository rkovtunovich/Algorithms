﻿using DataStructures.Heaps;
using Graphs.Abstraction;
using Graphs.Model;

namespace Graphs.Search;

public static class DijkstrasHeapAlgorithm
{
    public static void Search(Graph graph, Vertex origin)
    {
        int marcked = 0;

        origin.Distance = 0;
        origin.Mark = true;
        origin.Label = marcked.ToString();

        var heap = new HeapMin<double, Vertex>();
        foreach (var vertice in graph)
        {
            heap.Insert(vertice.Distance ?? throw new ArgumentNullException($"Distanse isn't initialized for vertice {vertice.Index}"), vertice);
        }

        while (heap.Length > 0)
        {
            Vertex closest = heap.ExtractValue();

            if (closest.Distance is int.MaxValue)
                continue;

            marcked++;
            closest.Mark = true;
            closest.Label = $"-={marcked}=-";

            MarckClosestNeighbors(graph, closest, heap);
        }
    }

    private static void MarckClosestNeighbors(Graph graph, Vertex vertice, HeapMin<double, Vertex> heap)
    {
        var edgesClosestVertice = graph.GetEdges(vertice);

        foreach (var edge in edgesClosestVertice)
        {
            if (edge.Mark)
                continue;

            double dist = (vertice.Distance ?? 0) + graph.GetEdgeLength(vertice, edge);

            if (edge.Distance is null || edge.Distance > dist)
            {
                edge.Distance = dist;
                heap.ReplaceKeyByValue(edge, dist);
            }             
        }
    }
}