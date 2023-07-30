using DataStructures.BTrees;
using Graphs.GraphImplementation;

namespace Graphs.Generators;

public class GraphByBPLusTheeTreeGenerator<TKey, TValue> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly BPlusTree<TKey, TValue> _tree; 

    public GraphByBPLusTheeTreeGenerator(BPlusTree<TKey, TValue> tree)
    {
        _tree = tree;
    }

    public Graph Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        if(_tree.Root is null)
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
        if(!currNode.HasChildren)
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
