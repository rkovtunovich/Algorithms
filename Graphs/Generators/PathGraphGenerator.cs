using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.Generators;

public class PathGraphGenerator : IGraphGenerator
{
    private readonly int _vertexCount;
    private readonly int _maxWeight;

    public PathGraphGenerator(int vertexCount, int maxWeight)
    {
        _vertexCount = vertexCount;
        _maxWeight = maxWeight;
    }

    public Graph Generate(string name)
    {
        var random = new Random();
        var graph = new UndirectedGraph(name);

        Vertex? prevVertex = null;

        for (int i = 0; i < _vertexCount; i++)
        {
            var vertex = new Vertex(i)
            {
                Weight = Math.Round(random.NextDouble() * _maxWeight, 2)
            };

            graph.AddVertex(vertex);

            if(prevVertex is not null)
            {
                graph.AddConnection(prevVertex, vertex);
                graph.AddConnection(vertex, prevVertex);
            }

            prevVertex = vertex;
        }

        return graph;
    }
}
