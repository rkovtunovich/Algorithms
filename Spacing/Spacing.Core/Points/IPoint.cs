namespace Spacing.Core.Points;

public interface IPoint
{
    public int DimensionCount { get; }

    public int GetCoordinate(int dimension);
}