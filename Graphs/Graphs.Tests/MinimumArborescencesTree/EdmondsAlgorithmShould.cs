using Graphs.Core.MinimumArborescencesTree;

namespace Graphs.Tests.MinimumArborescencesTree;

public class EdmondsAlgorithmShould
{
    [Fact]
    public void FindArborescencesTree_OrientedGraphWithSycle_TreeWithMinimumCost()
    {
        // arrange
        var path = Path.Combine(Directory.GetCurrentDirectory(), "SerializedGraphs", $"{nameof(FindArborescencesTree_OrientedGraphWithSycle_TreeWithMinimumCost)}.txt");
        var serializedGraph = GraphFileManager.ReadFromFile(path);
        var serializer = new DOTSerializer();
        var graph = serializer.Deserialize(serializedGraph) as OrientedGraph ?? throw new NullReferenceException();
        graph.FillIncomeEdges(true);

        // act
        var (tree, minimumCost) = EdmondsAlgorithm.FindArborescencesTree(graph);

        // assert
        tree.Should().NotBeNull();
        tree.Should().HaveCount(8);

        minimumCost.Should().Be(31);
    }
}
