using Spacing.Core.Points;

public interface IDistanceStrategy
{
    double CalculateDistance(IPoint point1, IPoint point2);
}
