using ResourceOptimization.Models;

namespace ResourceOptimization.Intervals;

// In general, the Weighted Interval Scheduling Maximization Problem is a variant of the Interval Scheduling Maximization Problem.
// The main difference is that each interval has an associated weight, and the goal is to maximize the total weight of the selected intervals.
// In this case using the greedy approach does not guarantee an optimal solution.
// The dynamic programming approach is used to solve this problem optimally.
public class WeightedIntervalSchedulingMaximizationProblem
{
    public static List<WeightedInterval> GetOptimalSet(List<WeightedInterval> intervals)
    {
        if (intervals.Count is 0)
            return [];

        // Sort the intervals by their end times in ascending order.
        var sortedIntervals = intervals.OrderBy(i => i.End).ToList();

        // Initialize an array to store the maximum weight that can be achieved up to each interval.
        var maxWeights = new int[sortedIntervals.Count];

        // Initialize the first element of the maxWeights array to the weight of the first interval.
        maxWeights[0] = sortedIntervals[0].Weight;

        // Loop through each interval in the sorted list.
        for (var i = 1; i < sortedIntervals.Count; i++)
        {
            // Find the index of the latest non-overlapping interval.
            var previousCompatibleIndex = FindLatestNonOverlapping(sortedIntervals, i);

            // Calculate the weight that can be achieved by including the current interval.
            var weightWithCurrent = sortedIntervals[i].Weight + (previousCompatibleIndex == -1 ? 0 : maxWeights[previousCompatibleIndex]);

            // Calculate the weight that can be achieved by excluding the current interval.
            var weightWithoutCurrent = maxWeights[i - 1];

            // Update the maxWeights array with the maximum weight that can be achieved up to the current interval.
            maxWeights[i] = Math.Max(weightWithCurrent, weightWithoutCurrent);
        }

        // Reconstruct the optimal set of intervals.
        var optimalSet = BacktrackOptimalSet(sortedIntervals, maxWeights);

        return optimalSet;
    }

    private static List<WeightedInterval> BacktrackOptimalSet(List<WeightedInterval> sortedIntervals, int[] maxWeights)
    {
        var optimalSet = new List<WeightedInterval>();

        // Start from the last interval and backtrack to find the optimal set of intervals.
        var i = sortedIntervals.Count - 1;
        
        while (i >= 0)
        {
            var latestNonOverlapping = FindLatestNonOverlapping(sortedIntervals, i);

            // If the current interval is included in the optimal set, add it to the list and jump to the latest non-overlapping interval.
            if (i == 0 || sortedIntervals[i].Weight + (latestNonOverlapping == -1 ? 0 : maxWeights[latestNonOverlapping]) > maxWeights[i - 1])
            {
                optimalSet.Add(sortedIntervals[i]);
                i = latestNonOverlapping;
            }
            else
            {
                i--;
            }
        }

        // Reverse the list to get the intervals in the correct order.
        optimalSet.Reverse();

        return optimalSet;
    }

    // Find the index of the latest non-overlapping interval.
    // This is done using binary search to find the interval with the largest end time that does not overlap with the current interval.
    private static int FindLatestNonOverlapping(List<WeightedInterval> intervals, int index)
    {
        int low = 0;
        int high = index - 1;
        while (low <= high)
        {
            int mid = (low + high) / 2;
            if (intervals[mid].End <= intervals[index].Start)
            {
                if (intervals[mid + 1].End <= intervals[index].Start)
                    low = mid + 1;
                else
                    return mid;
            }
            else
                high = mid - 1;
        }

        return -1;
    }
}