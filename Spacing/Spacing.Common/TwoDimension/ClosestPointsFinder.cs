using Helpers.Space;
using Spacing.Core.Points;
using Sorting.Common;

namespace Spacing.Common.TwoDimension;

public class ClosestPointsFinder
{
    public static (Point2D, Point2D, double) GetClosestPoints(Point2D[] points, bool showSteps = false)
    {
        if (points.Length < 2)
            throw new ArgumentException("At least two points are required.");

        if (showSteps)
            SpaceHelper.Show2DPoints(points);

        // sort by X coordinate
        MergeSort2DPoints.Sort(ref points, 0);

        if (showSteps)
            SpaceHelper.Show2DPoints(points);

        // copy of points sorted by Y coordinate
        var copy = (Point2D[])points.Clone();
        // sort by Y coordinate
        MergeSort2DPoints.Sort(ref points, 1);

        if (showSteps)
            SpaceHelper.Show2DPoints(points);

        var closestPoints = GetClosestPointsRec(points, copy);

        return closestPoints;
    }

    private static (Point2D, Point2D, double) GetClosestPointsRec(Point2D[] pointsX, Point2D[] pointsY)
    {
        if (pointsX.Length + pointsY.Length < 4)
            return GetClosestPoints(pointsX, pointsY);

        int middle = pointsX.Length / 2;

        Point2D[] pointsXL = new Point2D[middle];
        Point2D[] pointsXR = new Point2D[pointsX.Length - middle];
        Array.Copy(pointsX, 0, pointsXL, 0, pointsXL.Length);
        Array.Copy(pointsX, middle, pointsXR, 0, pointsXR.Length);

        Point2D[] pointsYL = new Point2D[middle];
        Point2D[] pointsYR = new Point2D[pointsY.Length - middle];
        Array.Copy(pointsY, 0, pointsYL, 0, pointsYL.Length);
        Array.Copy(pointsY, middle, pointsYR, 0, pointsYR.Length);

        var closestLeft = GetClosestPointsRec(pointsXL, pointsYL); // the best left pair
        var closestRight = GetClosestPointsRec(pointsXR, pointsYR); // the best right pair

        var currentClosest = ChooseClosest(closestLeft, closestRight);

        var closestSplit = GetClosestPointsSplit(pointsX, pointsY, currentClosest.d);

        currentClosest = ChooseClosest(currentClosest, closestSplit);

        return currentClosest;
    }

    private static double GetDistance2D(Point2D p1, Point2D p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }

    private static (Point2D, Point2D, double) GetClosestPoints(Point2D[] pointsX, Point2D[] pointsY)
    {
        Point2D p1 = new();
        Point2D p2 = new();

        double? minDist = null;

        for (int i = 0; i < pointsX.Length; i++)
        {
            for (int j = 0; j < pointsY.Length; j++)
            {
                if (pointsX[i].Equals(pointsY[j]))
                    continue;

                double currDist = GetDistance2D(pointsX[i], pointsY[j]);

                if (minDist is null || currDist < minDist)
                {
                    minDist = currDist;
                    p1 = pointsX[i];
                    p2 = pointsY[j];
                }
            }
        }

        return (p1, p2, minDist ?? -1);
    }

    private static (Point2D x, Point2D y, double d) GetClosestPointsSplit(Point2D[] pointsX, Point2D[] pointsY, double currentClosestDistance)
    {
        // the X coordinate of the last point in array is median because array is sorted and half divided
        double medianX = pointsX[^1].GetCoordinate(1);
        // the range of X coordinates to check
        double xMin = medianX - currentClosestDistance;
        // the range of X coordinates to check
        double xMax = medianX + currentClosestDistance;

        Point2D p1 = pointsX[0];
        Point2D p2 = pointsY[0];
        double bestDistance = -1;

        int currIndex = 0;
        for (int i = 0; i < pointsY.Length; i++)
        {
            // if the point is in the range
            if (pointsY[i].GetCoordinate(0) > xMin && pointsX[i].GetCoordinate(0) < xMax)
            {
                if (currIndex != i)
                    pointsY[currIndex] = pointsY[i];

                currIndex++;
            }
        }

        for (int i = 0; i < currIndex - 1; i++)
        {
            // check the next 7 points
            for (int j = 0; j < Math.Min(7, currIndex - i); j++)
            {
                if (pointsY[i].Equals(pointsY[i + j]))
                    continue;

                double dist = GetDistance2D(pointsY[i], pointsY[i + j]);
                if (dist < bestDistance || bestDistance is -1)
                {
                    bestDistance = dist;
                    p1 = pointsY[i];
                    p2 = pointsY[i + j];
                }
            }
        }

        return (p1, p2, bestDistance);
    }

    private static (Point2D x, Point2D y, double d) ChooseClosest((Point2D x, Point2D y, double d) distP1, (Point2D x, Point2D y, double d) distP2)
    {
        if (distP1.d is -1)
            return distP2;
        else if (distP2.d is -1)
            return distP1;
        else if (distP1.d < distP2.d)
            return distP1;
        else
            return distP2;
    }
}
