using Spacing.Core.Points;

public class ManhattanDistance : IDistanceStrategy
{
    public double CalculateDistance(IPoint point1, IPoint point2)
    {
        int dimensions = Math.Min(point1.DimensionCount, point2.DimensionCount);
        double sum = 0;
        for (int i = 0; i < dimensions; i++)
        {
            sum += Math.Abs(point1.GetCoordinate(i) - point2.GetCoordinate(i));
        }
        return sum;
    }
}
