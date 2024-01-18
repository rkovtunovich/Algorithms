using ScheduleOptimization.Models;

namespace ScheduleOptimization.Helpers;

public static class IntervalGenerator
{
    public static List<Interval> GetIntervals(int count, int min, int max)
    {
        var intervals = new List<Interval>();
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var start = random.Next(min, max);
            var end = random.Next(start, max);
            intervals.Add(new Interval(start, end));
        }

        return intervals;
    }
}
