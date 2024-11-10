namespace Graphs.Tests.Application.Search;

public class BellmanFordAlgorithmShould
{
    [Fact]
    public void FindShortestPaths_WhenGraphHasNegativeCycle_ShouldThrowException()
    {
        // Arrange
        var graph = new OrientedGraph("test", true);
        var vertex1 = new Vertex(1);
        var vertex2 = new Vertex(2);
        var vertex3 = new Vertex(3);
        var vertex4 = new Vertex(4);
        var vertex5 = new Vertex(5);
        graph.AddVertices(vertex1, vertex2, vertex3, vertex4, vertex5);
        graph.AddEdgeWithLength(vertex1, vertex2, 1);
        graph.AddEdgeWithLength(vertex2, vertex3, 2);
        graph.AddEdgeWithLength(vertex3, vertex4, 3);
        graph.AddEdgeWithLength(vertex4, vertex5, 4);
        graph.AddEdgeWithLength(vertex5, vertex1, -11);

        // Act
        Action act = () => BellmanFordAlgorithm.FindShortestPaths(graph, vertex1);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Graph contains a negative cycle");
    }

    [Fact]
    public void FindShortestPaths_WhenGraphHasNoNegativeCycle_ShouldReturnShortestPaths()
    {
        // Arrange
        var graph = new OrientedGraph("test", true);
        var vertex1 = new Vertex(1);
        var vertex2 = new Vertex(2);
        var vertex3 = new Vertex(3);
        var vertex4 = new Vertex(4);
        var vertex5 = new Vertex(5);
        graph.AddVertices(vertex1, vertex2, vertex3, vertex4, vertex5);
        graph.AddEdgeWithLength(vertex1, vertex2, 1);
        graph.AddEdgeWithLength(vertex2, vertex3, 2);
        graph.AddEdgeWithLength(vertex3, vertex4, 3);
        graph.AddEdgeWithLength(vertex4, vertex5, 4);

        // Act
        var result = BellmanFordAlgorithm.FindShortestPaths(graph, vertex1);

        // Assert
        result.Should().BeEquivalentTo(
        [
            (0, null),
            (1, vertex1),
            (3, vertex2),
            (6, vertex3),
            (10, vertex4)
        ]);
    }
}
