using DataStructures.BinaryTrees;
using DataStructures.BinaryTrees.Search;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;
using System.Numerics;

namespace Graphs.Generators;

public class GraphAVLTreeGenerator<TKey, TValue> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly SearchTree<TKey, TValue> _tree; 

    public GraphAVLTreeGenerator(SearchTree<TKey, TValue> tree)
    {
        _tree = tree;
    }

    public Graph Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        if(_tree.Root is null)
            return graph;

        var vertice = new Vertex(1)
        {
            Label = GetLabel(_tree.Root as AVLTreeNode<TKey, TValue>, "O")
        };
        graph.AddVertice(vertice);
        
        AddChilds(vertice, graph, _tree.Root);

        return graph;
    }

    private void AddChilds(Vertex parent, Graph graph, TreeNode<TKey, TValue> currNode)
    {
        var leftNode = currNode.LeftChild;
        if (leftNode is not null) {

            var leftChild = new Vertex(graph.Count() + 1)
            {
                Label =  GetLabel(leftNode as AVLTreeNode<TKey, TValue>, "L")
            };

            graph.AddVertice(leftChild);
            graph.AddEdge(parent, leftChild);
            graph.AddEdge(leftChild, parent);

            AddChilds(leftChild, graph, leftNode);
        }

        var rightNode = currNode.RightChild;
        if (rightNode is not null)
        {
            var rightChild = new Vertex(graph.Count() + 1)
            {
                Label = GetLabel(rightNode as AVLTreeNode<TKey, TValue>, "R")
            };

            graph.AddVertice(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph, rightNode);
        }
    }

    private string GetLabel(AVLTreeNode<TKey, TValue>? node, string side)
    {
        return $"{node?.Hight}:[{node.Key}] {side}";
    }
}
