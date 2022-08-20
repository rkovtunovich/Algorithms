namespace Graphs.GraphImplementation;

public class UndirectedVariableEdgeLengthGraph : UndirectedGraph
{
    public UndirectedVariableEdgeLengthGraph(string name) : base(name)
    {
    }

    public override double GetEdgeLength(Vertice begin, Vertice end)
    {
        if (_edgesLengths.TryGetValue((begin, end), out double leangth))
            return leangth;

        return 0;
    }

    public override bool IsVariableEdgeLength()
    {
        return true;
    }
}
