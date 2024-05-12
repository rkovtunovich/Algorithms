﻿using Models.Spacing;

public class EuclideanDistance : IDistanceStrategy
{
    public double CalculateDistance(IPoint point1, IPoint point2)
    {
        int dimensions = Math.Min(point1.DimensionCount, point2.DimensionCount);
        double sum = 0;
        for (int i = 0; i < dimensions; i++)
        {
            double diff = point1.GetCoordinate(i) - point2.GetCoordinate(i);
            sum += diff * diff;
        }
        return Math.Sqrt(sum);
    }
}