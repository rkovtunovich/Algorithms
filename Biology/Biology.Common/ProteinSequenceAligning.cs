namespace Biology.Common;

// The Needleman-Wunsch algorithm is a dynamic programming algorithm used for sequence alignment.
// It is commonly used in bioinformatics for aligning protein or nucleotide sequences to identify similarities.
// The algorithm aligns two sequences by maximizing the match score while allowing for gaps (insertions/deletions).
// The function returns the aligned sequences along with the total alignment cost.
//
// Time complexity: O(n * m), where n and m are the lengths of the input sequences.
// Space complexity: O(n * m), where n and m are the lengths of the input sequences.
//
// Note: There is also a space-optimized version of the Needleman-Wunsch algorithm that reduces the space complexity from O(n × m) to O(min(n, m)).
// This is achieved by only storing the necessary rows or columns of the dynamic programming table at any given time.
// Furthermore, by combining this approach with a divide and conquer strategy, the algorithm can recursively align subsequences,
// further optimizing memory usage. This version is particularly useful for aligning large sequences where memory efficiency is crucial.
public static class ProteinSequenceAligning<T> where T : notnull
{
    /// <summary>
    /// Aligns two sequences using the Needleman-Wunsch algorithm.
    /// </summary>
    /// <param name="sequence1">The first sequence to align.</param>
    /// <param name="sequence2">The second sequence to align.</param>
    /// <param name="symbolPenalty">The penalty for mismatching symbols (substitution).</param>
    /// <param name="gapPenalty">The penalty for introducing a gap (insertion or deletion).</param>
    /// <param name="gapSymbol">The symbol used to represent gaps in the alignment.</param>
    /// <returns>
    /// A tuple containing the aligned sequences and the total alignment cost.
    /// </returns>
    public static (List<T>? sequence1, List<T>? sequence2, double cost) Align(List<T> sequence1,
                                                                            List<T> sequence2,
                                                                            double symbolPenalty,
                                                                            double gapPenalty,
                                                                            T gapSymbol = default,
                                                                            bool costOnly = false)
    {
        // Initialize the dynamic programming table
        var dp = new double[sequence1.Count + 1][];
        for (int i = 0; i <= sequence1.Count; i++)
        {
            dp[i] = new double[sequence2.Count + 1];
            dp[i][0] = i * gapPenalty; // Fill the first column with gap penalties
        }

        for (int j = 0; j <= sequence2.Count; j++)
        {
            dp[0][j] = j * gapPenalty; // Fill the first row with gap penalties
        }

        // Fill the DP table by considering matches, mismatches, and gaps
        for (int i = 1; i <= sequence1.Count; i++)
        {
            for (int j = 1; j <= sequence2.Count; j++)
            {
                // Calculate the penalty for matching or mismatching the current symbols
                var currentSymbolPenalty = sequence1[i - 1].Equals(sequence2[j - 1]) ? 0 : symbolPenalty;

                // The DP transition: minimum of match/mismatch, gap in sequence1, or gap in sequence2
                dp[i][j] = Math.Min(
                    Math.Min(
                        dp[i - 1][j - 1] + currentSymbolPenalty, // Match/Mismatch
                        dp[i - 1][j] + gapPenalty),             // Gap in sequence2
                    dp[i][j - 1] + gapPenalty);                 // Gap in sequence1
            }
        }

        if (costOnly)
        {
            // Return only the total alignment cost
            return (default, default, dp[sequence1.Count][sequence2.Count]);
        }

        // Reconstruct the optimal alignment from the DP table
        return ReconstructAlignment(sequence1, sequence2, dp, symbolPenalty, gapPenalty, gapSymbol);
    }

    /// <summary>
    /// Reconstructs the aligned sequences by backtracking through the DP table.
    /// </summary>
    /// <param name="sequence1">The first sequence to align.</param>
    /// <param name="sequence2">The second sequence to align.</param>
    /// <param name="dp">The dynamic programming table with alignment scores.</param>
    /// <param name="symbolPenalty">The penalty for mismatching symbols (substitution).</param>
    /// <param name="gapPenalty">The penalty for introducing a gap (insertion or deletion).</param>
    /// <param name="gapSymbol">The symbol used to represent gaps in the alignment.</param>
    /// <returns>
    /// A tuple containing the aligned sequences and the total alignment cost.
    /// </returns>
    private static (List<T>, List<T>, double) ReconstructAlignment(List<T> sequence1, List<T> sequence2, double[][] dp, double symbolPenalty, double gapPenalty, T gapSymbol)
    {
        var i = sequence1.Count;
        var j = sequence2.Count;
        var cost = dp[i][j]; // The total alignment cost

        var alignment1 = new List<T>();
        var alignment2 = new List<T>();

        // Backtrack to construct the optimal alignment
        while (i > 0 && j > 0)
        {
            // Determine if the current position was a match/mismatch or a gap
            var currentSymbolPenalty = sequence1[i - 1].Equals(sequence2[j - 1]) ? 0 : symbolPenalty;

            if (dp[i][j] == dp[i - 1][j - 1] + currentSymbolPenalty)
            {
                // Match/Mismatch: align the current symbols from both sequences
                alignment1.Add(sequence1[i - 1]);
                alignment2.Add(sequence2[j - 1]);
                i--;
                j--;
            }
            else if (dp[i][j] == dp[i - 1][j] + gapPenalty)
            {
                // Gap in sequence2: align a symbol from sequence1 with a gap
                alignment1.Add(sequence1[i - 1]);
                alignment2.Add(gapSymbol);
                i--;
            }
            else
            {
                // Gap in sequence1: align a symbol from sequence2 with a gap
                alignment1.Add(gapSymbol);
                alignment2.Add(sequence2[j - 1]);
                j--;
            }
        }

        // If there are remaining symbols in sequence1, align them with gaps
        while (i > 0)
        {
            alignment1.Add(sequence1[i - 1]);
            alignment2.Add(gapSymbol);
            i--;
        }

        // If there are remaining symbols in sequence2, align them with gaps
        while (j > 0)
        {
            alignment1.Add(gapSymbol);
            alignment2.Add(sequence2[j - 1]);
            j--;
        }

        // Reverse the alignments since we built them backwards
        alignment1.Reverse();
        alignment2.Reverse();

        return (alignment1, alignment2, cost);
    }
}
