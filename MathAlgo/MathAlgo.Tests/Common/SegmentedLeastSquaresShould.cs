using Spacing.Core.Points;

namespace MathAlgo.Tests.Common;
public class SegmentedLeastSquaresShould
{
    [Fact]
    public void Calculate_WhenTwoPoints_ReturnsCorrectLines()
    {
        var points = new List<Point2D>
        {
            new(0, 0),
            new(1, 1),
        };

        var lines = SegmentedLeastSquares.Calculate(points);

        lines.Should().ContainSingle();
        lines[0].Slope.Should().BeApproximately(1, 1e-6);
        lines[0].YIntercept.Should().BeApproximately(0, 1e-6);
    }

    [Fact]
    public void Calculate_ReturnsCorrectLines()
    {
        var points = new List<Point2D>
        {
            new(0, 0),
            new(1, 1),
            new(2, 2),
            new(3, 3),
            new(4, 4),
            new(5, 5),
            new(6, 6),
            new(7, 7),
            new(8, 8),
            new(9, 9),
        };

        var lines = SegmentedLeastSquares.Calculate(points);

        lines.Should().ContainSingle();
        lines[0].Slope.Should().BeApproximately(1, 1e-6);
        lines[0].YIntercept.Should().BeApproximately(0, 1e-6);
    }

    [Fact]
    public void Calculate_WhenVerticalLine_ReturnsCorrectLines()
    {
        // Arrange
        var points = new List<Point2D>
        {
            new (1, 2),
            new (1, 4),
            new (1, 6),
        };

        // Act
        var lines = SegmentedLeastSquares.Calculate(points);

        // Assert
        lines.Should().ContainSingle();
        lines[0].IsVertical.Should().BeTrue();
        lines[0].VerticalX.Should().Be(1);
    }

    [Fact]
    public void Calculate_WhenHorizontalLine_ReturnsCorrectLines()
    {
        // Arrange
        var points = new List<Point2D>
        {
            new (1, 2),
            new (2, 2),
            new (3, 2),
        };

        // Act
        var lines = SegmentedLeastSquares.Calculate(points);

        // Assert
        lines.Should().ContainSingle();
        lines[0].Slope.Should().Be(0);
        lines[0].YIntercept.Should().Be(2);
    }

    [Fact]
    public void Calculate_WhenTwoVerticalSegments_ReturnsCorrectLines()
    {
        // Arrange
        var points = new List<Point2D>
        {
            new (1, 1),
            new ( 4, 4),
            new ( 1, 4),
            new ( 4, 1),

        };

        // Act
        var lines = SegmentedLeastSquares.Calculate(points);

        // Assert
        lines.Should().HaveCount(2);
        lines[0].IsVertical.Should().BeTrue();
        lines[0].VerticalX.Should().Be(1);
        lines[1].IsVertical.Should().BeTrue();
        lines[1].VerticalX.Should().Be(4);
    }

    [Fact]
    public void Calculate_WhenTwoSegmentsThatIncreaseAndDecrease_ReturnsCorrectLines()
    {
        // Arrange
        var points = new List<Point2D>
        {
            new (1, 1),
            new (2, 2),
            new (3, 3),
            new (4, 2),
            new (5, 1),
        };

        // Act
        var lines = SegmentedLeastSquares.Calculate(points);

        // Assert
        lines.Should().HaveCount(2);
        lines[0].Slope.Should().BeApproximately(1, 1e-6);
        lines[0].YIntercept.Should().BeApproximately(0, 1e-6);
        lines[1].Slope.Should().BeApproximately(-1, 1e-6);
        lines[1].YIntercept.Should().BeApproximately(6, 1e-6);
    }
}
