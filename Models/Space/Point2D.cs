namespace Models.Space;

public readonly struct Point2D: IPoint2D
{
    public readonly int x;
    public readonly int y;

    public Point2D(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public override string? ToString()
    {
        return $"{x}:{y}";
    }
}