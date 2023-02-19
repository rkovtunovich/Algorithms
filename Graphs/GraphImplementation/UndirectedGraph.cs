using Graphs.Abstraction;
using Graphs.Model;

namespace Graphs.GraphImplementation;

public class UndirectedGraph : Graph
{
    private int[]? _degreeDistribuition = null;

    public UndirectedGraph(string name)
    {
        Name = name;
    }

    public override void AddEdge(Vertex sourse, Vertex destination)
    {
        if (!_nodes.ContainsKey(sourse) || !_nodes.ContainsKey(destination))
            throw new Exception("this vertices isn't included in the garph!");

        var sourceEdges = _nodes[sourse];
        var destinationEdges = _nodes[destination];

        AddConnection(sourceEdges, destination);
        AddConnection(destinationEdges, sourse);
    }

    public override bool IsOriented()
    {
        return false;
    }

    public override Graph Transpose()
    {
        return this;
    }

    #region Degree

    public int[] GetDedreeDistributionsCount()
    {
        if (_degreeDistribuition is not null)
            return _degreeDistribuition;

        if (_nodes.Count == 0)
            return Array.Empty<int>();

        _degreeDistribuition = new int[_nodes.Count];

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

        var digreeDistribuition = new double[_nodes.Count];

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

        var added = new HashSet<(Vertex, Vertex)>();

        foreach (var item in _nodes)
        {
            int degree = GetDegree(item.Key);
            S1 += degree;
            S2 += degree * degree;
            S3 += degree * degree * degree;

            foreach (var connectedVertice in item.Value)
            {
                if (added.Contains((item.Key, connectedVertice)))
                    continue;

                Sl += degree * GetDegree(connectedVertice);

                added.Add((connectedVertice, item.Key));
            }
        }

        Sl *= 2;

        double coeff = (S1 * Sl - S2 * S2) / (S1 * S3 - S2 * S2);

        return coeff;
    }

    #endregion

    public void CalculateLocalClusteringCoefficient()
    {
        foreach (var item in _nodes)
        {
            var degree = GetDegree(item.Key);

            var neighbors = GetEdges(item.Key);

            var pairs = from item1 in neighbors
                        from item2 in neighbors
                        where item1.Index < item2.Index
                        select Tuple.Create(item1, item2);

            double connectedNeighbors = 0;
            foreach (var pair in pairs)
            {
                if (IsConnected(pair.Item1, pair.Item2))
                    connectedNeighbors++;
            }

            item.Key.LocalClusteringCoefficient = !pairs.Any() ? 0 : connectedNeighbors / pairs.Count();
        }
    }

    public double CalculateOverallClusteringCoefficient()
    {
        double connectedTriplets = 0;
        double numberOfTriangles = 0;

        foreach (var item in _nodes)
        {
            double degree = GetDegree(item.Key);

            connectedTriplets += degree / 2 * (degree - 1);

            var neighbors = GetEdges(item.Key);

            var pairs = from item1 in neighbors
                        from item2 in neighbors
                        where item1.Index < item2.Index
                        select Tuple.Create(item1, item2);

            foreach (var pair in pairs)
            {
                if (IsConnected(pair.Item1, pair.Item2))
                    numberOfTriangles++;
            }
        }

        double coeff = connectedTriplets == 0 ? 0 : numberOfTriangles * 3 / connectedTriplets;

        return coeff;
    }

    public override double GetEdgeLength(Vertex begin, Vertex end)
    {
        return 1;
    }

    public override void RemoveEdge(Vertex sourse, Vertex destination)
    {
        RemoveConnection(_nodes[sourse], destination);
        RemoveConnection(_nodes[destination], sourse);
    }
}
