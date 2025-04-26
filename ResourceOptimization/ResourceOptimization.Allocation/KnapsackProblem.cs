namespace ResourceOptimization.Allocation;

public static class KnapsackProblem
{
    public static int[,] Choose(IReadOnlyList<int> values,
                             IReadOnlyList<int> sizes,
                             int capacity)
    {
        int n = values.Count;
        var dp = new int[n + 1, capacity + 1];

        // NOTE: i must go through *all* n items (≤ n, not < n)
        for (int i = 1; i <= n; i++)
        {
            int w = sizes[i - 1];
            int v = values[i - 1];

            for (int c = 0; c <= capacity; c++)
            {
                if (w > c)
                    dp[i, c] = dp[i - 1, c];                       // cannot take
                else
                    dp[i, c] = Math.Max(dp[i - 1, c],              // skip
                                        dp[i - 1, c - w] + v);     // take
            }
        }
        return dp;      // dp[n, capacity] is the best profit
    }

    /// <summary>
    /// Exact multiple-knapsack for small m (2 or 3).
    /// Capacity C is identical for each bag.
    /// Returns the maximum achievable profit.
    /// </summary>
    public static int MultiKnapsack(IReadOnlyList<int> values,
                                    IReadOnlyList<int> sizes,
                                    int capacity,
                                    int m)                      // 2 or 3 recommended
    {
        if (m < 1) 
            throw new ArgumentException(nameof(m));

        // ---------- m = 1 falls back to simple DP ----------
        if (m is 1)
            return Choose(values, sizes, capacity)[values.Count, capacity];

        // ---------- m = 2 implementation ----------
        if (m is 2)
        {
            int[,] dp = new int[capacity + 1, capacity + 1];

            for (int idx = 0; idx < values.Count; idx++)
            {
                int w = sizes[idx];
                int p = values[idx];

                // reverse iteration keeps 0/1 semantics
                for (int c1 = capacity; c1 >= 0; c1--)
                    for (int c2 = capacity; c2 >= 0; c2--)
                    {
                        int cur = dp[c1, c2];
                        if (c1 >= w)
                            dp[c1 - w, c2] = Math.Max(dp[c1 - w, c2], cur + p);
                        if (c2 >= w)
                            dp[c1, c2 - w] = Math.Max(dp[c1, c2 - w], cur + p);
                    }
            }

            int best = 0;
            for (int c1 = 0; c1 <= capacity; c1++)
                for (int c2 = 0; c2 <= capacity; c2++)
                    best = Math.Max(best, dp[c1, c2]);

            return best;
        }

        // ---------- m = 3 implementation ----------
        if (m is 3)
        {
            int[,,] dp = new int[capacity + 1, capacity + 1, capacity + 1];

            for (int idx = 0; idx < values.Count; idx++)
            {
                int w = sizes[idx];
                int p = values[idx];

                for (int c1 = capacity; c1 >= 0; c1--)
                    for (int c2 = capacity; c2 >= 0; c2--)
                        for (int c3 = capacity; c3 >= 0; c3--)
                        {
                            int cur = dp[c1, c2, c3];
                            if (c1 >= w)
                                dp[c1 - w, c2, c3] = Math.Max(dp[c1 - w, c2, c3], cur + p);
                            if (c2 >= w)
                                dp[c1, c2 - w, c3] = Math.Max(dp[c1, c2 - w, c3], cur + p);
                            if (c3 >= w)
                                dp[c1, c2, c3 - w] = Math.Max(dp[c1, c2, c3 - w], cur + p);
                        }
            }

            int best = 0;
            for (int c1 = 0; c1 <= capacity; c1++)
                for (int c2 = 0; c2 <= capacity; c2++)
                    for (int c3 = 0; c3 <= capacity; c3++)
                        best = Math.Max(best, dp[c1, c2, c3]);

            return best;
        }

        // ---------- larger m : warn the caller ----------
        throw new NotSupportedException(
            $"Exact DP explodes for m > 3 (O(C^m) states). " +
            $"Use branch-and-bound or heuristics for bigger m.");
    }

    internal record Item(int Weight, int Profit);
}
