using ScheduleOptimization;
using ScheduleOptimization.Helpers;
using ScheduleOptimization.Models;

namespace ExamplesRunning;

internal static class ScheduleOptimizationExample
{
    internal static void RunLongestProcessingTimeFirst()
    {
        var jobs = Helpers.ArrayHelper.GetUnsortedArray(100, 1, 10).ToList();
        var result = LongestProcessingTimeFirst.GetMachineLoading(10, jobs);

        foreach (var machine in result)
        {
            Console.WriteLine($"M:{machine.Key}, length = {machine.Value.Sum()} [{string.Join(",", machine.Value.ToArray())}]");
        }
    }

    internal static void RunMinimizingIntervalCoverageProblem()
    {
        //var intervals = IntervalGenerator.GetIntervals(10, 1, 10);
        var intervals = new List<Interval>
        {
            new(16, 20),
            new(18, 22),
            new(21, 23),
        };

        var result = MinimizingIntervalCoverageProblem.GetMinimumTimePointsSet(intervals);

        Console.WriteLine($"Minimum time points set: [{string.Join(",", result.ToArray())}]");
    }

    internal static void RunMinimizingIntervalIntersection()
    {
        //var intervals = new List<Interval>
        //{
        //    new(16, 20),
        //    new(18, 22),
        //    new(21, 23),
        //};

        var intervals = IntervalGenerator.GetIntervals(10, 1, 25);
        var result = MinimizingIntervalIntersection.GetMinimumIntervalIntersection(intervals);

        Console.WriteLine($"Intervals: [{string.Join<Interval>(",", intervals.ToArray())}]");
        Console.WriteLine($"Minimum interval intersection: [{string.Join<Interval>(",", result.ToArray())}]");
    }
}

