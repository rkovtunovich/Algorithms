﻿using Graphs.Abstraction;

namespace Graphs.GraphImplementation;

public class OrientedGraph<T> : Graph<T>
{
    public OrientedGraph(string name)
    {
        Name = name;
    }

    public override void AddEdge(Vertice<T> sourse, Vertice<T> destination)
    {
        if (!_nodes.ContainsKey(sourse) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the garph!");

        var sourceEdges = _nodes[sourse];
        AddConnection(sourceEdges, destination);
    }

    public override bool IsOriented()
    {
        return true;
    }
}