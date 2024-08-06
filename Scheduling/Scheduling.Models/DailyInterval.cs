using MathAlgo.Models;

namespace Scheduling.Models;

public record DailyInterval : Interval
{
    public const int HoursInDay = 24;

    public DailyInterval(int start, int end) : base(start, end)
    {
    }

    public override int Duration => End - Start < 0 ? End - Start + HoursInDay : End - Start;
}
