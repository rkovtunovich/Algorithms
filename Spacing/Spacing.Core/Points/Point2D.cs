namespace Spacing.Core.Points;

public readonly struct Point2D : IPoint
{
    public readonly int x;
    public readonly int y;

    public int DimensionCount => 2;

    public Point2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Get the coordinate of the point in the given dimension.
    /// 0 for x, 1 for y.
    /// </summary>
    /// <param name="dimension"></param>
    /// <returns></returns>
    public int GetCoordinate(int dimension)
    {
        return dimension switch
        {
            0 => x,
            1 => y,
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };
    }

    public override string? ToString()
    {
        return $"{x}:{y}";
    }
}