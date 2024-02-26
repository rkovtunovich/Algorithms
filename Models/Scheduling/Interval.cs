namespace Models.Scheduling;

public record Interval
{
    public Interval()
    {
    }

    public Interval(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; init; }

    public int End { get; init; }

    public override string ToString()
    {
        return $"[{Start},{End}]";
    }

    public virtual int Duration => End - Start;

    public bool Belongs(int value)
    {
        return value >= Start && value <= End;
    }
}
