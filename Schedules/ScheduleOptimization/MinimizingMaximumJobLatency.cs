namespace ScheduleOptimization;

// The algorithm aims to solve a scheduling problem focused on minimizing the maximum job latency.
// Each job has a deadline and a duration.
// The algorithm schedules the jobs in such a way that the maximum latency (the time a job finishes past its deadline) is minimized.
// The algorithm uses a greedy approach, scheduling jobs in order of their deadlines.

// Explanation
//
// 1. Initialize Variables:
//      intervals will hold the time intervals for each scheduled job.
//      sortedJobs contains the jobs sorted by their deadlines.
//      maxLatency keeps track of the maximum latency across all jobs.
//      currentTime is the time at which the next job will start.
// 2. Sort Jobs: The jobs are sorted by their deadlines to facilitate the greedy choice criterion.
// 3. Schedule Jobs: Loop through each sorted job and: 
// 4. Create a new interval for the job based on its duration and the currentTime.
//      Add this interval to the intervals list.
//      Update currentTime to reflect the time at which the next job can start.
//      Calculate the latency for the current job and update maxLatency if needed.
// 5. Return the Result: The function returns a tuple containing the maximum latency and the intervals for all the scheduled jobs.
// 
// Time Complexity Analysis
// Sorting the jobs takes O(nlogn) time.
// Looping through the sorted jobs takes O(n) time.
// The overall time complexity is O(nlogn)+O(n)=O(nlogn), where n is the number of jobs.

public static class MinimizingMaximumJobLatency
{
    // Define a method that takes a list of jobs and returns a tuple containing the maximum latency and the schedule.
    public static (int maxLatency, List<Interval> scheduling) GetSchedule(List<Job> jobs)
    {
        // Initialize a list to store the intervals (start and end times) of the scheduled jobs.
        var intervals = new List<Interval>();

        // Sort the jobs by their deadlines in ascending order.
        var sortedJobs = jobs.OrderBy(job => job.Deadline).ToList();

        // Initialize a variable to keep track of the maximum latency.
        var maxLatency = 0;

        // Initialize a variable to keep track of the current time.
        var currentTime = 0;

        // Loop through each job in the sorted list.
        foreach (var job in sortedJobs)
        {
            // Create an interval for the current job based on its duration and the current time.
            var interval = new Interval(currentTime, currentTime + job.Duration);

            // Add the interval to the list.
            intervals.Add(interval);

            // Update the current time.
            currentTime += job.Duration;

            // Calculate the latency for the current job.
            var latency = interval.End - job.Deadline;

            // Update the maximum latency if the current job's latency is greater.
            if (latency > maxLatency)
                maxLatency = latency;
        }

        // Return the maximum latency and the schedule.
        return (maxLatency, intervals);
    }
}
