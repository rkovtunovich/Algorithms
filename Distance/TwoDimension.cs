using Helpers.Space;
using Models.Space;
using Sorting.Common;

namespace Distance;

public class TwoDimension
{
    public static (Point2D, Point2D, double) GetClosestPoints(Point2D[] points)
    {
        SpaceHelper.Show2DPoints(points);
        MergeSort2DPoints.Sort(ref points, new XOrdinateComparer<Point2D>());
        SpaceHelper.Show2DPoints(points);

        var copy = (Point2D[])points.Clone();
        MergeSort2DPoints.Sort(ref points, new YOrdinateComparer<Point2D>());
        SpaceHelper.Show2DPoints(points);

        var closestPoints = GetClosestPointsRec(points, copy);

        return closestPoints;
    }

    // O(n log n)
    private static (Point2D, Point2D, double) GetClosestPointsRec(Point2D[] pointsX, Point2D[] pointsY)
    {
        if (pointsX.Length + pointsY.Length < 4)
        {
            return GetClosestPoints(pointsX, pointsY);
        }


        int midlle = (pointsX.Length) / 2;

        Point2D[] pointsXL = new Point2D[midlle];
        Point2D[] pointsXR = new Point2D[pointsX.Length - midlle];
        Array.Copy(pointsX, 0, pointsXL, 0, pointsXL.Length);
        Array.Copy(pointsX, midlle, pointsXR, 0, pointsXR.Length);

        Point2D[] pointsYL = new Point2D[midlle];
        Point2D[] pointsYR = new Point2D[pointsY.Length - midlle];
        Array.Copy(pointsY, 0, pointsYL, 0, pointsYL.Length);
        Array.Copy(pointsY, midlle, pointsYR, 0, pointsYR.Length);

        var closestLeft = GetClosestPointsRec(pointsXL, pointsYL); // the best left pair
        var closestRight = GetClosestPointsRec(pointsXR, pointsYR); // the best right pair

        var currentClosest = ChooseClosest(closestLeft, closestRight);

        var closestSplit = GetClosestPointsSplit(pointsX, pointsY, currentClosest.Item3);

        currentClosest = ChooseClosest(currentClosest, closestSplit);

        return currentClosest;
    }

    private static double GetDistance2D(Point2D p1, Point2D p2)
    {
        return Math.Sqrt(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2));
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

    private static (Point2D, Point2D, double) GetClosestPointsSplit(Point2D[] pointsX, Point2D[] pointsY, double currentClosestDistance)
    {
        int medianX = pointsX[pointsX.Length - 1].GetX(); // the x coordinate of the last point in array is median because array is sorted and half divided
        double xMin = medianX - currentClosestDistance;
        double xMax = medianX + currentClosestDistance;

        Point2D p1 = pointsX[0];
        Point2D p2 = pointsY[0];
        double bestDistance = -1;

        int currIndex = 0;
        for (int i = 0; i < pointsY.Length; i++)
        {
            if (pointsY[i].GetX() > xMin && pointsX[i].GetX() < xMax)
            {
                if (currIndex != i)
                {
                    pointsY[currIndex] = pointsY[i];
                }
                currIndex++;
            }
        }

        for (int i = 0; i < currIndex - 1; i++)
        {
            for (int j = 0; j < Math.Min(7, currIndex - i); j++)
            {
                if (pointsY[i].Equals(pointsY[i + j]))
                    continue;

                double dist = GetDistance2D(pointsY[i], pointsY[i + j]);
                if (dist < bestDistance || bestDistance == -1)
                {
                    bestDistance = dist;
                    p1 = pointsY[i];
                    p2 = pointsY[i + j];
                }
            }
        }

        return (p1, p2, bestDistance);
    }

    private static (Point2D, Point2D, double) ChooseClosest((Point2D, Point2D, double) distP1, (Point2D, Point2D, double) distP2) 
    {
        if (distP1.Item3 == -1) return distP2;
        else if (distP2.Item3 == -1) return distP1;
        else if (distP1.Item3 < distP2.Item3) return distP1;
        else return distP2;
    }
}
