namespace Sorting.Tests.Common;

public class HeapSortShould
{
    [Fact]
    public void SortAnArrayOfKeysUsingAHeap()
    {
        // Arrange
        var array = new[] { 7, 13, 4, 3, 4, 7 };

        // Act
        HeapSort<int, string>.Sort(array);

        // Assert
        array.Should().BeInAscendingOrder();
    }

    [Fact]
    public void SortAnArrayOfKeysUsingAHeapWithNegativeNumbers()
    {
        // Arrange
        var array = new[] { -7, -13, -4, -3, -4, -7 };

        // Act
        HeapSort<int, string>.Sort(array);

        // Assert
        array.Should().BeInAscendingOrder();
    }

    [Fact]
    public void SortAnArrayOfKeysUsingAHeapWithRandomNumbers()
    {
        // Arrange
        var array = new[] { 7, -13, 4, -3, 4, -7 };

        // Act
        HeapSort<int, string>.Sort(array);

        // Assert
        array.Should().BeInAscendingOrder();
    }

    [Fact]
    public void SortAnArrayOfKeysUsingAHeapWithLargeNumbers()
    {
        // Arrange
        var array = new[] { 7000000, 13000000, 4000000, 3000000, 4000000, 7000000 };

        // Act
        HeapSort<int, string>.Sort(array);

        // Assert
        array.Should().BeInAscendingOrder();
    }
}
