namespace ResourceOptimization.Intervals;

public static class ResourceNumberMinimizationProblem
{
    // Interval Coloring is a version of the Interval Scheduling problem in which all intervals must be
    // scheduled while minimizing the number of resources used. In other words, the goal is to partition the set of
    // intervals into subsets such that each subset contains only compatible intervals and the number of subsets
    // is as small as possible. This problem has many applications, for example, to find the minimum number of
    // classrooms to schedule all lectures or exams (given as intervals) so that no two classes occur at the same
    // time in the same room. Other applications are in multi-processor systems, service windows etc.
    //
    // 1. Initialize Variables: 
    //      schedule is a dictionary that will hold the resource number as the key and a list of intervals assigned to that resource as the value.
    //      sortedIntervals is the list of intervals sorted by start times.
    // 2. Sort Intervals: Sort the intervals by their start times in ascending order. This helps in efficiently checking for overlaps. 
    // 3. Schedule Intervals: Loop through each interval and try to fit it into an existing resource (if possible). If not, create a new resource. 
    // 4. Return the Result: Return the final schedule that minimizes the number of resources.
    // 
    // Time Complexity Analysis
    // Sorting the intervals takes O(nlogn) time.
    // Looping through the sorted intervals and the existing resources in the worst case takes O(n×m),
    // where  m is the number of resources. However, m is generally much smaller than n.
    // So, the overall time complexity is O(nlogn) for sorting and O(n) for scheduling, making it O(nlogn).

    public static Dictionary<int, List<Interval>> GetSchedule(List<Interval> intervals)
    {
        // Initialize a dictionary to hold the schedule. The key is the resource number, and the value is the list of intervals assigned to that resource.
        var schedule = new Dictionary<int, List<Interval>>();

        // Sort the intervals by their start times in ascending order.
        var sortedIntervals = intervals.OrderBy(i => i.Start).ToList();

        // Initialize a variable to keep track of the number of resources used.
        var resourceNumber = 1;

        // Loop through each interval in the sorted list.
        foreach (var interval in sortedIntervals)
        {
            // Flag to check if the current interval is scheduled.
            var isScheduled = false;

            // Loop through each resource and its scheduled intervals.
            foreach (var (key, value) in schedule)
            {
                // Check if the current interval can be added to the existing resource (i.e., it does not overlap with the last interval in the resource).
                if (value[^1].End <= interval.Start)
                {
                    // Add the interval to the existing resource and update the flag.
                    schedule[key].Add(interval);
                    isScheduled = true;
                    break;
                }
            }

            // If the interval could not be scheduled in any existing resource, create a new resource.
            if (!isScheduled)
            {
                schedule.Add(resourceNumber, new List<Interval> { interval });
                resourceNumber++;
            }
        }

        // Return the final schedule.
        return schedule;
    }
}
