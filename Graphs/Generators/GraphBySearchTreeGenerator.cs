using DataStructures.BinaryTrees;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;
using System.Numerics;

namespace Graphs.Generators;

public class GraphSearchTreeGenerator<TKey, TValue> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly BinaryTree<TKey, TValue> _tree; 

    public GraphSearchTreeGenerator(BinaryTree<TKey, TValue> tree)
    {
        _tree = tree;
    }

    public Graph Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        var vertice = new Vertex(1)
        {
            Label = CreateLabel(_tree.Root)
        };
        graph.AddVertex(vertice);
        
        AddChilds(vertice, graph, _tree.Root);

        return graph;
    }

    private void AddChilds(Vertex parent, Graph graph, TreeNode<TKey, TValue> currNode)
    {
        var leftNode = currNode.LeftChild;
        if (leftNode is not null) {

            var leftChild = new Vertex(graph.Count() + 1)
            {
                Label = CreateLabel(leftNode)
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
                Label = CreateLabel(rightNode)
            };

            graph.AddVertex(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph, rightNode);
        }
    }

    private string CreateLabel(TreeNode<TKey, TValue> node)
    {
        var label = $"[{node.Key}]";

        if (node.Value is not null)
            label = $"{label}:{node.Value}";

        return label;
    }

}
