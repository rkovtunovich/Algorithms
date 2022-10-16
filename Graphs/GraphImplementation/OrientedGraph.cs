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

    public override Graph Transpose()
    {
        OrientedGraph transposed = new("Transposed");
        CopyVerticesTo(transposed);

        foreach (var vertice in this)
        {
            var edges = GetEdges(vertice);

            foreach (var edge in edges)
            {
                transposed.AddConnection(edge, vertice); 
            }
        }

        return transposed;
    }

    #region Edges

    public override double GetEdgeLength(Vertice begin, Vertice end)
    {
        if (_edgesLengths.TryGetValue((begin, end), out double length))
            return length;

        return 0;
    }

    public override void RemoveEdge(Vertice sourse, Vertice destination)
    {
        RemoveConnection(_nodes[sourse], destination);
    }

    public override bool IsVariableEdgeLength()
    {
        return true;
    }

    #endregion
}
