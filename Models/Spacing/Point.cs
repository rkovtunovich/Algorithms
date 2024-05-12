namespace Models.Spacing;

public readonly struct Point : IPoint
{
    public int DimensionCount => 1;

    public readonly int x;

    public Point(int x)
    {
        this.x = x;
    }

    public int GetCoordinate(int dimension = 0)
    {
        return x;
    }

    public override string? ToString()
    {
        return $"{x}";
    }
}