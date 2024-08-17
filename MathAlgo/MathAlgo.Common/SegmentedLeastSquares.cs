using Spacing.Core;
using Spacing.Core.Points;

namespace MathAlgo.Common;

// Segmented Least Squares (SLS) is an algorithm that deals with the problem of fitting multiple line segment to a set of data points.
// The goal is to minimize the total error while also balancing the number of segment used.
// This approach is particularly useful when the data can be naturally divided into different segment,
// where each segment can be approximated well by a linear function.
// Problem Definition
// Given a set of data points (𝑥1,𝑦1), (𝑥2,𝑦2), …, (𝑥𝑛,𝑦𝑛) where 𝑥1 < 𝑥2 < … < 𝑥𝑛
// the goal is to partition these points into segment and fit a linear function to each segment such that the sum of the squared errors
// plus a penalty for the number of segment used is minimized.

public class SegmentedLeastSquares
{
    public static List<Line> Calculate(List<Point2D> points, double penalty = 1.0)
    {
        // sort points by x, ensure that the points are sorted in correct order
        points.Sort((a, b) => a.X.CompareTo(b.X));

        // initialize variables
        var n = points.Count;

        // costs[i] is the minimum cost of fitting the first i points
        var costs = new double[n + 1];

        // segment[i] is the index of the last point in the segment that ends at point i
        // helps track the optimal solution and reconstruct the segments
        var segment = new int[n + 1];

        // errors[i, j] is the least squares error for the segment from point i to j
        var errors = new double[n + 1, n + 1];

        // Step 1: Compute the least squares error for each segment

        // Precompute cumulative sums for sumX, sumY, sumXY, and sumX2 as optimization
        var sumX = new double[n + 1];
        var sumY = new double[n + 1];
        var sumXY = new double[n + 1];
        var sumX2 = new double[n + 1];

        for (int k = 1; k <= n; k++)
        {
            sumX[k] = sumX[k - 1] + points[k - 1].X;
            sumY[k] = sumY[k - 1] + points[k - 1].Y;
            sumXY[k] = sumXY[k - 1] + points[k - 1].X * points[k - 1].Y;
            sumX2[k] = sumX2[k - 1] + points[k - 1].X * points[k - 1].X;
        }

        // Compute the least squares error for each segment using precomputed cumulative sums
        for (int i = 1; i <= n; i++)
            for (int j = 1; j <= n; j++)
                errors[i, j] = LeastSquaresError(points, i, j, sumX, sumY, sumXY, sumX2);

        // Step 2: Use dynamic programming to find the minimum cost
        costs[0] = 0;
        for (int j = 1; j <= n; j++)
        {
            costs[j] = double.MaxValue;
            for (int i = 1; i <= j; i++)
            {
                // cost =  cost of fitting the first i points + error of segment i to j + penalty
                var cost = costs[i - 1] + errors[i, j] + penalty;
                if (cost < costs[j])
                {
                    costs[j] = cost;
                    segment[j] = i;
                }
            }
        }

        // Step 3: Reconstruct the optimal solution
        var lines = new List<Line>();
        var current = n;
        while (current > 0)
        {
            var i = segment[current];

            var pointA = points[i - 1];
            var pointB = points[current - 1];
            if (pointA != pointB)
                lines.Add(Line.FromPoints(pointA, pointB));

            current = i - 1;
        }

        lines.Reverse();

        return lines;
    }

    // Calculate the least squares error for a segment of points from i to j
    // using precomputed cumulative sums
    private static double LeastSquaresError(List<Point2D> points, int i, int j, double[] sumX, double[] sumY, double[] sumXY, double[] sumX2)
    {
        var n = j - i + 1;
        var sumXi = sumX[j] - sumX[i - 1];
        var sumYi = sumY[j] - sumY[i - 1];
        var sumXYi = sumXY[j] - sumXY[i - 1];
        var sumX2i = sumX2[j] - sumX2[i - 1];

        var error = 0.0;

        var denominator = n * sumX2i - sumXi * sumXi;

        // Check if the points form a vertical line (undefined slope)
        if (denominator is 0)
        {
            // Points are vertical, no error if they all lie on the same vertical line
            return 0;
        }

        var slope = (n * sumXYi - sumXi * sumYi) / denominator;
        var yIntercept = (sumYi - slope * sumXi) / n;

        for (int k = i; k <= j; k++)
        {
            var y = slope * points[k - 1].X + yIntercept;
            error += Math.Pow(points[k - 1].Y - y, 2);
        }

        return error;
    }
}