﻿using DataStructures.BTrees;
using Graphs.Core.Abstraction;
using Graphs.Core.Model;
using Graphs.Core.GraphImplementation;

namespace Graphs.Core.Generators;

public class GraphByTwoTheeTreeGenerator<TKey> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly TwoThreeTree<TKey> _tree;

    public GraphByTwoTheeTreeGenerator(TwoThreeTree<TKey> tree)
    {
        _tree = tree;
    }

    public Graph Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        if (_tree.Root is null)
            return graph;

        var vertex = new Vertex(1)
        {
            Label = GetLabel(_tree.Root)
        };
        graph.AddVertex(vertex);

        AddChilds(vertex, graph, _tree.Root);

        return graph;
    }

    private void AddChilds(Vertex parent, Graph graph, BTreeNode<TKey> currNode)
    {
        if (!currNode.HasChildren)
            return;

        foreach (var node in currNode.Children)
        {
            var vertex = new Vertex(graph.Count() + 1)
            {
                Label = GetLabel(node)
            };

            graph.AddVertex(vertex);
            graph.AddEdge(parent, vertex);

            AddChilds(vertex, graph, node);
        }
    }

    private string GetLabel(BTreeNode<TKey>? node)
    {
        if (node is null)
            return string.Empty;

        return $"{string.Join(",", node.Keys)}";
    }
}