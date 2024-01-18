using ScheduleOptimization;
using ScheduleOptimization.Helpers;

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
        var intervals = IntervalGenerator.GetIntervals(10, 1, 10);

        var result = MinimizingIntervalCoverageProblem.GetMinimumTimePointsSet(intervals);

        Console.WriteLine($"Minimum time points set: [{string.Join(",", result.ToArray())}]");
    }
}

