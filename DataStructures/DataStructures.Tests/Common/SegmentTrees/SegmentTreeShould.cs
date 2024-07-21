using DataStructures.Common.SegmentTrees;

namespace DataStructures.Tests.Common.SegmentTrees;

public class SegmentTreeShould
{
    [Fact]
    public void BuildSegmentTree_WhenArrayIsGiven()
    {
        // Arrange
        var array = new int[] { 1, 3, 5, 7, 9, 11 };

        // Act
        var segmentTree = new SegmentTree<int>(array);

        // Assert
        segmentTree.Should().NotBeNull();
        segmentTree.Values.Should().BeEquivalentTo(new int[] { 0, 36, 32, 4, 12, 20, 1, 3, 5, 7, 9, 11 });
    }

    [Fact]
    public void UpdateSegmentTree_WhenIndexAndValueAreGiven()
    {
        // Arrange
        var array = new int[] { 1, 3, 5, 7, 9, 11 };
        var segmentTree = new SegmentTree<int>(array);

        // Act
        segmentTree.Update(2, 6);

        // Assert
        segmentTree.Values.Should().BeEquivalentTo(new int[] { 0, 37, 33, 4, 13, 20, 1, 3, 6, 7, 9, 11 });
    }

    [Fact]
    public void QuerySegmentTree_WhenLeftAndRightIndicesAreGiven()
    {
        // Arrange
        var array = new int[] { 1, 3, 5, 7, 9, 11 };
        var segmentTree = new SegmentTree<int>(array);

        // Act
        var result = segmentTree.Query(1, 3);

        // Assert
        result.Should().Be(15);
    }
}
