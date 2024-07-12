namespace Sorting.Tests.Common;
public class QuickSortShould
{
    [Fact]
    public void Sort_WhenArrayIsEmpty_ShouldNotThrowException()
    {
        // Arrange
        var array = Array.Empty<int>();

        // Act
        QuickSort.Sort(array);

        // Assert
        array.Should().BeEmpty();
    }

    [Fact]
    public void Sort_WhenArrayHasOneElement_ShouldNotThrowException()
    {
        // Arrange
        var array = new int[] { 1 };

        // Act
        QuickSort.Sort(array);

        // Assert
        array.Should().BeEquivalentTo(new int[] { 1 });
    }

    [Fact]
    public void Sort_WhenArrayHasMultipleElements_ShouldSortArray()
    {
        // Arrange
        var array = new int[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };

        // Act
        QuickSort.Sort(array);

        // Assert
        array.Should().BeInAscendingOrder();
    }

    [Fact]
    public void Sort_WhenArrayHasMultipleElementsInDescendingOrder_ShouldSortArray()
    {
        // Arrange
        var array = new int[] { 9, 7, 5, 3, 1 };

        // Act
        QuickSort.Sort(array, true);

        // Assert
        array.Should().BeInDescendingOrder();
    }
}
