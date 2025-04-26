using ResourceOptimization.Models;
using ResourceOptimization.Models.Helpers;

namespace ResourceOptimization.Intervals;

// The primary goal is to find the maximum number of mutually non-overlapping intervals (or tasks) that can be scheduled.
// The algorithm is a greedy one, where you select the next job that finishes first, provided it doesn't conflict with the already selected jobs.
//
// 1. Sort Intervals: The intervals are sorted by their end times in ascending order.
//  This is crucial for the greedy choice criterion, which selects the job that finishes the earliest and doesn't conflict with the already chosen ones.
// 
// 2. Initialize Variables: 
//      optimalSet will contain the selected non-overlapping intervals.
//      lastFinishTime keeps track of the last interval's finish time that was added to the optimal set.
//
// 3. Loop Through Sorted Intervals: For each interval, if its start time is greater than or equal to the last finish time,
//  it means the interval is non-overlapping with the intervals already in the optimalSet. Therefore, add it to the optimalSet and update lastFinishTime.
// 
// 4. Return the Result: The function returns the optimalSet, which is the maximum set of mutually non-overlapping intervals.
// 
// The algorithm runs in O(nlogn) time complexity, mainly due to the sorting step, where n is the number of intervals.

public class IntervalSchedulingMaximizationProblem
{
    // Define a method that takes a list of intervals and returns an optimal set of non-overlapping intervals.
    public static List<Interval> GetOptimalSet(List<Interval> intervals)
    {
        // Sort the intervals by their end times in ascending order.
        var sortedIntervals = intervals.OrderBy(i => i.End).ToList();

        // Initialize a list to hold the optimal set of non-overlapping intervals.
        var optimalSet = new List<Interval>();

        // Initialize the last finish time to 0.
        var lastFinishTime = 0;

        // Loop through each interval in the sorted list.
        foreach (var interval in sortedIntervals)
        {
            // Check if the start time of the current interval is greater or equal to the last finish time.
            if (interval.Start >= lastFinishTime)
            {
                // If it is, then the interval can be added to the optimal set.
                optimalSet.Add(interval);

                // Update the last finish time to the end time of the current interval.
                lastFinishTime = interval.End;
            }
        }

        // Return the optimal set of intervals.
        return optimalSet;
    }

    // Define a method that takes a list of intervals and returns the maximum number of non-overlapping intervals.
    // taking into account daily periods, also intervals can be started one day and finished the next day.
    public static List<DailyInterval> GetOptimalSetForCycledTimeline(List<DailyInterval> intervals)
    {
        var periodLength = 24;
        var cutPoint = GetCutPoint(intervals);

        var sortedIntervals = intervals.OrderBy(i => GetSortingKey(i, periodLength, cutPoint)).ToList();

        IntervalViewer.ShowIntervals(sortedIntervals, periodLength);

        var optimalSet = new List<DailyInterval>();

        var lastFinishTime = 0;
        var start = 0;

        foreach (var interval in sortedIntervals)
        {
            if (interval.Start >= lastFinishTime && interval.End > start && interval.Start < interval.End)
            {
                optimalSet.Add(interval);
                if (optimalSet.Count is 1)
                    start = interval.Start;

                lastFinishTime = interval.End;
            }
            else if (interval.Start >= lastFinishTime && interval.End <= start && start <= lastFinishTime)
            {
                optimalSet.Add(interval);
                if (optimalSet.Count is 1)
                    start = interval.Start;

                lastFinishTime = interval.End;
            }

            if (optimalSet.Count is 0)
            {
                optimalSet.Add(interval);
                start = interval.Start;
                lastFinishTime = interval.End;
            }

        }

        return optimalSet;
    }

    // Find the first interval that does not overlap with any intervals spanning across midnight (intervals for which the end time is less than the start time).
    // The start of this interval marks a point in the timeline where cutting it would not split any single job into two parts.
    // If there is no such interval (i.e., all intervals overlap with intervals spanning midnight),
    // any point that minimizes the split intervals could be considered, though this scenario is less likely given the problem's constraints.
    private static int GetCutPoint(List<DailyInterval> intervals)
    {
        var maxCrossedEnding = 0;

        for (var i = 0; i < intervals.Count; i++)
        {
            if (intervals[i].End < intervals[i].Start)
            {
                maxCrossedEnding = Math.Max(maxCrossedEnding, intervals[i].End);
                break;
            }
        }

        var cutPoint = int.MaxValue;

        for (var i = 0; i < intervals.Count; i++)
        {
            if (intervals[i].Start < maxCrossedEnding)
                continue;

            cutPoint = Math.Min(cutPoint, intervals[i].Start);

        }

        return cutPoint is int.MaxValue ? 0 : cutPoint;
    }

    private static int GetSortingKey(DailyInterval interval, int periodLength, int cutPoint)
    {
        int key;

        if (interval.Start < cutPoint)
            key = periodLength;
        else if (interval.End > interval.Start)
            key = interval.End - cutPoint;
        else
            key = interval.End - cutPoint + periodLength;

        return key;
    }
}
