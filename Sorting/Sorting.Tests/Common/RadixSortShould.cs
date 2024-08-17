namespace Sorting.Tests.Common;

public class RadixSortShould
{
    [Fact]
    public void Sort_WhenUnsortedArray_ReturnsSortedArray()
    {
        // Arrange
        int[] arr = { 170, 45, 75, 90, 802, 24, 2, 66 };

        // Act
        RadixSort.Sort(arr);

        // Assert
        arr.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Sort_WhenArrayWithDuplicates_ReturnsSortedArray()
    {
        // Arrange
        int[] arr = { 170, 45, 75, 90, 802, 24, 2, 66, 24 };

        // Act
        RadixSort.Sort(arr);

        // Assert
        arr.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Sort_WhenArrayWithSingleElement_ReturnsSortedArray()
    {
        // Arrange
        int[] arr = { 170 };

        // Act
        RadixSort.Sort(arr);

        // Assert
        arr.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Sort_WhenEmptyArray_ReturnsEmptyArray()
    {
        // Arrange
        int[] arr = Array.Empty<int>();

        // Act
        RadixSort.Sort(arr);

        // Assert
        arr.Should().BeEmpty();
    }

    [Fact]
    public void Sort_WhenArrayWithTwoElements_ReturnsSortedArray()
    {
        // Arrange
        int[] arr = { 170, 45 };

        // Act
        RadixSort.Sort(arr);

        // Assert
        arr.Should().BeInAscendingOrder();
    }
}
