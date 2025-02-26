namespace Graphs.Core.Model.Graphs;

public class OrientedGraph : GraphBase
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

    /// <summary>
    /// Finds the root of an oriented tree or hierarchy.
    /// The root is defined as the vertex with the minimum in-degree (i.e., no incoming edges or the fewest).
    /// In a tree-like hierarchy, this should be exactly one vertex with in-degree = 0.
    /// If multiple vertices share the same minimum in-degree, any of them could serve as the root.
    /// </summary>
    /// <returns>The vertex identified as the root.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the graph is empty or if no root can be determined.
    /// </exception>
    public Vertex FindRoot()
    {
        // If the graph is empty, there's no root.
        if (!this.Any())
            throw new InvalidOperationException("The graph has no vertices, cannot determine root.");

        // We'll track the minimum in-degree found so far, and the corresponding vertex.
        int minInDegree = int.MaxValue;
        Vertex? root = null;

        // Loop over each vertex in the graph to compute its in-degree.
        // The in-degree of a vertex v is the number of edges (x -> v) that exist in the graph.
        foreach (var vertex in this)
        {
            // Compute how many incoming edges 'vertex' has
            int inDegree = 0;
            foreach (var otherVertex in this)
            {
                // Check if there's a directed edge (otherVertex -> vertex)
                if (IsConnected(otherVertex, vertex))              
                    inDegree++;            
            }

            // Update the root candidate if this vertex has fewer incoming edges
            if (inDegree < minInDegree)
            {
                minInDegree = inDegree;
                root = vertex;
            }
        }

        // If no root was found (should be impossible if the graph has vertices), throw an exception
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

    public List<Vertex> GetIncomeEdges(Vertex vertex)
    {
        if (!IncomeEdges.TryGetValue(vertex, out _))
            return [];

        return IncomeEdges[vertex];
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

    public override GraphBase Clone()
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

        if (IsVariableEdgeLength())
            foreach (var item in _edgesLengths)
            {
                clone.SetEdgeLength(item.Key.Item1, item.Key.Item2, item.Value);
            }

        if (TrackIncomeEdges)
            clone.FillIncomeEdges();

        return clone;
    }

    #endregion

    #region Utils

    public override GraphBase Transpose()
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
            if (!IncomeEdges.TryGetValue(vertex, out List<Vertex>? edges))
                return vertex;

            if (edges.Count is 0)
                return vertex;
        }

        return null;
    }

    #endregion
}