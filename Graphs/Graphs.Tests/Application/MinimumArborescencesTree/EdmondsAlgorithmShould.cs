using Graphs.Application.MinimumArborescenceTrees;

namespace Graphs.Tests.Application.MinimumArborescencesTree;

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
        var (tree, minimumCost) = EdmondsAlgorithm.FindArborescenceTree(graph);

        // assert
        tree.Should().NotBeNull();
        tree.Should().HaveCount(8);

        minimumCost.Should().Be(31);
    }
}
