namespace Graphs.Tests.Application.Search;

public class MaxFlowAlgorithmsShould
{
    [Fact]
    public void FordFulkersonScaling_WhenAllCapacitiesZero_ShouldReturnZero()
    {
        // Arrange
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var s = new Vertex(1);
        var t = new Vertex(2);
        g.AddVertices(s, t);
        g.AddEdgeWithLength(s, t, 0); // zero capacity

        // Act
        var flow = MaxFlowAlgorithms.FordFulkersonScaling(g, s, t);

        // Assert
        flow.Should().Be(0);
    }

    [Fact]
    public void FordFulkersonScaling_WhenSingleEdge_ShouldReturnThatCapacity()
    {
        // Arrange
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var s = new Vertex(1);
        var t = new Vertex(2);
        g.AddVertices(s, t);
        g.AddEdgeWithLength(s, t, 5);

        // Act
        var flow = MaxFlowAlgorithms.FordFulkersonScaling(g, s, t);

        // Assert
        flow.Should().Be(5);
        g.GetEdgeLength(s, t).Should().Be(5, "original graph must remain unchanged");
    }

    [Fact]
    public void FordFulkersonScaling_WhenClassicCormenExample_ShouldReturn23()
    {
        // Arrange (CLRS example network)
        // Expected max flow = 23
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var s = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);
        var v5 = new Vertex(5);
        var t = new Vertex(6);
        g.AddVertices(s, v2, v3, v4, v5, t);
        g.AddEdgeWithLength(s, v2, 16);
        g.AddEdgeWithLength(s, v3, 13);
        g.AddEdgeWithLength(v2, v3, 10);
        g.AddEdgeWithLength(v3, v2, 4);
        g.AddEdgeWithLength(v2, v4, 12);
        g.AddEdgeWithLength(v4, v3, 9);
        g.AddEdgeWithLength(v3, v5, 14);
        g.AddEdgeWithLength(v5, v4, 7);
        g.AddEdgeWithLength(v4, t, 20);
        g.AddEdgeWithLength(v5, t, 4);

        // Act
        var flow = MaxFlowAlgorithms.FordFulkersonScaling(g, s, t);

        // Assert
        flow.Should().Be(23);
    }

    [Fact]
    public void FordFulkersonScaling_WhenFractionalCapacities_ShouldMatchEdmondsKarp()
    {
        // Arrange - small network with fractional capacities
        // Max flow reasoning: only paths into c (a->c 2.4) + (b->c 1.1) = 3.5, c->t = 6.3 (not limiting), so max flow = 3.5
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var s = new Vertex(1);
        var a = new Vertex(2);
        var b = new Vertex(3);
        var c = new Vertex(4);
        var t = new Vertex(5);
        g.AddVertices(s, a, b, c, t);
        g.AddEdgeWithLength(s, a, 5.7);
        g.AddEdgeWithLength(s, b, 3.2);
        g.AddEdgeWithLength(a, c, 2.4);
        g.AddEdgeWithLength(b, c, 1.1);
        g.AddEdgeWithLength(a, b, 1.5); // unused for additional flow due to bottleneck at b->c
        g.AddEdgeWithLength(c, t, 6.3);

        // Act
        var scaling = MaxFlowAlgorithms.FordFulkersonScaling(g, s, t);
        var ek = MaxFlowAlgorithms.EdmondsKarp(g, s, t);

        // Assert
        scaling.Should().BeApproximately(3.5, 1e-6);
        scaling.Should().BeApproximately(ek, 1e-6);
    }

    [Fact]
    public void FordFulkersonScaling_WhenNoPathFromSourceToSink_ShouldReturnZero()
    {
        // Arrange
        var g = new OrientedGraph("G", trackIncomeEdges: true);
        var s = new Vertex(1);
        var a = new Vertex(2);
        var t = new Vertex(3);
        g.AddVertices(s, a, t);
        g.AddEdgeWithLength(s, a, 10);
        // no edge to t

        // Act
        var flow = MaxFlowAlgorithms.FordFulkersonScaling(g, s, t);

        // Assert
        flow.Should().Be(0);
    }
}
