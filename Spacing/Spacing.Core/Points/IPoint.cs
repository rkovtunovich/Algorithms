namespace Spacing.Core.Points;

public interface IPoint
{
    public int DimensionCount { get; }

    public double GetCoordinate(int dimension);
}