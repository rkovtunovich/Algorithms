namespace Graphs.Core.Model.Graphs;

public class UndirectedGraph : GraphBase
{
    private int[]? _degreeDistribution = null;

    public UndirectedGraph(string name)
    {
        Name = name;
    }

    public override void AddEdge(Vertex source, Vertex destination)
    {
        if (!_nodes.ContainsKey(source))
            AddVertex(source);
            
        if (!_nodes.ContainsKey(destination))
            AddVertex(destination);

        var sourceEdges = _nodes[source];
        var destinationEdges = _nodes[destination];

        AddConnection(sourceEdges, destination);
        AddConnection(destinationEdges, source);
    }

    public override bool IsOriented()
    {
        return false;
    }

    public override GraphBase Transpose()
    {
        return this;
    }

    #region Degree

    public int[] GetDegreeDistributionsCount()
    {
        if (_degreeDistribution is not null)
            return _degreeDistribution;

        if (_nodes.Count == 0)
            return [];

        _degreeDistribution = new int[_nodes.Count];

        foreach (var node in _nodes)
        {
            _degreeDistribution[GetDegree(node.Key)]++;
        }

        return _degreeDistribution;
    }

    public double[] GetDegreeDistributionsFraction()
    {
        if (_nodes.Count == 0)
            return Array.Empty<double>();

        if (_degreeDistribution is null)
            _degreeDistribution = GetDegreeDistributionsCount();

        var digreeDistribuition = new double[_nodes.Count];

        for (int i = 0; i < _degreeDistribution.Length; i++)
        {
            digreeDistribuition[i] = (double)_degreeDistribution[i] / _nodes.Count;
        }

        return digreeDistribuition;
    }

    public double[] GetDegreeDistributionsCumulative()
    {
        var fracDegreeDistr = GetDegreeDistributionsFraction();

        var cumulativeDegreeDistr = new double[_nodes.Count];
        cumulativeDegreeDistr[0] = 1;

        for (int i = 1; i < fracDegreeDistr.Length; i++)
        {
            cumulativeDegreeDistr[i] = cumulativeDegreeDistr[i - 1] - fracDegreeDistr[i];
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

            var neighbors = GetAdjacentEdges(item.Key);

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

            var neighbors = GetAdjacentEdges(item.Key);

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

        if (_edgesLengths.Count is 0)
            return 1;

        if (_edgesLengths.TryGetValue((begin, end), out double length))
            return length;

        return 0;
    }

    public override void RemoveEdge(Vertex source, Vertex destination)
    {
        RemoveConnection(_nodes[source], destination);
        RemoveConnection(_nodes[destination], source);
    }
}
