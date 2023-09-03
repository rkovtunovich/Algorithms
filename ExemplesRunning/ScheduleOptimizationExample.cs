using ScheduleOptimization;

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
}

