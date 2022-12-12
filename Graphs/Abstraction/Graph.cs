using System.Collections;

namespace Graphs.Abstraction;

public abstract class Graph : IEnumerable<Vertice>
{
    protected readonly Dictionary<Vertice, LinkedList<Vertice>> _nodes = new();

    protected readonly Dictionary<(Vertice, Vertice), double> _edgesLengths = new();

    public string? Name { get; set; }

    public int Count { get => _nodes.Count; }

    public abstract bool IsOriented();

    public virtual bool IsVariableEdgeLength()
    {
        return false;
    }

    public abstract Graph Transpose();

    public virtual void Clear()
    {
        _nodes.Clear();
    }

    public virtual int GetDegree(Vertice vertice)
    {
        return _nodes[vertice].Count;
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

    public bool HasVertice(Vertice vertice)
    {
        return _nodes.ContainsKey(vertice);
    }

    public virtual Vertice? GetVerticeByIndex(int index)
    {
        foreach (var node in _nodes)
        {
            if (node.Key.Index == index)
                return node.Key;
        }

        return null;
    }

    public virtual void AddVertice(Vertice vertice)
    {
        if (_nodes.ContainsKey(vertice))
            return;

        _nodes.TryAdd(vertice, new LinkedList<Vertice>());
    }

    public virtual void CopyVerticesTo(Graph graph)
    {
        graph.Clear();

        foreach (var item in _nodes)
        {
            graph.AddVertice(item.Key);
        }
    }

    #endregion

    #region Edges

    public abstract void AddEdge(Vertice sourse, Vertice destination);

    public abstract void RemoveEdge(Vertice sourse, Vertice destination);

    public abstract double GetEdgeLength(Vertice begin, Vertice end);

    public virtual LinkedList<Vertice> GetEdges(Vertice vertice)
    {
        return _nodes[vertice];
    }

    public virtual void SetEdgeLength(Vertice begin, Vertice end, double length)
    {
        _edgesLengths.TryAdd((begin, end), length);
    }

    public virtual void ChangeEdgeLength(Vertice begin, Vertice end, double length)
    {
        if (!_edgesLengths.TryGetValue((begin, end), out double currLength))
        {
            AddEdge(begin, end);
            SetEdgeLength(begin, end, length);
            return;
        }
        
        double newLength = currLength + length;
        if(newLength == 0)
        {
            RemoveEdge(begin, end);
            _edgesLengths.Remove((begin, end));
        }
        

    }

    #endregion

    #region Connections

    public virtual bool IsConnected(Vertice firstVertice, Vertice secondVertice)
    {
        var edges = GetEdges(firstVertice);
        return edges.Contains(secondVertice);
    }

    public void AddConnection(Vertice begin, Vertice end)
    {
        if (!HasVertice(begin))
            AddVertice(begin);

        if (!HasVertice(end))
            AddVertice(end);

        _nodes[begin].AddLast(end);
    }

    protected static void AddConnection(LinkedList<Vertice> edges, Vertice vertice)
    {
        if (edges.Count == 0)
        {
            edges.AddFirst(vertice);
            return;
        }

        if (edges.Contains(vertice))
            return;
        
        edges.AddLast(vertice);
    }

    protected static void RemoveConnection(LinkedList<Vertice> edges, Vertice vertice)
    {
        if (edges.Count == 0)
            return;

        edges.Remove(vertice);
    }

    #endregion

    #region Enumerable

    public IEnumerator<Vertice> GetEnumerator()
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
