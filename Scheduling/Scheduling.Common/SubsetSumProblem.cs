namespace Scheduling.Common;

public class SubsetSumProblem
{
    // Problem is closely related to the knapsack problem.
    // The goal is to select a subset of jobs that maximizes the total processing time, 
    // while the total processing time does not exceed the given time capacity.
    // This is a combinatorial optimization problem where we want to fill a available time as efficiently as possible.
    // This problem can be solved using dynamic programming. 
    // The time complexity of the solution is O(n * T), where n is the number of jobs and T is the time capacity.
    // The space complexity of the solution is O(n * T).
    // Problem restatement:
    //      Given a set of jobs {J1, J2, ..., Jn} with processing times {P1, P2, ..., Pn} and a time capacity T,
    //      find a subset of jobs that maximizes the total processing time, while the total processing time does not exceed T.
    public static List<Job> MaximizeProcessingTime(List<Job> jobs, int timeCapacity)
    {  
        int n = jobs.Count;
        int[][] dp = new int[n + 1][];

        // Initialize the dp array.
        for (int i = 0; i <= n; i++)        
            dp[i] = new int[timeCapacity + 1];        

        // Fill the dp array.
        for (int i = 1; i <= n; i++)
        {
            for (int t = 0; t <= timeCapacity; t++)
            {
                if (jobs[i - 1].Duration > t)                
                    dp[i][t] = dp[i - 1][t];              
                else              
                    dp[i][t] = Math.Max(dp[i - 1][t], dp[i - 1][t - jobs[i - 1].Duration] + jobs[i - 1].Duration);                
            }
        }

        // Find the selected jobs.
        var timePoints = new List<Job>();
        int processedTime = timeCapacity;
        for (int i = n; i > 0; i--)
        {
            if (dp[i][processedTime] != dp[i - 1][processedTime])
            {
                timePoints.Add(jobs[i - 1]);
                processedTime -= jobs[i - 1].Duration;
            }
        }

        return timePoints;
    }
}
