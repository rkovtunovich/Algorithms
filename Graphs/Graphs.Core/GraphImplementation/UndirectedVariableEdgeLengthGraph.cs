using Graphs.Core.Model;

namespace Graphs.Core.GraphImplementation;

public class UndirectedVariableEdgeLengthGraph : UndirectedGraph
{
    public UndirectedVariableEdgeLengthGraph(string name) : base(name)
    {
    }

    public void AddEdge(Vertex source, Vertex destination, double length)
    {
        AddEdge(source, destination);
        SetEdgeLength(source, destination, length);
        SetEdgeLength(destination, source, length);
    }

    public override double GetEdgeLength(Vertex begin, Vertex end)
    {
        if (_edgesLengths.TryGetValue((begin, end), out double length))
            return length;

        return 0;
    }

    public Dictionary<(Vertex, Vertex), double> GetEdgesLength()
    {
        return _edgesLengths;
    }

    public override bool IsVariableEdgeLength()
    {
        return true;
    }
}
