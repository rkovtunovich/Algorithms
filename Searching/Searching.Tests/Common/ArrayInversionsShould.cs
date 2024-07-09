namespace Searching.Tests.Common;

public class ArrayInversionsShould
{
    [Fact]
    public void Count_WhenArrayHasInversions_ShouldReturnInversionsCount()
    {
        // Arrange
        var array = new int[] { 2, 3, 8, 6, 1 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void Count_WhenArrayIsEmpty_ShouldReturnZero()
    {
        // Arrange
        var array = Array.Empty<int>();

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Count_WhenArrayHasOneElement_ShouldReturnZero()
    {
        // Arrange
        var array = new int[] { 1 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Count_WhenArrayIsSorted_ShouldReturnZero()
    {
        // Arrange
        var array = new int[] { 1, 2, 3, 4, 5 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Count_WhenArrayReversed_ShouldReturnInversionsCount()
    {
        // Arrange
        var array = new int[] { 5, 4, 3, 2, 1 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public void Count_WhenArrayHasDuplicates_ShouldReturnInversionsCount()
    {
        // Arrange
        var array = new int[] { 1, 2, 1, 2, 1 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    public void Count_WhenArrayHasNegativeNumbers_ShouldReturnInversionsCount()
    {
        // Arrange
        var array = new int[] { -1, -2, -3, -4, -5 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public void Count_WhenArrayHasZero_ShouldReturnInversionsCount()
    {
        // Arrange
        var array = new int[] { 0, 1, 2, 3, 4, 5 };

        // Act
        var result = ArrayInversions.Count(ref array);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void Count_WhenArrayHasInversionsWithMetric_ShouldReturnInversionsCount()
    {
        // Arrange
        var array = new int[] { 2, 3, 8, 6, 1 };
        var metric = new Func<int, int>(x => 2 * x); // ai > 2 * aj

        // Act
        var result = ArrayInversions.Count(ref array, metric);

        // Assert
        result.Should().Be(3);
    }
}
