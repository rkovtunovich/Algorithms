using DataStructures.Lists;

namespace Graphs.Abstraction;

public abstract class Graph : IEnumerable<Vertex>
{
    protected readonly Dictionary<Vertex, LinkedList<Vertex>> _nodes = new();

    protected readonly Dictionary<(Vertex, Vertex), double> _edgesLengths = new();

    protected bool _isVariableLength = false;

    public string? Name { get; set; }

    public int Count { get => _nodes.Count; }

    public abstract bool IsOriented();

    public virtual bool IsVariableEdgeLength()
    {
        return _isVariableLength;
    }

    public abstract Graph Transpose();

    public virtual void Clear()
    {
        _nodes.Clear();
    }

    public virtual int GetDegree(Vertex vertex)
    {
        return _nodes[vertex].Count;
    }

    public virtual Graph Clone()
    {
        var type = GetType();
        var clone = Activator.CreateInstance(type, $"{Name}_clone") as Graph ?? throw new NullReferenceException();

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

    public virtual void CopyVerticesTo(Graph graph)
    {
        graph.Clear();

        foreach (var item in _nodes)
        {
            graph.AddVertex(item.Key);
        }
    }

    #endregion

    #region Edges

    public abstract void AddEdge(Vertex source, Vertex destination);

    public abstract void RemoveEdge(Vertex source, Vertex destination);

    public abstract double GetEdgeLength(Vertex begin, Vertex end);

    public virtual LinkedList<Vertex> GetEdges(Vertex vertex)
    {
        return _nodes[vertex];
    }

    public virtual void SetEdgeLength(Vertex begin, Vertex end, double length)
    {
        if(length > 0)
            _isVariableLength = true;

        _edgesLengths.TryAdd((begin, end), length);
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
        if(newLength == 0)
        {
            RemoveEdge(begin, end);
            _edgesLengths.Remove((begin, end));
        }       
    }
    
    #endregion

    #region Connections

    public virtual bool IsConnected(Vertex firstVertex, Vertex secondVertex)
    {
        var edges = GetEdges(firstVertex);
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

        var edges = GetEdges(vertex);

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
                if (visited.Contains(current))
                    return BackTrackPath(current, path);

                visited.Add(current);

                var edges = GetEdges(current);

                foreach (var edge in edges)
                {
                    if(stack.Contains(edge))
                        return BackTrackPath(current, path);

                    if (visited.Contains(edge))
                        continue;

                    stack.Push(edge);
                    path[edge] = current;
                }
            }
        }

        visited.Clear();

        // If no cycle is found after traversing the entire graph, return an empty list.
        return new();
    }

    private SequentialList<Vertex> BackTrackPath(Vertex current, Dictionary<Vertex, Vertex> path)
    {
        var cycle = new SequentialList<Vertex>
        {
            current
        };

        while (true)
        {
            current = path[current];
            cycle.Add(current);
            if (cycle.Count > 2 && IsConnected(current, cycle[0]))
                break;
        }

        return cycle;
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

    public Vertex this[int index]
    {
        get
        {
            return GetVertexByIndex(index) ?? throw new IndexOutOfRangeException();        
        }
    }
}
