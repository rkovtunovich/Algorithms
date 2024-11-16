namespace Graphs.Tests.Core.Model.Graphs;

public class UndirectedGraphShould
{
    [Fact]
    public void IntersectWith_WhenGraphsHaveCommonVertices_ShouldLeaveOnlyCommonVerticesAndEdges()
    {
        // Arrange
        var graph1 = new UndirectedGraph("graph1");
        var graph2 = new UndirectedGraph("graph2");
        var vertex1 = new Vertex(1);
        var vertex2 = new Vertex(2);
        var vertex3 = new Vertex(3);
        var vertex4 = new Vertex(4);
        var vertex5 = new Vertex(5);

        graph1.AddVertices(vertex1, vertex2, vertex3, vertex4);
        graph2.AddVertices(vertex3, vertex4, vertex5);

        graph1.AddEdge(vertex1, vertex2);
        graph1.AddEdge(vertex2, vertex3);
        graph1.AddEdge(vertex3, vertex4);

        graph2.AddEdge(vertex3, vertex4);
        graph2.AddEdge(vertex4, vertex5);

        // Act
        graph1.IntersectWith(graph2);

        // assert
        graph1.Should().NotBeNull();
        graph1.Count.Should().Be(2);
        graph1.Should().Contain(vertex3);
        graph1.Should().Contain(vertex4);
        graph1.Should().NotContain(vertex1);
        graph1.Should().NotContain(vertex2);
        graph1.Should().NotContain(vertex5);
        graph1.IsConnected(vertex3, vertex4).Should().BeTrue();
    }

    [Fact]
    public void SearchCycle_WhenOrientedGraphWithSycle_CycleFound()
    {
        // Arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "SerializedGraphs", $"{nameof(SearchCycle_WhenOrientedGraphWithSycle_CycleFound)}.txt");
        var serializedGraph = GraphFileManager.ReadFromFile(path);
        var serializer = new DOTSerializer();
        var graph = serializer.Deserialize(serializedGraph);

        // Act
        var cycle = graph.SearchCycle();

        // Assert
        cycle.Should().NotBeNull();
        cycle.Should().HaveCount(4);
        cycle.Should().Contain(new Vertex(4));
        cycle.Should().Contain(new Vertex(5));
        cycle.Should().Contain(new Vertex(7));
        cycle.Should().Contain(new Vertex(2));
    }
}
