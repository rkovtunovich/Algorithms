namespace ResourceOptimization.Allocation;

/// <summary>
/// Given a list of non-negative integer weights, finds whether one can pick
/// exactly `groupSize` of them so that the sum of the chosen weights lies
/// between [threshold .. totalSum − threshold].  If so, it returns one such
/// subset of indices.
///
/// This solves the general “subset-sum with cardinality constraint” problem,
/// and can be used for:
///  - Gerrymandering checks (each precinct is a weight, threshold=majority).
///  - Workload balancing (split tasks evenly but each half must carry at least X work).
///  - Resource allocation where you must pick exactly k resources but also hit a minimum capacity.
///
/// Running time: O(n · groupSize · totalSum) in the worst case (polynomial
/// in n and in the magnitude of the weights).  Space is the same order.
/// </summary>
public static class BalancedPartition
{
    /// <summary>
    /// Attempts to pick exactly groupSize items out of weights[] so that
    /// threshold ≤ sum(picked) ≤ totalSum − threshold.
    /// </summary>
    /// <param name="weights">Array of non-negative integer weights (length n).</param>
    /// <param name="groupSize">Exactly how many items must be chosen (k ≤ n).</param>
    /// <param name="threshold">
    /// Lower bound on the chosen‐sum; also enforces the other group’s sum ≥ threshold.
    /// </param>
    /// <returns>
    /// (possible, indices):
    ///  • possible = true if such a selection exists.
    ///  • indices = one zero-based list of exactly groupSize positions
    ///    whose weights satisfy the threshold constraints.
    /// </returns>
    public static (bool possible, List<int>? indices) TrySplit(
        int[] weights,
        int groupSize,
        int threshold
    )
    {
        int n = weights.Length;
        if (groupSize < 0 || groupSize > n)
            throw new ArgumentException("groupSize must be between 0 and n");

        int total = weights.Sum();

        // If we must take all n items, we only check that their sum ≥ threshold.
        // Otherwise, we need both sides ≥ threshold, which requires total ≥ 2*threshold.
        if (groupSize == n)
        {
            if (total < threshold)
                return (false, null);
            // In that case there is exactly one way: pick everything.
            return (true, Enumerable.Range(0, n).ToList());
        }
        else
        {
            // Now groupSize < n. We require both groupSum ≥ threshold
            // AND (total - groupSum) ≥ threshold ⇒ total ≥ 2*threshold.
            if (total < 2 * threshold)
                return (false, null);
        }

        // dp[j,s] = can we pick j items summing exactly to s?
        bool[,] dp = new bool[groupSize + 1, total + 1];
        // prev[j,s] = index of last item used to reach dp[j,s], or -1 if none.
        int[,] prev = new int[groupSize + 1, total + 1];

        dp[0, 0] = true;
        for (int j = 0; j <= groupSize; j++)
            for (int s = 0; s <= total; s++)
                prev[j, s] = -1;

        // Fill DP by iterating items
        for (int i = 0; i < n; i++)
        {
            int w = weights[i];
            // go backwards on j to avoid reuse in same iteration
            for (int j = groupSize; j >= 1; j--)
            {
                for (int s = 0; s + w <= total; s++)
                {
                    if (dp[j - 1, s] && !dp[j, s + w])
                    {
                        dp[j, s + w] = true;
                        prev[j, s + w] = i;
                    }
                }
            }
        }

        // Look for any sum s in [threshold .. total-threshold]
        for (int s = threshold; s <= total - threshold; s++)
        {
            if (dp[groupSize, s])
            {
                // Backtrack one valid solution
                var indices = new List<int>(groupSize);
                int jj = groupSize, ss = s;
                while (jj > 0)
                {
                    int item = prev[jj, ss];
                    indices.Add(item);
                    ss -= weights[item];
                    jj--;
                }
                indices.Reverse();

                return (true, indices);
            }
        }

        return (false, null);
    }
}
