using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Generators;

public class GraphByHeapGenerator<TKey, TValue> : IGraphGenerator where TKey : INumber<TKey>
{
    private readonly Heap<TKey, TValue> _heap;

    public GraphByHeapGenerator(Heap<TKey, TValue> heap)
    {
        _heap = heap;
    }

    public GraphBase Generate(string name = nameof(_heap))
    {
        var graph = new UndirectedGraph(name);

        var vertex = new Vertex(1)
        {
            Label = $"[{_heap.Extremum.Key}]"
        };
        graph.AddVertex(vertex);

        AddChilds(vertex, graph);

        return graph;
    }

    private void AddChilds(Vertex parent, GraphBase graph)
    {
        int leftPos = _heap.GetLeftChildPosition(parent.Index);
        if (leftPos is not -1)
        {

            var leftChild = new Vertex(leftPos)
            {
                Label = $"[{_heap[leftPos].Key}]"
            };

            graph.AddVertex(leftChild);
            graph.AddEdge(parent, leftChild);
            graph.AddEdge(leftChild, parent);

            AddChilds(leftChild, graph);
        }

        int rightPos = _heap.GetRightChildPosition(parent.Index);
        if (rightPos is not -1)
        {
            var rightChild = new Vertex(rightPos)
            {
                Label = $"[{_heap[rightPos].Key}]"
            };

            graph.AddVertex(rightChild);
            graph.AddEdge(parent, rightChild);
            graph.AddEdge(rightChild, parent);

            AddChilds(rightChild, graph);
        }
    }
}
