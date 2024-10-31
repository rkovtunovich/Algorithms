using DataStructures.Trees.BinaryTrees;
using DataStructures.Trees.BinaryTrees.Search;
using DataStructures.Trees.BinaryTrees.Search.RedBlack;
using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Generators;

public class GraphRedBlackTreeGenerator<TKey, TValue> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly SearchTree<TKey, TValue> _tree;

    public GraphRedBlackTreeGenerator(SearchTree<TKey, TValue> tree)
    {
        _tree = tree;
    }

    public GraphBase Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        if (_tree.Root is null)
            return graph;

        var vertex = new Vertex(1)
        {
            Label = GetLabel(_tree.Root as RedBlackTreeNode<TKey, TValue>, "O")
        };
        graph.AddVertex(vertex);

        AddChilds(vertex, graph, _tree.Root);

        return graph;
    }

    private void AddChilds(Vertex parent, GraphBase graph, TreeNode<TKey, TValue> currNode)
    {
        var leftNode = currNode.LeftChild;
        if (leftNode is not null)
        {

            var leftChild = new Vertex(graph.Count() + 1)
            {
                Label = GetLabel(leftNode as RedBlackTreeNode<TKey, TValue>, "L")
            };

            graph.AddVertex(leftChild);
            graph.AddEdge(parent, leftChild);
            graph.AddEdge(leftChild, parent);

            AddChilds(leftChild, graph, leftNode);
        }

        var rightNode = currNode.RightChild;
        if (rightNode is not null)
        {
            var rightChild = new Vertex(graph.Count() + 1)
            {
                Label = GetLabel(rightNode as RedBlackTreeNode<TKey, TValue>, "R")
            };

            graph.AddVertex(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph, rightNode);
        }
    }

    private string GetLabel(RedBlackTreeNode<TKey, TValue>? node, string side)
    {
        return $"{node?.Color}:[{node.Key}] {side}";
    }
}
