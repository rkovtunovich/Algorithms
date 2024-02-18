using ScheduleOptimization.Models;
using Sorting.Common;

namespace ScheduleOptimization;

// Real world problem example:
// Some security consultants working in the financial domain are currently advising a client who is investigating a potential money-laundering scheme. 
// The investigation thus far has indicated that n suspicious transactions took place in recent days, each involving money transferred into a single account. 
// Unfortunately, the sketchy nature of the evidence to date means that they don't know the identity of the account, the amounts of the transactions, or the exact times at which the transactions took place. 
// What they do have is an approximate time-stamp for each transaction; the evidence indicates that transaction i took place at time ti+-ei for some "margin of error" ei
// (In other words, it took place sometime between ti-ei and ti+ei.) 
// Note that different transactions may have different margins of error
// In the last day or so, they've come across a bank account that (for other reasons we don't need to go into here) they suspect might be the one involved in the crime. 
// There are n recent events involving the account, which took place at times x1, x2, ..., xn
// To see whether it's plausible that this really is the account they're looking for, 
// they're wondering whether it's possible to associate each of the account's n events with a distinct one of the n suspicious transactions in such a way that, 
// if the account event at time xi is associated with the suspicious transaction that occurred approximately at time tj then |tj-xi|<=ej 
//  (In other words, they want to know if the activity on the account lines up with the suspicious transactions to within the margin of error; 
//  the tricky part here is that they don't know which account event to associate with which suspicious transaction.
// Give an efficient algorithm that takes the given data and decides whether such an association exists. If possible, you should make the running time be at most O(n^2)

// Solution:
// Sort the events in ascending order.
// Sort the intervals first by their length (margin of error) and then by start time. This is to ensure that the intervals with the smallest margin of error are considered first.
// Iterate through each event and try to match it with an interval.
// If no matching interval is found, return false.
// If all events are matched, return true.

// Time complexity: The complexity of your algorithm is mainly influenced by the sorting operations and the nested loop for matching events with intervals.
// The sorting operations are O(nlogn), and the nested loop is O(n^2) in the worst case, making the overall complexity O(n^2), which meets the requirement.

public class SequenceEventsMatchingIntervals
{
    // Method to try to match a list of events with a list of intervals, considering margins of error.
    public static (bool isMatched, Dictionary<int, Interval?> marching) TryMatch(List<int> events, List<Interval> intervals)
    {
        var matching = new Dictionary<int, Interval?>(); // Dictionary to store the matching of events with intervals.

        QuickSort.Sort(ref events); // Sort the events in ascending order.

        // Sort intervals first by their length (margin of error) and then by start time.
        var sortedIntervals = intervals.OrderBy(x => (x.Duration)).ThenBy(x => x.Start).ToList<Interval?>();

        var isMatched = true; // Flag to indicate if all events are successfully matched.

        // Iterate through each event to find a matching interval.
        for (int i = 0; i < events.Count; i++)
        {
            var currentEvent = events[i]; // Get the current event.
                                          // Try to match the current event with one of the intervals.
            var interval = TryMatchEvent(currentEvent, sortedIntervals);

            // If no matching interval is found, set isMatched to false.
            if (interval is null)
                isMatched = false;

            // Add the event and its matching interval to the dictionary.
            matching.Add(currentEvent, interval);
        }

        return (isMatched, matching); // Return the result (whether all events are matched and the matching details).
    }

    // Helper method to find a matching interval for a given event.
    private static Interval? TryMatchEvent(int currentEvent, List<Interval?> intervals)
    {
        // Iterate through each interval to find a match.
        for (int i = 0; i < intervals.Count; i++)
        {
            var interval = intervals[i]; // Get the current interval.

            // Skip if the interval is already used (null).
            if (interval is null)
                continue;

            // Check if the event falls within the interval's range.
            if (interval.Start <= currentEvent && interval.End >= currentEvent)
            {
                intervals[i] = null; // Mark the interval as used.
                return interval; // Return the matching interval.
            }
        }

        return null; // Return null if no matching interval is found.
    }
}

    
