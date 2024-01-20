namespace ScheduleOptimization.Models;

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
}
