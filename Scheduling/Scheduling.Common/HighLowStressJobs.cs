namespace Scheduling.Common;


public class HighLowStressJobs
{
    /// <summary>
    /// Finds the maximum revenue obtainable from low-stress and high-stress jobs over n weeks.
    /// </summary>
    /// <param name="lowStress">An array representing the revenue from low-stress jobs for each week.</param>
    /// <param name="highStress">An array representing the revenue from high-stress jobs for each week.</param>
    /// <returns>The maximum obtainable revenue.</returns>
    public static int FindMaxValue(int[] lowStress, int[] highStress)
    {
        int n = lowStress.Length;
        if (n == 0) return 0; // No weeks, no jobs, no revenue

        // DP array to store the maximum revenue up to each week
        var dp = new int[n + 1];

        // Base case: Week 1
        dp[1] = Math.Max(lowStress[0], highStress[0]); // Can choose either low-stress or high-stress in week 1

        // Iterate through weeks 2 to n
        for (int i = 2; i <= n; i++)
        {
            // Option 1: Take the low-stress job this week, which has no restrictions from the previous week
            int takeLowStress = lowStress[i - 1] + dp[i - 1];

            // Option 2: Take the high-stress job this week, so the previous week must be "none" for preparation
            int takeHighStress = highStress[i - 1] + dp[i - 2];

            // Choose the best option
            dp[i] = Math.Max(takeLowStress, takeHighStress);
        }

        // Return the maximum revenue obtainable at the last week
        return dp[n];
    }
}

