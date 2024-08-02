namespace Searching.Tests.Common;

public class LocalMatrixMinimumShould
{
    [Fact]
    public void Find_WhenSingleElement()
    {
        // Arrange
        var matrix = new int[][]
        {
            [1]
        };
        var expected = new List<int[]> { new int[] { 0, 0, 1 } };

        // Act
        var actual = LocalMatrixMinimum.Find(matrix);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Find_WhenTwoByTwoMatrix()
    {
        // Arrange
        var matrix = new int[][]
        {
            [1, 2],
            [3, 4]
        };
        var expected = new List<int[]> { new int[] { 0, 0, 1 } };

        // Act
        var actual = LocalMatrixMinimum.Find(matrix);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Find_WhenThreeByThreeMatrix()
    {
        // Arrange
        var matrix = new int[][]
        {
            [1, 2, 3],
            [4, 5, 6],
            [7, 8, 9]
        };
        var expected = new List<int[]> { new int[] { 0, 0, 1 } };

        // Act
        var actual = LocalMatrixMinimum.Find(matrix);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Find_WhenThreeByThreeMatrixWithLocalMinimum()
    {
        // Arrange
        var matrix = new int[][]
        {
            [1, 2, 3],
            [4, 0, 6],
            [7, 8, 9]
        };
        var expected = new List<int[]> { new int[] { 1, 1, 0 } };

        // Act
        var actual = LocalMatrixMinimum.Find(matrix);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}