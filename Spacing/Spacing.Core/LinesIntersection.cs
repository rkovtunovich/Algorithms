namespace Spacing.Core;

public struct LinesIntersection
{
    public Line Line1 { get; set; }

    public Line Line2 { get; set; }

    public double X { get; }

    public double Y { get; }

    public LinesIntersection(Line line1, Line line2)
    {
        Line1 = line1;
        Line2 = line2;

        X = (line2.YIntercept - line1.YIntercept) / (line1.Slope - line2.Slope);
        Y = line1.Slope * X + line1.YIntercept;
    }
}
