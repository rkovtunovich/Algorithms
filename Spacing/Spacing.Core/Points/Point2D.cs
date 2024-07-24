namespace Spacing.Core.Points;

public readonly struct Point2D(double x, double y) : IPoint
{
    public readonly double X = x;
    public readonly double Y = y;

    public int DimensionCount => 2;

    /// <summary>
    /// Get the coordinate of the point in the given dimension.
    /// 0 for X, 1 for Y.
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public double GetCoordinate(int dimension)
    {
        return dimension switch
        {
            0 => X,
            1 => Y,
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };
    }

    public override string? ToString()
    {
        return $"{X}:{Y}";
    }

    public int CompareByX(IPoint? other)
    {
        if (other is null)
            return 1;

        if (DimensionCount != other.DimensionCount)
            throw new ArgumentException("Cannot compare points of different dimensions.");

        return GetCoordinate(0).CompareTo(other.GetCoordinate(0));
    }

    public int CompareByY(IPoint? other)
    {
        if (other is null)
            return 1;

        if (DimensionCount != other.DimensionCount)
            throw new ArgumentException("Cannot compare points of different dimensions.");

        return GetCoordinate(1).CompareTo(other.GetCoordinate(1));
    }

    public int CompareByDimension(int dimension, IPoint? other)
    {
        if (other is null)
            return 1;

        if (DimensionCount != other.DimensionCount)
            throw new ArgumentException("Cannot compare points of different dimensions.");

        return GetCoordinate(dimension).CompareTo(other.GetCoordinate(dimension));
    }
}