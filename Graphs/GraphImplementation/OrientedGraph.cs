namespace Graphs.GraphImplementation;

public class OrientedGraph : Graph
{
    public OrientedGraph(string name)
    {
        Name = name;
    }

    private Dictionary<Vertex, List<Vertex>> _belowNeighbors = new();

    public override void AddEdge(Vertex source, Vertex destination)
    {
        if (!_nodes.ContainsKey(source) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the graph!");

        var sourceEdges = _nodes[source];
        AddConnection(sourceEdges, destination);
    }

    public override bool IsOriented()
    {
        return true;
    }

    public void AddBelowNeighbor(Vertex owner, Vertex neighbor)
    {
        if(!_belowNeighbors.ContainsKey(owner))
            _belowNeighbors.Add(owner, new List<Vertex> { neighbor });
        else
        {
            if(!_belowNeighbors[owner].Contains(neighbor))
                _belowNeighbors[owner].Add(neighbor);
        }         
    }

    public List<Vertex> GetBelowNeighbors(Vertex owner)
    {
        return _belowNeighbors[owner];
    }

    public override Graph Transpose()
    {
        OrientedGraph transposed = new("Transposed");
        CopyVerticesTo(transposed);

        foreach (var vertex in this)
        {
            var edges = GetEdges(vertex);

            foreach (var edge in edges)
            {
                transposed.AddConnection(edge, vertex); 
            }
        }

        return transposed;
    }

    #region Edges

    public override double GetEdgeLength(Vertex begin, Vertex end)
    {
        if (_edgesLengths.TryGetValue((begin, end), out double length))
            return length;

        return 0;
    }

    public override void RemoveEdge(Vertex sourse, Vertex destination)
    {
        RemoveConnection(_nodes[sourse], destination);
    }

    public override bool IsVariableEdgeLength()
    {
        return true;
    }

    #endregion
}