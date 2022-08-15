using Graphs.GraphImplementation;
using System.Collections;

namespace Graphs.Abstraction;

public abstract class Graph : IEnumerable<Vertice>
{
    protected readonly Dictionary<Vertice, LinkedList<Vertice>> _nodes = new();

    public string? Name { get; set; }

    public abstract void AddEdge(Vertice sourse, Vertice destination);

    public abstract bool IsOriented();

    public abstract Graph Transpose(Graph graph);

    public virtual void AddVertice(Vertice vertice)
    {
        if (_nodes.ContainsKey(vertice))
            return;

        _nodes.TryAdd(vertice, new LinkedList<Vertice>());
    }

    public virtual void Clear()
    {
        _nodes.Clear();
    }

    public virtual void CopyVerticesTo(Graph graph)
    {
        graph.Clear();

        foreach (var item in _nodes)
        {
            graph.AddVertice(item.Key);
        }
    }

    public virtual LinkedList<Vertice> GetEdges(Vertice vertice)
    {
        return _nodes[vertice];
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

    public virtual int GetDegree(Vertice vertice)
    {
        return _nodes[vertice].Count;
    }

    #region Connections

    public bool hasVertice(Vertice vertice)
    {
        return _nodes.ContainsKey(vertice);
    }

    public virtual bool IsConnected(Vertice firstVertice, Vertice secondVertice)
    {
        var edges = GetEdges(firstVertice);
        return edges.Contains(secondVertice);
    }

    public void AddConnection(Vertice begin, Vertice end)
    {
        if (!hasVertice(begin))
            AddVertice(begin);

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
