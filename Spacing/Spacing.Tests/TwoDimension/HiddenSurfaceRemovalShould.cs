namespace Spacing.Tests.TwoDimension;

public class HiddenSurfaceRemovalShould
{
    [Fact]
    public void FindVisibleLines_WhenNoIntersectingLines_ShouldReturnAllLines()
    {
        // Arrange
        var lines = new List<Line>
        {
            new(1, 1),
            new(1, 2),
            new(1, 3),
        };

        // Act
        var visibleLines = HiddenSurfaceRemoval.FindVisibleLines(lines);

        // Assert
        // Only the line with the highest intercept is visible.
        // Line 3: y = 1x + 3
        visibleLines.Should().BeEquivalentTo(new List<Line>
        {
            new(1, 3),
        });
    }

    [Fact]
    public void FindVisibleLines_WhenSingleIntersectingLine_ShouldReturnVisibleLines()
    {
        // Arrange
        var lines = new List<Line>
        {
            new(2, 1),
            new(1, 3),
        };

        // Act
        var visibleLines = HiddenSurfaceRemoval.FindVisibleLines(lines);

        // Assert
        visibleLines.Should().BeEquivalentTo(lines);
    }

    [Fact]
    public void FindVisibleLines_WhenMultipleIntersectingLines_ShouldReturnVisibleLines()
    {
        // Arrange
        var lines = new List<Line>
        {
            new(3, 1),
            new(-1, 5),
            new(0.5, 2),
        };

        // Act
        var visibleLines = HiddenSurfaceRemoval.FindVisibleLines(lines);

        // Assert
        visibleLines.Should().BeEquivalentTo(new List<Line>
        {
            new(3, 1),
            new (-1, 5)
        });
    }

    [Fact]
    public void FindVisibleLines_WhenParallelLinesWithOneIntersectingLine_ShouldReturnVisibleLines()
    {
        // Arrange
        var lines = new List<Line>
        {
            new(1, 2),
            new(1, 1),
            new(-1, 4),
        };

        // Act
        var visibleLines = HiddenSurfaceRemoval.FindVisibleLines(lines);

        // Assert
        // Line 3 is visible and one of the parallel lines with the highest intercept is visible
        visibleLines.Should().BeEquivalentTo(new List<Line>
        {
            new(-1, 4),
            new(1, 2),
        });
    }

    [Fact]
    public void FindVisibleLines_WhenMultipleIntersectingAllVisibleLines_ShouldReturnVisibleLines()
    {
        // Arrange
        var lines = new List<Line>
        {
            new(1, 2),
            new(2, 1),
            new(-0.5, 3),
            new(3, -1),
        };

        // Act
        var visibleLines = HiddenSurfaceRemoval.FindVisibleLines(lines);

        // Assert
        // All lines are visible
        visibleLines.Should().BeEquivalentTo(lines);
    }
}
