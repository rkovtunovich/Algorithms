using Models.Spacing;

public interface IDistanceStrategy
{
    double CalculateDistance(IPoint point1, IPoint point2);
}
