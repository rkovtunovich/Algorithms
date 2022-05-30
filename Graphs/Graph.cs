using System.Collections;

namespace Graphs;

public class Graph<T> : IEnumerable<T> where T : notnull
{
    private readonly Dictionary<T, LinkedList<T>> _nodes = new();

    private int[]? _degreeDistribuition = null;  

    public string Name { get; set; } = "undirected";

    public void AddVertice(T vertice)
    {
        if (_nodes.ContainsKey(vertice))
            return;

        _nodes.TryAdd(vertice, new LinkedList<T>());
    }

    public void AddEdge(T sourse, T destination)
    {
        if (!_nodes.ContainsKey(sourse) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the garph!");

        var sourceEdges = _nodes[sourse];
        var destinationEdges = _nodes[destination];

        Graph<T>.AddConnection(sourceEdges, destination);
        Graph<T>.AddConnection(destinationEdges, sourse);     
    } 

    public LinkedList<T> GetEdges(T vertice)
    {
        return _nodes[vertice];
    }

    public bool IsConnected(T firstVertice, T secondVertice)
    {
        var edges = GetEdges(firstVertice);
        return edges.Contains(secondVertice);
    }

    #region Degree

    public int GetDegree(T vertice)
    {
        return _nodes[vertice].Count;
    }

    public int[] GetDedreeDistributionsCount()
    {
        if (_degreeDistribuition is not null)
            return _degreeDistribuition;

        if (_nodes.Count == 0)
            return Array.Empty<int>();

        _degreeDistribuition = new int[_nodes.Count - 1];

        foreach (var node in _nodes)
        {
            _degreeDistribuition[GetDegree(node.Key)]++;
        }

        return _degreeDistribuition;
    }

    public double[] GetDedreeDistributionsFraction()
    {
        if (_nodes.Count == 0)
            return Array.Empty<double>();

        if (_degreeDistribuition is null)
            _degreeDistribuition = GetDedreeDistributionsCount();

        var digreeDistribuition = new double[_nodes.Count - 1];

        for (int i = 0; i < _degreeDistribuition.Length; i++)
        {
            digreeDistribuition[i] = (double)_degreeDistribuition[i] / _nodes.Count;
        }

        return digreeDistribuition;
    }
    
    public double[] GetDegreeDistributionsCumulative()
    {
        var fracDigreeDistr = GetDedreeDistributionsFraction();

        var cumulativeDegreeDistr = new double[_nodes.Count];
        cumulativeDegreeDistr[0] = 1;

        for (int i = 1; i < fracDigreeDistr.Length; i++)
        {
            cumulativeDegreeDistr[i] = cumulativeDegreeDistr[i - 1] - fracDigreeDistr[i];
        }

        return cumulativeDegreeDistr;
    }

    #endregion

    private static void AddConnection(LinkedList<T> edges, T vertice)
    {
        if (edges.Count == 0)
            edges.AddFirst(vertice);
        else
            edges.AddLast(vertice);
    }

    public IEnumerator<T> GetEnumerator()
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
}
