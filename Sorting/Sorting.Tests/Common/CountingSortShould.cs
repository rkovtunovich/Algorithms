namespace Sorting.Tests.Common;

public class CountingSortShould
{
    [Fact]
    public void SortArrayInAscendingOrder()
    {
        // Arrange
        var array = new int[] { 4, 2, 2, 8, 3, 3, 1 };
        var maxValue = 8;

        // Act
        CountingSort.Sort(array, maxValue);

        // Assert
        array.Should().BeInAscendingOrder();
    }
}
