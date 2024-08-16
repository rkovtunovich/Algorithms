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
            new Point2D(0, 0),
            new Point2D(1, 1),
            new Point2D(2, 2),
            new Point2D(3, 3),
            new Point2D(4, 4),
            new Point2D(5, 5),
            new Point2D(6, 6),
            new Point2D(7, 7),
            new Point2D(8, 8),
            new Point2D(9, 9),
        };

        var lines = SegmentedLeastSquares.Calculate(points);

        lines.Should().ContainSingle();
        lines[0].Slope.Should().BeApproximately(1, 1e-6);
        lines[0].YIntercept.Should().BeApproximately(0, 1e-6);
    }
}
