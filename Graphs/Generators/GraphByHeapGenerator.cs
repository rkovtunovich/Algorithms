using DataStructures.Heaps;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;
using System.Numerics;

namespace Graphs.Generators;

public class GraphByHeapGenerator<TKey, TValue> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly Heap<TKey, TValue> _heap; 

    public GraphByHeapGenerator(Heap<TKey, TValue> heap)
    {
        _heap = heap;
    }

    public Graph Generate(string name = nameof(_heap))
    {
        var graph = new UndirectedGraph(name);

        var vertice = new Vertice(1)
        {
            Label = $"[{_heap.Extremum.Key}]"
        };
        graph.AddVertice(vertice);
        
        AddChilds(vertice, graph);

        return graph;
    }

    private void AddChilds(Vertice parent, Graph graph)
    {
        int leftPos = _heap.GetLeftChildPosition(parent.Index);
        if (leftPos is not -1) {

            var leftChild = new Vertice(leftPos)
            {
                Label = $"[{_heap[leftPos].Key}]"
            };

            graph.AddVertice(leftChild);
            graph.AddEdge(parent, leftChild);
            graph.AddEdge(leftChild, parent);

            AddChilds(leftChild, graph);
        }

        int rightPos = _heap.GetRightChildPosition(parent.Index);
        if (rightPos is not -1)
        {
            var rightChild = new Vertice(rightPos)
            {
                Label = $"[{_heap[rightPos].Key}]"
            };

            graph.AddVertice(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph);
        }
    }
}
