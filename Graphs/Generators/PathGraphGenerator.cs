using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.Generators;

public class PathGraphGenerator : IGraphGenerator
{
    private readonly int _verticeCount;
    private readonly int _maxWeigth;

    public PathGraphGenerator(int verticeCount, int maxWeigth)
    {
        _verticeCount = verticeCount;
        _maxWeigth = maxWeigth;
    }

    public Graph Generate(string name)
    {
        var random = new Random();
        var graph = new UndirectedGraph(name);

        Vertex? prevVertice = null;

        for (int i = 0; i < _verticeCount; i++)
        {
            var vertice = new Vertex(i)
            {
                Weight = Math.Round(random.NextDouble() * _maxWeigth, 2)
            };

            graph.AddVertice(vertice);

            if(prevVertice is not null)
            {
                graph.AddConnection(prevVertice, vertice);
                graph.AddConnection(vertice, prevVertice);
            }

            prevVertice = vertice;
        }

        return graph;
    }
}
