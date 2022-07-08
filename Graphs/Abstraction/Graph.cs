using System.Collections;

namespace Graphs.Abstraction;

public abstract class Graph<T> : IEnumerable<Vertice<T>>
{
    protected readonly Dictionary<Vertice<T>, LinkedList<Vertice<T>>> _nodes = new();

    public string? Name { get; set; }

    public abstract void AddEdge(Vertice<T> sourse, Vertice<T> destination);

    public abstract bool IsOriented();

    public virtual void AddVertice(Vertice<T> vertice)
    {
        if (_nodes.ContainsKey(vertice))
            return;

        _nodes.TryAdd(vertice, new LinkedList<Vertice<T>>());
    }

    public virtual void Clear()
    {
        _nodes.Clear();
    }

    public virtual void CopyVerticesTo(Graph<T> graph)
    {
        graph.Clear();

        foreach (var item in _nodes)
        {
            graph.AddVertice(item.Key);
        }
    }

    public virtual LinkedList<Vertice<T>> GetEdges(Vertice<T> vertice)
    {
        return _nodes[vertice];
    }

    public virtual Vertice<T>? GetVerticeByIndex(int index)
    {
        foreach (var node in _nodes)
        {
            if (node.Key.Index == index)
                return node.Key;
        }

        return null;
    }

    public virtual int GetDegree(Vertice<T> vertice)
    {
        return _nodes[vertice].Count;
    }

    #region Connections

    public virtual bool IsConnected(Vertice<T> firstVertice, Vertice<T> secondVertice)
    {
        var edges = GetEdges(firstVertice);
        return edges.Contains(secondVertice);
    }

    protected static void AddConnection(LinkedList<Vertice<T>> edges, Vertice<T> vertice)
    {
        if (edges.Count == 0)
            edges.AddFirst(vertice);
        else
            edges.AddLast(vertice);
    }

    #endregion

    #region Enumerable

    public IEnumerator<Vertice<T>> GetEnumerator()
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
