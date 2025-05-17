
namespace Graphs.Tests.Application.Search;

public class ShortestPathCounterShould
{
    [Fact]
    public void SingleVertexGraph_ReturnsOne()
    {
        // Arrange
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var v = new Vertex(1);
        g.AddVertex(v);

        // Act
        long count = ShortestPathCounter.CountShortestPaths(g, v, v);

        // Assert
        count.Should().Be(1, "there is exactly one trivial path from a node to itself");
    }

    [Fact]
    public void SingleEdgeGraph_ReturnsOne()
    {
        // 1 → 2
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        g.AddVertex(v1);
        g.AddVertex(v2);
        g.AddEdgeWithLength(v1, v2, length: 5);

        long count = ShortestPathCounter.CountShortestPaths(g, v1, v2);
        count.Should().Be(1, "only one direct shortest path exists");
    }

    [Fact]
    public void TwoParallelPaths_ReturnsTwo()
    {
        // Build:
        // 1 → 2 → 4
        // 1 → 3 → 4
        // all edge weights = 1, so both have total length 2
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        g.AddVertices(v1, v2, v3, v4);

        g.AddEdgeWithLength(v1, v2, 1);
        g.AddEdgeWithLength(v2, v4, 1);
        g.AddEdgeWithLength(v1, v3, 1);
        g.AddEdgeWithLength(v3, v4, 1);

        long count = ShortestPathCounter.CountShortestPaths(g, v1, v4);
        count.Should().Be(2, "there are exactly two distinct shortest paths (via 2 and via 3)");
    }

    [Fact]
    public void WeightedGraph_PicksOnlyShortest_ReturnsOne()
    {
        // 1→2→4 cost 1+1=2
        // 1→3→4 cost 2+2=4
        // only the first is shortest
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        g.AddVertices(v1, v2, v3, v4);

        g.AddEdgeWithLength(v1, v2, 1);
        g.AddEdgeWithLength(v2, v4, 1);
        g.AddEdgeWithLength(v1, v3, 2);
        g.AddEdgeWithLength(v3, v4, 2);

        long count = ShortestPathCounter.CountShortestPaths(g, v1, v4);
        count.Should().Be(1, "only the path via 2+4 with total cost 2 is minimal");
    }

    [Fact]
    public void NegativeEdges_NoNegativeCycles_CorrectlyHandlesMultipleShortest()
    {
        // 1→2→4 with weights -1 + -2 = -3
        // 1→3→4 with weights -2 + -1 = -3
        // both have total cost -3 ⇒ two shortest
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        g.AddVertices(v1, v2, v3, v4);

        g.AddEdgeWithLength(v1, v2, -1);
        g.AddEdgeWithLength(v2, v4, -2);
        g.AddEdgeWithLength(v1, v3, -2);
        g.AddEdgeWithLength(v3, v4, -1);

        long count = ShortestPathCounter.CountShortestPaths(g, v1, v4);
        count.Should().Be(2, "there are two distinct negative‐cost shortest paths");
    }
}
