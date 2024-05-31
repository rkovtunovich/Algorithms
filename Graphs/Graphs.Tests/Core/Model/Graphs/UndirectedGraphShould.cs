namespace Graphs.Tests.Core.Model.Graphs;

public class UndirectedGraphShould
{
    [Fact]
    public void SearchCycle_OrientedGraphWithSycle_CycleFound()
    {
        // arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "SerializedGraphs", $"{nameof(SearchCycle_OrientedGraphWithSycle_CycleFound)}.txt");
        var serializedGraph = GraphFileManager.ReadFromFile(path);
        var serializer = new DOTSerializer();
        var graph = serializer.Deserialize(serializedGraph);

        // act
        var cycle = graph.SearchCycle();

        // assert
        cycle.Should().NotBeNull();
        cycle.Should().HaveCount(4);
        cycle.Should().Contain(new Vertex(4));
        cycle.Should().Contain(new Vertex(5));
        cycle.Should().Contain(new Vertex(7));
        cycle.Should().Contain(new Vertex(2));
    }
}
