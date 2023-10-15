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

    public Vertex FindRoot()
    {
        // Initialize variables to keep track of minimum in-degree and corresponding vertex
        int minInDegree = int.MaxValue;
        Vertex? root = null;

        // Loop through all vertices in the graph
        foreach (var vertex in this)
        {
            // Calculate in-degree for each vertex
            int inDegree = 0;
            foreach (var otherVertex in this)
            {
                if (IsConnected(otherVertex, vertex))               
                    inDegree++;                
            }

            // Update minimum in-degree and root vertex
            if (inDegree < minInDegree)
            {
                minInDegree = inDegree;
                root = vertex;
            }
        }

        if (root is null)       
            throw new InvalidOperationException("Could not determine root of the graph.");
        
        return root;
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