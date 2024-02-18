namespace ScheduleOptimization.Models;

public record DailyInterval : Interval
{
    private const int HoursInDay = 24;

    public DailyInterval()
    {
    }

    public DailyInterval(int start, int end) : base(start, end)
    {
    }

    public override int Duration => End - Start < 0 ? End - Start + HoursInDay : End - Start;  
}
