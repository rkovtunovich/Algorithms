using ScheduleOptimization.Models;

namespace ScheduleOptimization;

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
}
