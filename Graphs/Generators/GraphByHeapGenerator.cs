using DataStructures;
using Graphs.Abstraction;
using Graphs.GraphImplementation;
using System.Numerics;

namespace Graphs.Generators;

public class GraphByHeapGenerator<T> : GraphGenerator where T : INumber<T>
{
    private readonly Heap<T> _heap; 

    public GraphByHeapGenerator(Heap<T> heap)
    {
        _heap = heap;
    }

    public override Graph Generate()
    {
        var graph = new UndirectedGraph(nameof(_heap));

        var vertice = new Vertice(1)
        {
            Label = $"[{_heap.Extremum.ToString()}]"
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
                Label = $"[{_heap[leftPos].ToString()}]"
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
                Label = $"[{_heap[rightPos].ToString()}]"
            };

            graph.AddVertice(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph);
        }
    }
}
