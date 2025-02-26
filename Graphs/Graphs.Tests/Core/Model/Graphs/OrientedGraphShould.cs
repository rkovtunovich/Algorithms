namespace Graphs.Tests.Core.Model.Graphs;

public class OrientedGraphShould
{
    [Fact]
    public void FindRoot_WhenEmptyGraph_ThrowsException()
    {
        // Arrange
        var graph = new OrientedGraph("EmptyGraph");

        // Act
        Action act = () => graph.FindRoot();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The graph has no vertices, cannot determine root.");
    }

    [Fact]
    public void FindRoot_WhenSingleVertex_ReturnsVertex()
    {
        // Arrange
        var graph = new OrientedGraph("SingleVertex");
        var v1 = new Vertex(1);
        graph.AddVertex(v1);

        // Act
        var root = graph.FindRoot();

        // Assert
        root.Should().Be(v1);
    }

    [Fact]
    public void FindRoot_WhenTwoVertices_ReturnsRoot()
    {
        // Arrange
        var graph = new OrientedGraph("TwoVertices");
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);

        graph.AddVertices(v1, v2);
        graph.AddEdge(v1, v2);

        // Act
        var root = graph.FindRoot();

        // Assert
        root.Should().Be(v1);
    }

    [Fact]
    public void FindRoot_WhenTwoVerticesWithIncomingEdges_ReturnsRoot()
    {
        // Arrange
        var graph = new OrientedGraph("TwoVerticesWithIncomingEdges", trackIncomeEdges: true);
        var v1 = new Vertex(1);
        var v2 = new Vertex(2);

        graph.AddVertices(v1, v2);
        graph.AddEdge(v1, v2);
        graph.FillIncomeEdges();
        
        // Act
        var root = graph.FindRoot();
        
        // Assert
        root.Should().Be(v1);
    }
}
