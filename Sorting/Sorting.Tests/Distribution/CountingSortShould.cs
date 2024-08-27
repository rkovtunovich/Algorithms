namespace Sorting.Tests.Distribution;

public class CountingSortShould
{
    [Fact]
    public void Sort_WhenArrayIsEmpty_ShouldReturnEmptyArray()
    {
        // Arrange
        var array = Array.Empty<int>();
        var maxValue = 0;

        // Act
        CountingSort.Sort(array, maxValue);

        // Assert
        array.Should().BeEmpty();
    }

    [Fact]
    public void Sort_WhenArrayContainsSingleElement_ShouldReturnSameArray()
    {
        // Arrange
        var array = new int[] { 42 };
        var maxValue = 42;

        // Act
        CountingSort.Sort(array, maxValue);

        // Assert
        array.Should().Equal(42);
    }

    [Fact]
    public void Sort_WhenArrayContainsMultipleElements_ShouldReturnSortedArray()
    {
        // Arrange
        var array = new int[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };
        var maxValue = 9;

        // Act
        CountingSort.Sort(array, maxValue);

        // Assert
        array.Should().Equal(1, 1, 2, 3, 3, 4, 5, 5, 5, 6, 9);
    }

    [Fact]
    public void Sort_WhenArrayContainsZeroesAndOnes_ShouldReturnSortedArray()
    {
        // Arrange
        var array = new int[] { 0, 1, 0, 1, 0, 1, 0, 1 };
        var maxValue = 1;

        // Act
        CountingSort.Sort(array, maxValue);

        // Assert
        array.Should().Equal(0, 0, 0, 0, 1, 1, 1, 1);
    }
}
