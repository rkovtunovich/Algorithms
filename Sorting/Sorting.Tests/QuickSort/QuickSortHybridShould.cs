using Sorting.QuickSort;

namespace Sorting.Tests.QuickSort;

public class QuickSortHybridShould
{
    [Fact]
    public void SortArrayUsingQuickSortHybrid()
    {
        // Arrange
        var array = new int[] { 3, 7, 4, 9, 5, 2, 6, 1 };
        var expected = new int[] { 1, 2, 3, 4, 5, 6, 7, 9 };

        // Act
        QuickSortHybrid.Sort(array);

        // Assert
        array.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void SortListUsingQuickSortHybrid()
    {
        // Arrange
        var list = new List<int> { 3, 7, 4, 9, 5, 2, 6, 1 };
        var expected = new List<int> { 1, 2, 3, 4, 5, 6, 7, 9 };

        // Act
        QuickSortHybrid.Sort(ref list);

        // Assert
        list.Should().BeEquivalentTo(expected);
    }
}
