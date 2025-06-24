using DataStructures.Heaps;

namespace ResourceOptimization.Scheduling;

/// <summary>
/// Implements the Moore–Hodgson algorithm to minimize the number of late jobs
/// on a single machine. Jobs are processed in nondecreasing order of deadline.
/// Whenever the running time exceeds the current job's deadline, the algorithm
/// greedily drops the job with the largest processing time seen so far. A
/// max-heap keyed by job duration allows finding this job in O(log n) time.
/// The overall complexity is therefore O(n log n).
///
/// This approach is useful in settings such as production lines or other
/// single-machine environments where each late job incurs a significant
/// penalty and the goal is to complete as many jobs on time as possible.
/// </summary>
public static class MinimizingNumberOfLateJobs
{
    /// <summary>
    /// Returns a schedule splitting jobs into those that can be completed
    /// on time and those that must be rejected as late.
    /// </summary>
    /// <param name="jobs">List of jobs with processing durations and deadlines.</param>
    /// <returns>Tuple of on-time jobs followed by late jobs.</returns>
    public static (List<Job> OnTimeJobs, List<Job> LateJobs) Schedule(List<Job> jobs)
    {
        if (jobs.Count is 0)
            return ([], []);

        // Step 1: sort jobs by nondecreasing deadline
        var sortedByDeadline = jobs.OrderBy(j => j.Deadline).ToList();

        // Step 2: iterate through jobs maintaining current time
        // and a max-heap keyed by duration to remove the longest job
        var maxHeap = new HeapMax<int, Job>();
        int currentTime = 0;
        var lateJobs = new List<Job>();

        foreach (var job in sortedByDeadline)
        {
            currentTime += job.Duration;
            maxHeap.Insert(job.Duration, job);

            // If deadline violated, drop the longest job so far
            if (currentTime > job.Deadline)
            {
                var removed = maxHeap.ExtractNode();
                currentTime -= removed.Key;
                lateJobs.Add(removed.Value!);
            }
        }

        // Jobs remaining in the heap are exactly those finished on time
        var lateSet = lateJobs.ToHashSet();
        var onTimeJobs = sortedByDeadline.Where(j => !lateSet.Contains(j)).ToList();

        return (onTimeJobs, lateJobs);
    }
}

