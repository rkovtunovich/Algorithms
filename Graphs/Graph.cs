using System.Collections;

namespace Graphs;

public class Graph<T> : IEnumerable<Vertice<T>>
{
    private readonly Dictionary<Vertice<T>, LinkedList<Vertice<T>>> _nodes = new();

    private int[]? _degreeDistribuition = null;

    public string Name { get; set; } = "undirected";

    public void AddVertice(Vertice<T> vertice)
    {
        if (_nodes.ContainsKey(vertice))
            return;

        _nodes.TryAdd(vertice, new LinkedList<Vertice<T>>());
    }

    public void AddEdge(Vertice<T> sourse, Vertice<T> destination)
    {
        if (!_nodes.ContainsKey(sourse) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the garph!");

        var sourceEdges = _nodes[sourse];
        var destinationEdges = _nodes[destination];

        Graph<T>.AddConnection(sourceEdges, destination);
        Graph<T>.AddConnection(destinationEdges, sourse);
    }

    public LinkedList<Vertice<T>> GetEdges(Vertice<T> vertice)
    {
        return _nodes[vertice];
    }

    public Vertice<T>? GetVerticeByIndex(int index)
    {
        foreach (var node in _nodes)
        {
            if (node.Key.Index == index)
                return node.Key;
        }

        return null;
    }

    #region Connections

    private static void AddConnection(LinkedList<Vertice<T>> edges, Vertice<T> vertice)
    {
        if (edges.Count == 0)
            edges.AddFirst(vertice);
        else
            edges.AddLast(vertice);
    }

    public bool IsConnected(Vertice<T> firstVertice, Vertice<T> secondVertice)
    {
        var edges = GetEdges(firstVertice);
        return edges.Contains(secondVertice);
    }

    #endregion

    #region Degree

    public int GetDegree(Vertice<T> vertice)
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

    public double GetCorrelationCoefficient()
    {
        double S1 = 0, S2 = 0, S3 = 0, Sl = 0;

        var added = new HashSet<(Vertice<T>, Vertice<T>)>();

        foreach (var item in _nodes)
        {
            int degree = GetDegree(item.Key);
            S1 += degree;
            S2 += (degree * degree);
            S3 += (degree * degree * degree);

            foreach (var connectedVertice in item.Value)
            {
                if (added.Contains((item.Key, connectedVertice)))
                    continue;

                Sl += (degree * GetDegree(connectedVertice));

                added.Add((connectedVertice, item.Key));
            }
        }

        Sl *= 2;

        double coeff = (S1 * Sl - S2 * S2) / (S1 * S3 - S2 * S2);

        return coeff;
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
