namespace Models.Space;

public readonly struct Point: IPoint
{
    public readonly int x;

    public Point(int x)
    {
        this.x = x;
    }

    public int GetX()
    {
        return x;
    }

    public override string? ToString()
    {
        return $"{x}";
    }
}