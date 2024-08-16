using Spacing.Core;
using Spacing.Core.Points;

namespace MathAlgo.Common;

public class SegmentedLeastSquares
{
    public static List<Line> Calculate(List<Point2D> points, double penalty = 1.0)
    {
        // sort points by x
        points.Sort((a, b) => a.X.CompareTo(b.X));

        var lines = new List<Line>();
        var n = points.Count;
        var dp = new double[n];
        var prev = new int[n];
        dp[0] = 0;
        prev[0] = -1;

        for (var i = 1; i < n; i++)
        {
            dp[i] = double.MaxValue;
            for (var j = 0; j < i; j++)
            {
                var cost = dp[j] + Cost(points, j, i);
                if (cost < dp[i])
                {
                    dp[i] = cost;
                    prev[i] = j;
                }
            }
        }

        for (var i = n - 1; i != -1; i = prev[i])
        {
            var line = FitLine(points, prev[i], i);
            lines.Add(line);
        }

        lines.Reverse();
        return lines;
    }

    private static Line FitLine(List<Point2D> points, int v, int i)
    {
        var n = i - v + 1;
        var sumX = 0.0;
        var sumY = 0.0;
        var sumXY = 0.0;
        var sumX2 = 0.0;
        for (var j = v; j <= i; j++)
        {
            sumX += points[j].X;
            sumY += points[j].Y;
            sumXY += points[j].X * points[j].Y;
            sumX2 += points[j].X * points[j].X;
        }

        var slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        var yIntercept = (sumY - slope * sumX) / n;

        return new Line(slope, yIntercept);
    }

    private static double Cost(List<Point2D> points, int j, int i)
    {
        var line = FitLine(points, j, i);
        var cost = 0.0;
        for (var k = j; k <= i; k++)
        {
            var y = line.GetY(points[k].X);
            cost += Math.Pow(y - points[k].Y, 2);
        }

        return cost;
    }
}