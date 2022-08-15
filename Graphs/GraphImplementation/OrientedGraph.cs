using Graphs.Abstraction;

namespace Graphs.GraphImplementation;

public class OrientedGraph : Graph
{
    public OrientedGraph(string name)
    {
        Name = name;
    }

    private Dictionary<Vertice, List<Vertice>> _belowNeighbors = new();

    public override void AddEdge(Vertice sourse, Vertice destination)
    {
        if (!_nodes.ContainsKey(sourse) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the garph!");

        var sourceEdges = _nodes[sourse];
        AddConnection(sourceEdges, destination);
    }

    public override bool IsOriented()
    {
        return true;
    }

    public void AddBelowNeighbor(Vertice owner, Vertice neigbor)
    {
        if(!_belowNeighbors.ContainsKey(owner))
            _belowNeighbors.Add(owner, new List<Vertice> { neigbor });
        else
        {
            if(!_belowNeighbors[owner].Contains(neigbor))
                _belowNeighbors[owner].Add(neigbor);
        }         
    }

    public List<Vertice> GetBelowNeighbors(Vertice owner)
    {
        return _belowNeighbors[owner];
    }

    public override Graph Transpose(Graph graph)
    {
        OrientedGraph transposed = new("Transposed");
        graph.CopyVerticesTo(transposed);

        foreach (var vertice in graph)
        {
            var edges = graph.GetEdges(vertice);

            foreach (var edge in edges)
            {
                transposed.AddConnection(edge, vertice); 
            }
        }

        return transposed;
    }
}
