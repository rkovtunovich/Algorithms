using Spacing.Core.Points;

namespace Spacing.Core;

public struct Line
{
    public double Slope { get; }

    public double YIntercept { get; }

    public readonly bool IsVertical => double.IsInfinity(Slope);

    public double? VerticalX { get; }

    public Line(double slope, double yIntercept)
    {
        Slope = slope;
        YIntercept = yIntercept;
    }

    public Line(double verticalX)
    {
        Slope = double.PositiveInfinity;
        YIntercept = double.NaN;
        VerticalX = verticalX;
    }

    public double GetY(double x) => Slope * x + YIntercept;

    public bool AreEquivalent(Line other) => Slope == other.Slope && YIntercept == other.YIntercept;

    public static Line FromPoints(Point2D p1, Point2D p2)
    {
        // handle case where point lies on the same vertical line
        if (p1.X == p2.X)
            return new Line(p1.X);

        var slope = (p2.Y - p1.Y) / (p2.X - p1.X);
        var yIntercept = p1.Y - slope * p1.X;

        return new Line(slope, yIntercept);
    }
}
