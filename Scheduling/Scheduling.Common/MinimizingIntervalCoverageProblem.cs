namespace Scheduling.Common;

// Real world applications example:
// You're working with a group of security consultants who are helping to monitor a large computer system. 
// There's particular interest in keeping track of processes that are labeled "sensitive." 
// Each such process has a designated start time and finish time, and it runs continuously between these times; 
// the consultants have a list of the planned start and finish times of all sensitive processes that will be run that day.
// As a simple first step, they've written a program called status_check that, 
// when invoked, runs for a few seconds and records various pieces of logging information about all the sensitive processes running on the system at that moment. 
// (We'll model each invocation of status_check as lasting for only this single point in time.) 
// What they'd like to do is to run status_check as few times as possible during the day, but enough that for each sensitive process P, 
// status_check is invoked at least once during the execution of process P

public static class MinimizingIntervalCoverageProblem
{

    public static List<int> GetMinimumTimePointsSet(List<Interval> intervals)
    {
        var timePoints = new List<int>();

        // Sort the intervals by their finish times.
        var sortedIntervals = intervals.OrderBy(i => i.End).ToList();

        // Initially, no time point is selected.
        int lastSelectedTimePoint = -1;

        foreach (var interval in sortedIntervals)
        {
            // Select the finish time of the interval if it's not covered by the last selected time point.
            if (interval.Start > lastSelectedTimePoint)
            {
                lastSelectedTimePoint = interval.End;
                timePoints.Add(lastSelectedTimePoint);
            }
        }

        return timePoints;
    }
}
