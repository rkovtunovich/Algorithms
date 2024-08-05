namespace MathAlgo.Models;

public record Interval
{
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

    public bool Overlaps(Interval interval)
    {
        return Start <= interval.End && interval.Start <= End;
    }

    public bool Before(Interval interval)
    {
        return End < interval.Start;
    }

    public bool After(Interval interval)
    {
        return Start > interval.End;
    }
}
