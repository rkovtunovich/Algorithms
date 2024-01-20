using ScheduleOptimization.Models;

namespace ScheduleOptimization;

// Real world applications example:
// The manager of a large student union on campus comes to you with the following problem. 
// She's in charge of a group of n students, each of whom is scheduled to work one shift during the week. 
// There are different jobs associated with these shifts (tending the main desk, helping with package delivery, rebooting cranky information kiosks, etc.), but we can view each shift as a single contiguous interval of time. 
// There can be multiple shifts going on at once.
// She's trying to choose a subset of these n students to form a super vising committee that she can meet with once a week. 
// She considers such a committee to be complete if, for every student not on the committee, that student's shift overlaps (at least partially) the shift of some student who is on the committee. 
// In this way, each student's performance can be observed by at least one person who's serving on the committee.
// Give an efficient algorithm that takes the schedule of n shifts and produces a complete supervising committee containing as few students as possible.
// Example. 
// Suppose n=3 and the shifts are
// Monday 4pm - Monday 8pm,
// Monday 6pm - Monday 10pm 
// Monday 9pm - Monday 11pm
// Then the smallest complete supervising committee would consist of just the second student, since the second shift overlaps both the first and the third.

// The algorithm is a greedy one, where you select the next job that finishes first, provided it doesn't conflict with the already selected jobs.
// The algorithm works by selecting shifts that provide maximum coverage over other shifts, thus minimizing the total number of shifts required.

// The key steps in your algorithm are: 
// 1. Sort Intervals by End Time in Descending Order: This sorting prioritizes intervals that finish later, as they are more likely to overlap with other intervals.
// 2. Iteratively Select Intervals for the Committee: The algorithm examines each interval in the sorted list and selects it for the committee if it provides new coverage—that is, if it overlaps with intervals not already covered by previously selected committee members.
// 3. Maximize Coverage with Each Selection: By selecting intervals that cover the most remaining intervals, the algorithm ensures that each addition to the committee contributes significantly to the overall coverage.
// 4. Result: The final set of intervals (committee members) ensures that every shift is either directly part of the committee or overlaps with at least one member's shift.

// The algorithm runs in O(nlogn) time complexity, mainly due to the sorting step, where n is the number of intervals.

public static class MinimizingIntervalIntersection
{
    public static List<Interval> GetMinimumIntervalIntersection(List<Interval> intervals)
    {
        // Sort the intervals by their end times in descending order.
        var sortedByEnd = intervals.OrderByDescending(i => i.End).ToList();

        // Initialize an empty list to store the optimal set of intervals for the committee.
        var optimalSet = new List<Interval>();

        // Handle edge cases: if there are 0 or 1 intervals, no further processing is needed.
        if (sortedByEnd.Count is 0 or 1)
            return optimalSet;

        // Start with the first interval in the sorted list as the initial candidate for the committee.
        var candidate = sortedByEnd[0];
        var limit = candidate.Start;

        // Iterate through the rest of the intervals.
        for (int i = 1; i < intervals.Count; i++)
        {
            var current = sortedByEnd[i];

            // Check if the current interval overlaps with the candidate interval.
            if (current.End >= candidate.Start )
            {
                // If the current interval starts earlier and still overlaps, update the candidate.
                if (current.Start < candidate.Start && current.End >= limit)             
                    candidate = current;                               
            }
            else
            {
                // If there is no overlap, add the candidate to the optimal set and update the new candidate.
                optimalSet.Add(candidate);
                candidate = current;
                limit = candidate.Start;
            }
        }

        // Add the final candidate to the optimal set.
        optimalSet.Add(candidate);

        return optimalSet;
    }
}
