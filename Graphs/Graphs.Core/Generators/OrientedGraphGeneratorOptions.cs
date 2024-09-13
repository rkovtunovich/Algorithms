namespace Graphs.Core.Generators;

public class OrientedGraphGeneratorOptions : GraphGeneratorOptions
{
    /// <summary>
    /// Satisfy two conditions:
    /// 1. Each edge goes from a vertex with a smaller index to a vertex with a larger index (vi, vj) => i < j.
    /// 2. Each vertex except vn has at least one edge leaving it 
    /// </summary>
    public bool IsOrdered { get; set; }

    /// <summary>
    /// If true, the graph will keep track of incoming edges for each vertex.
    /// </summary>
    public bool TrackIncomeEdges { get; set; }
}
