using MathAlgo.Models;

namespace Sorting.Tests.QuickSort;

public class IntervalsFuzzySortShould
{
    [Fact]
    public void Sort_WhenArrayIsEmpty_ShouldReturnEmptyArray()
    {
        // Arrange
        var intervals = new List<Interval>();

        // Act
        IntervalsFuzzySort.Sort(intervals);

        // Assert
        intervals.Should().BeEmpty();
    }

    [Fact]
    public void Sort_WhenArrayHasOneElement_ShouldReturnArrayWithOneElement()
    {
        // Arrange
        var intervals = new List<Interval> { new(1, 1) };

        // Act
        IntervalsFuzzySort.Sort(intervals);

        // Assert
        intervals.Should().BeEquivalentTo(new Interval[] { new(1, 1) });
    }

    [Fact]
    public void Sort_WhenArrayHasMultipleElements_ShouldSortArray()
    {
        // Arrange
        var intervals = new List<Interval>
        {
            new(3, 5),
            new(1, 2),
            new(4, 6),
            new(1, 4),
            new(5, 7),
            new(9, 10),
            new(2, 4),
            new(6, 8),
            new(5, 7),
            new(3, 5),
            new(5, 6)
        };
        var originalIntervals = intervals.ToList();

        // Act
        IntervalsFuzzySort.Sort(intervals);

        // Assert
        AssertIntervalsAreSorted(intervals);
        intervals.Should().BeEquivalentTo(originalIntervals);
    }

    [Fact]
    public void Sort_WhenArrayHasMultipleElementsInDescendingOrder_ShouldSortArray()
    {
        // Arrange
        var intervals = new List<Interval>
        {
            new(9, 10),
            new(7, 8),
            new(5, 6),
            new(3, 4),
            new(1, 2)
        };

        // Act
        IntervalsFuzzySort.Sort(intervals);

        // Assert
        AssertIntervalsAreSorted(intervals);
    }

    private void AssertIntervalsAreSorted(List<Interval> intervals)
    {
        for (int i = 0; i < intervals.Count - 1; i++)
        {
            var isBefore = intervals[i].Before(intervals[i + 1]);
            var isOverlapping = intervals[i].Overlaps(intervals[i + 1]);

            var result = isBefore || isOverlapping;

            // should be before or overlapping
            result.Should().BeTrue();
        }
    }
}
