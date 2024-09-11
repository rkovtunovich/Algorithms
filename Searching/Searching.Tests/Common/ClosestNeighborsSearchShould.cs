namespace Searching.Tests.Common;

public class ClosestNeighborsSearchShould
{
    [Fact]
    public void Find_WhenSequenceIsEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        List<int> sequence = [];
        int target = 5;
        int k = 3;

        // Act
        List<int> result = ClosestNeighborsSearch.Find(sequence, target, k);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Find_WhenSequenceHasElements_ShouldReturnClosestNeighbors()
    {
        // Arrange
        List<int> sequence = [1, 3, 5, 7, 9];
        int target = 5;
        int k = 2;

        // Act
        List<int> result = ClosestNeighborsSearch.Find(sequence, target, k);

        // Assert
        result.Should().BeEquivalentTo(new List<int> { 3, 5, 7 });
    }
}
