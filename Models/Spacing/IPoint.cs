namespace Models.Spacing;

public interface IPoint
{
    public int DimensionCount { get; }

    public int GetCoordinate(int dimension);
}