using MathAlgo.Models;

namespace ResourceOptimization.Models.Helpers;

public static class IntervalGenerator
{
    public static List<T> GetIntervals<T>(int count, int min, int max) where T : Interval, new()
    {
        var intervals = new List<T>();
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var start = random.Next(min, max);
            var end = random.Next(start, max);
            intervals.Add(new T() { Start = start, End = end });
        }

        return intervals;
    }

    public static List<T> GetPeriodicIntervals<T>(int count, int min, int max) where T : Interval, new()
    {
        var intervals = new List<T>();
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var start = random.Next(min, max);
            var end = random.Next(min, max);

            while (start == end)
                end = random.Next(min, max);

            intervals.Add(new T() { Start = start, End = end });
        }

        return intervals;
    }
}
