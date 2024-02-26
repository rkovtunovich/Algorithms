namespace Graphs.Core.GraphImplementation;

public class OrientedGraph : Graph
{
    public OrientedGraph(string name, bool trackIncomeEdges = false)
    {
        Name = name;
        TrackIncomeEdges = trackIncomeEdges;
    }

    public Dictionary<Vertex, List<Vertex>> IncomeEdges { get; private set; } = [];

    public bool TrackIncomeEdges { get; private set; }

    public override bool IsOriented()
    {
        return true;
    }

    #region Vertices

    public Vertex FindRoot()
    {
        // Initialize variables to keep track of minimum in-degree and corresponding vertex
        int minInDegree = int.MaxValue;
        Vertex? root = null;

        // Loop through all vertices in the graph
        foreach (var vertex in this)
        {
            // Skip vertices with degree 0
            if(GetDegree(vertex) == 0)
                continue;

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

    public override void RemoveVertex(Vertex vertex)
    {
        base.RemoveVertex(vertex);

        if (TrackIncomeEdges)
            IncomeEdges.Remove(vertex);

        for (int i = 0; i < IncomeEdges.Count; i++)
        {
            var incomeEdges = IncomeEdges.ElementAt(i).Value;
            incomeEdges.Remove(vertex);
        }
    }

    #endregion

    #region Edges

    public override double GetEdgeLength(Vertex begin, Vertex end)
    {
        if (_edgesLengths.TryGetValue((begin, end), out double length))
            return length;

        return 0;
    }

    public override void AddEdge(Vertex source, Vertex destination)
    {
        if (!_nodes.TryGetValue(source, out LinkedList<Vertex>? value) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the graph!");

        var sourceEdges = value;
        AddConnection(sourceEdges, destination);

        if (TrackIncomeEdges)
            AddIncomeEdge(destination, source);
    }

    public void AddIncomeEdge(Vertex owner, Vertex neighbor)
    {
        if (!IncomeEdges.TryGetValue(owner, out List<Vertex>? value))
            IncomeEdges.Add(owner, new List<Vertex> { neighbor });
        else
        {
            if (!value.Contains(neighbor))
                value.Add(neighbor);
        }
    }

    public void FillIncomeEdges(bool setAsTrackable = false)
    {
        IncomeEdges.Clear();

        foreach (var vertex in this)
        {
            IncomeEdges.Add(vertex, []);
        }

        foreach (var vertex in this)
        {
            var edges = GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                AddIncomeEdge(edge, vertex);
            }
        }

        if (setAsTrackable)
            TrackIncomeEdges = true;
    }

    public override void RemoveEdge(Vertex source, Vertex destination)
    {
        RemoveConnection(_nodes[source], destination);

        if (TrackIncomeEdges)
        {
            var incomeEdges = IncomeEdges[destination];
            incomeEdges.Remove(source);
        }          
    }

    public override void CleanEdges()
    {
        base.CleanEdges();

        if (TrackIncomeEdges)
            FillIncomeEdges();
    }

    public override bool IsVariableEdgeLength()
    {
        return true;
    }

    public override Graph Clone()
    {
        OrientedGraph clone = new(Name, TrackIncomeEdges);
        CopyVerticesTo(clone);

        foreach (var vertex in this)
        {
            var edges = GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                clone.AddConnection(vertex, edge);
            }
        }

        if(IsVariableEdgeLength())
            foreach (var item in _edgesLengths)
            {
                clone.SetEdgeLength(item.Key.Item1, item.Key.Item2, item.Value);
            }

        if(TrackIncomeEdges)
            clone.FillIncomeEdges();

        return clone;
    }

    #endregion

    #region Utils

    public override Graph Transpose()
    {
        OrientedGraph transposed = new("Transposed");
        CopyVerticesTo(transposed);

        foreach (var vertex in this)
        {
            var edges = GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                transposed.AddConnection(edge, vertex);
            }
        }

        return transposed;
    }

    public Vertex? GetSink()
    {
        foreach (var vertex in this)
        {
            if (GetDegree(vertex) is 0)
                return vertex;
        }

        return null;
    }

    public Vertex? GetSource()
    {
        foreach (var vertex in this)
        {
            if(!IncomeEdges.TryGetValue(vertex, out List<Vertex>? edges))
                return vertex;

            if (edges.Count is 0)
                return vertex;
        }

        return null;
    }

    #endregion
}