using Graphs.Application.SteinerTrees;

namespace Graphs.Tests.Application.SteinerTrees;

public class SteinerTreeSearchShould
{
    [Fact]
    public void ReturnZeroWhenSingleTerminal()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("g");
        var v1 = new Vertex(1);

        // Act
        var weight = SteinerTreeSearch.FindMinimumSteinerTreeWeight(graph, [v1]);

        // Assert
        weight.Should().Be(0);
    }

    [Fact]
    public void ReturnEdgeWeightForTwoTerminals()
    {
        // Arrange
        var graph = new UndirectedVariableEdgeLengthGraph("g");
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        graph.AddEdge(v1, v2, 2);

        // Act
        var weight = SteinerTreeSearch.FindMinimumSteinerTreeWeight(graph, [v1, v2]);

        // Assert
        weight.Should().Be(2);
    }

    [Fact]
    public void UseOnlyTerminalsWhenCheaper()
    {
        // Arrange
        var g = new UndirectedVariableEdgeLengthGraph("g");
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);

        g.AddEdge(v1, v2, 1);
        g.AddEdge(v2, v3, 1);
        g.AddEdge(v1, v3, 2);
        g.AddEdge(v1, v4, 2);
        g.AddEdge(v2, v4, 2);
        g.AddEdge(v3, v4, 2);

        // Act
        var weight = SteinerTreeSearch.FindMinimumSteinerTreeWeight(g, [v1, v2, v3]);

        // Assert
        weight.Should().Be(2);
    }

    [Fact]
    public void UseSteinerVertexWhenBeneficial()
    {
        // Arrange
        var g = new UndirectedVariableEdgeLengthGraph("g");
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);
        var v3 = new Vertex(3);
        var v4 = new Vertex(4);

        g.AddEdge(v1, v2, 2);
        g.AddEdge(v1, v3, 2);
        g.AddEdge(v2, v3, 2);
        g.AddEdge(v1, v4, 1);
        g.AddEdge(v2, v4, 1);
        g.AddEdge(v3, v4, 1);

        // Act
        var weight = SteinerTreeSearch.FindMinimumSteinerTreeWeight(g, [v1, v2, v3]);

        // Assert
        weight.Should().Be(3);
    }
}

