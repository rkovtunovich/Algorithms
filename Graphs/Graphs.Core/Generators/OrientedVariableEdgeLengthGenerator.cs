﻿using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Generators;

public class OrientedVariableEdgeLengthGenerator : IGraphGenerator
{
    private static readonly Random _random = new();
    private readonly int _countVertices;
    private readonly double _saturation;
    private readonly bool _trackIncomeEdges; // if it needs vertex without incoming edges
    private readonly bool _hasNegativeEdges; 

    // if it needs vertex without incoming edges
    private readonly int? _originIndex;

    public OrientedVariableEdgeLengthGenerator(int countVertices, double saturation, int? originIndex = null, bool trackIncomeEdges = false, bool hasNegativeEdges = false)
    {
        _countVertices = countVertices;
        _originIndex = originIndex;
        _saturation = saturation;
        _trackIncomeEdges = trackIncomeEdges;
        _hasNegativeEdges = hasNegativeEdges;
    }

    public GraphBase Generate(string name)
    {
        var graph = new OrientedGraph(name, _trackIncomeEdges);

        for (int i = 1; i <= _countVertices; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var vertex in graph)
        {
            GenerateDirectedConnections(graph, _countVertices, vertex);
        }

        var random = new Random();

        foreach (var vertex in graph)
        {
            var edges = graph.GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                int length = random.Next(1, 10);

                if (_hasNegativeEdges)
                    length *= random.Next(0, 2) == 1 ? 1 : -1;

                graph.SetEdgeLength(vertex, edge, length);
            }
        }

        return graph;
    }


    private void GenerateDirectedConnections(OrientedGraph graph, int countVertices, Vertex owner)
    {
        int numberConnections = _random.Next(0, (int)(countVertices * _saturation));

        var alreadyAdded = new HashSet<Vertex>();

        while (numberConnections > 0)
        {
            int newIndex = _random.Next(1, countVertices);

            if (newIndex == owner.Index)
                continue;

            if (_originIndex == newIndex)
                continue;

            var newConnection = graph.GetVertexByIndex(newIndex) ?? throw new Exception($"graph doesn't contain vertex with index {newIndex}");

            numberConnections--;

            if (graph.IsConnected(newConnection, owner))
                continue;

            graph.AddEdge(owner, newConnection);

            alreadyAdded.Add(newConnection);
        }
    }
}

