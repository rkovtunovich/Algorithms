namespace Spacing.Core;

public struct Line
{
    public double Slope { get; }

    public double YIntercept { get; }

    public Line(double slope, double yIntercept)
    {
        Slope = slope;
        YIntercept = yIntercept;
    }

    public double GetY(double x) => Slope * x + YIntercept;

    public bool AreEquivalent(Line other) => Slope == other.Slope && YIntercept == other.YIntercept;
}
