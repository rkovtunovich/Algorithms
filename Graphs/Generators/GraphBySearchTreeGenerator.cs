using DataStructures.SearchTrees;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using System.Numerics;

namespace Graphs.Generators;

public class GraphSearchTreeGenerator<TKey, TValue> : GraphGenerator where TKey : INumber<TKey>
{
    private readonly SearchTree<TKey, TValue> _tree; 

    public GraphSearchTreeGenerator(SearchTree<TKey, TValue> tree)
    {
        _tree = tree;
    }

    public override Graph Generate(string name)
    {
        var graph = new UndirectedGraph(name);

        var vertice = new Vertice(1)
        {
            Label = $"[{_tree.Root.Key}]"
        };
        graph.AddVertice(vertice);
        
        AddChilds(vertice, graph, _tree.Root);

        return graph;
    }

    private void AddChilds(Vertice parent, Graph graph, TreeNode<TKey, TValue> currNode)
    {
        var leftNode = currNode.LeftChild;
        if (leftNode is not null) {

            var leftChild = new Vertice(graph.Count() + 1)
            {
                Label = $"[{leftNode.Key}]"
            };

            graph.AddVertice(leftChild);
            graph.AddEdge(parent, leftChild);
            graph.AddEdge(leftChild, parent);

            AddChilds(leftChild, graph, leftNode);
        }

        var rightNode = currNode.RightChild;
        if (rightNode is not null)
        {
            var rightChild = new Vertice(graph.Count() + 1)
            {
                Label = $"[{rightNode.Key}]"
            };

            graph.AddVertice(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph, rightNode);
        }
    }
}
