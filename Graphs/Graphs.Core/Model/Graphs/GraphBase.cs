using DataStructures.Lists;

namespace Graphs.Core.Model.Graphs;

public abstract class GraphBase : IEnumerable<Vertex>
{
    protected readonly Dictionary<Vertex, LinkedList<Vertex>> _nodes = [];

    protected readonly Dictionary<(Vertex, Vertex), double> _edgesLengths = [];

    protected bool _isVariableLength = false;

    public string? Name { get; set; }

    public int Count { get => _nodes.Count; }

    public abstract bool IsOriented();

    public virtual bool IsVariableEdgeLength()
    {
        return _isVariableLength;
    }

    public abstract GraphBase Transpose();

    public virtual void Clear()
    {
        _nodes.Clear();
    }

    public virtual int GetDegree(Vertex vertex)
    {
        return _nodes[vertex].Count;
    }

    public virtual GraphBase Clone()
    {
        var type = GetType();
        var clone = Activator.CreateInstance(type, $"{Name}_clone") as GraphBase ?? throw new NullReferenceException();

        CopyVerticesTo(clone);

        foreach (var node in _nodes)
        {
            clone._nodes[node.Key] = new(node.Value);
        }

        foreach (var length in _edgesLengths)
        {
            clone._edgesLengths.TryAdd(length.Key, length.Value);
        }

        return clone;
    }

    #region Vertices

    public Vertex this[int index]
    {
        get
        {
            return GetVertexByIndex(index) ?? throw new IndexOutOfRangeException();
        }
    }

    public bool HasVertex(Vertex vertex)
    {
        return _nodes.ContainsKey(vertex);
    }

    public virtual Vertex? GetVertexByIndex(int index)
    {
        foreach (var node in _nodes)
        {
            if (node.Key.Index == index)
                return node.Key;
        }

        return null;
    }

    public virtual void AddVertex(Vertex vertex)
    {
        if (_nodes.ContainsKey(vertex))
            return;

        _nodes.TryAdd(vertex, new LinkedList<Vertex>());
    }

    public virtual void RemoveVertex(Vertex vertex)
    {
        if (!_nodes.ContainsKey(vertex))
            return;

        _nodes.Remove(vertex);

        foreach (var item in _nodes)
        {
            RemoveConnection(item.Value, vertex);
        }
    }

    public virtual void CopyVerticesTo(GraphBase graph)
    {
        graph.Clear();

        foreach (var item in _nodes)
        {
            graph.AddVertex(item.Key);
        }
    }

    public Vertex CreateVertex()
    {
        var vertex = new Vertex(Count + 1);
        AddVertex(vertex);
        return vertex;
    }

    #endregion

    #region Edges

    public abstract void AddEdge(Vertex source, Vertex destination);

    public virtual void AddEdgeWithLength(Vertex source, Vertex destination, double length)
    {
        AddEdge(source, destination);
        SetEdgeLength(source, destination, length);
    }

    public abstract void RemoveEdge(Vertex source, Vertex destination);

    public virtual void CleanEdges()
    {
        foreach (var node in _nodes)
        {
            node.Value.Clear();
        }
    }

    public abstract double GetEdgeLength(Vertex begin, Vertex end);

    public virtual LinkedList<Vertex> GetAdjacentEdges(Vertex vertex)
    {
        return _nodes[vertex];
    }

    public virtual List<(Vertex, Vertex)> GetAllEdges()
    {
        var edges = new List<(Vertex, Vertex)>();

        foreach (var node in _nodes)
        {
            foreach (var edge in node.Value)
            {
                edges.Add((node.Key, edge));
            }
        }

        return edges;
    }

    public virtual void SetEdgeLength(Vertex begin, Vertex end, double length)
    {
        if (length > 0)
            _isVariableLength = true;

        if (!_edgesLengths.TryAdd((begin, end), length))
            _edgesLengths[(begin, end)] = length;
    }

