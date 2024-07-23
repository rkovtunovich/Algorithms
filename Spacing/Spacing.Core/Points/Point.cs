namespace Spacing.Core.Points;

public readonly struct Point(int x) : IPoint
{
    public int DimensionCount => 1;

    public readonly int X = x;

    public int GetCoordinate(int dimension = 0)
    {
        return X;
    }

    public override string? ToString()
    {
        return $"{X}";
    }

    public int CompareTo(IPoint? other)
    {
        if (other is null)      
            return 1;
        
        if (DimensionCount != other.DimensionCount)        
            throw new ArgumentException("Cannot compare points of different dimensions.");
        
        return GetCoordinate().CompareTo(other.GetCoordinate(0));
    }
}