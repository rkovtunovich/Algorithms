namespace Searching.Common;

/// <summary>
/// Provides methods for finding the Longest Increasing Subsequence (LIS)
/// in an integer array, including backtracking to retrieve the actual subsequence.
/// </summary>
public static class LongestIncreasingSubsequence
{
    /// <summary>
    /// Finds the longest strictly increasing subsequence in the given array.
    /// This does NOT require the subsequence to start with the first element.
    /// </summary>
    /// <param name="arr">The input array of integers.</param>
    /// <returns>A list of integers representing the LIS in the order they appear.</returns>
    public static List<int> ComputeLIS(int[] arr)
    {
        int n = arr.Length;
        if (n is 0)
            return [];

        // dp[i] = length of the LIS that ends exactly at index i
        int[] dp = new int[n];
        // pred[i] = predecessor index for arr[i] in the LIS ending at i
        int[] pred = new int[n];

        // Each element is at least a subsequence of length 1, no predecessor initially
        for (int i = 0; i < n; i++)
        {
            dp[i] = 1;
            pred[i] = -1;
        }

        // Build dp array by checking all j < i
        for (int i = 1; i < n; i++)
        {
            for (int j = 0; j < i; j++)
            {
                // If we can extend the subsequence from j to i
                if (arr[j] < arr[i] && dp[j] + 1 > dp[i])
                {
                    dp[i] = dp[j] + 1;
                    pred[i] = j;
                }
            }
        }

        // Find the index i that has the maximum dp[i]
        int lisLength = 0;
        int lisEnd = 0; // store the index where the LIS ends
        for (int i = 0; i < n; i++)
        {
            if (dp[i] > lisLength)
            {
                lisLength = dp[i];
                lisEnd = i;
            }
        }

        // Reconstruct the subsequence by backtracking from lisEnd
        var lisSequence = new List<int>();
        int currentIndex = lisEnd;
        while (currentIndex is not -1)
        {
            lisSequence.Add(arr[currentIndex]);
            currentIndex = pred[currentIndex];
        }

        // The sequence is built backwards, so reverse it
        lisSequence.Reverse();

        return lisSequence;
    }
}