    public virtual void ChangeEdgeLength(Vertex begin, Vertex end, double length)
    {
        if (!_edgesLengths.TryGetValue((begin, end), out double currentLength))
        {
            AddEdge(begin, end);
            SetEdgeLength(begin, end, length);
            return;
        }

        double newLength = currentLength + length;
        if (newLength == 0)
        {
            RemoveEdge(begin, end);
            _edgesLengths.Remove((begin, end));
        }
    }

    #endregion

    #region Connections

    public virtual bool IsConnected(Vertex firstVertex, Vertex secondVertex)
    {
        var edges = GetAdjacentEdges(firstVertex);
        return edges.Contains(secondVertex);
    }

    public void AddConnection(Vertex begin, Vertex end)
    {
        if (!HasVertex(begin))
            AddVertex(begin);

        if (!HasVertex(end))
            AddVertex(end);

        _nodes[begin].AddLast(end);
    }

    protected static void AddConnection(LinkedList<Vertex> edges, Vertex vertex)
    {
        if (edges.Count == 0)
        {
            edges.AddFirst(vertex);
            return;
        }

        if (edges.Contains(vertex))
            return;

        edges.AddLast(vertex);
    }

    protected static void RemoveConnection(LinkedList<Vertex> edges, Vertex vertex)
    {
        if (edges.Count == 0)
            return;

        edges.Remove(vertex);
    }

    public virtual Vertex? GetClosestNeighbor(Vertex vertex)
    {
        Vertex? closest = null;

        var edges = GetAdjacentEdges(vertex);

        double bestLength = double.MaxValue;

        foreach (var edge in edges)
        {
            var length = GetEdgeLength(vertex, edge);

            if (length < bestLength)
            {
                bestLength = length;
                closest = edge;
            }
        }

        return closest;
    }

    #endregion

    #region Cycle

    /// <summary>
    /// Searches for a cycle in the graph starting from the specified origin vertex using Depth-first search (DFS).
    /// </summary>
    /// <param name="graph">Graph to search the cycle in.</param>
    /// <returns>A hash set of vertices that form a cycle starting from the origin vertex.</returns>
    public SequentialList<Vertex> SearchCycle()
    {
        // we got to key vertex from value vertex
        var path = new Dictionary<Vertex, Vertex>();
        var visited = new HashSet<Vertex>();
        var cycle = new SequentialList<Vertex>();

        foreach (var vertex in this)
        {
            if (visited.Contains(vertex))
                continue;

            var stack = new Stack<Vertex>();
            stack.Push(vertex);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                // If current vertex is already visited, then we have a cycle.
                //if (visited.Contains(current))
                //    return BackTrackPath(current, path);

                visited.Add(current);

                var edges = GetAdjacentEdges(current);

                foreach (var edge in edges)
                {
                    path[edge] = current;

                    if (visited.Contains(edge))
                    {
                        if (CheckIfCycle(edge, path, out cycle))
                            return cycle ?? [];
                        else
                            continue;
                    }

                    stack.Push(edge);
                }
            }
        }

        visited.Clear();

        // If no cycle is found after traversing the entire graph, return an empty list.
        return cycle ?? [];
    }

    private bool CheckIfCycle(Vertex start, Dictionary<Vertex, Vertex> path, out SequentialList<Vertex>? cycle)
    {
        cycle = null;
        var track = new SequentialList<Vertex>();

        var current = start;

        while (true)
        {
            track.Add(current);

            if (!path.TryGetValue(current, out Vertex next))
                return false;

            if (track.Contains(next))
            {
                var startIndex = track.IndexOf(next);
                var length = track.Count - startIndex;
                cycle = track.GetRange(startIndex, length);
                return true;
            }

            current = next;
        }
    }

    #endregion

    #region Enumerable

    public IEnumerator<Vertex> GetEnumerator()
    {
        foreach (var item in _nodes)
        {
            yield return item.Key;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in _nodes)
        {
            yield return item.Key;
        }
    }

    #endregion
}
