namespace Biology.Common;

public static class SequenceConcatenationAligner
{
    /// <summary>
    /// Finds the minimum alignment cost and returns the aligned sequences for the target and concatenated sequences from the library.
    /// </summary>
    /// <param name="A">The target sequence to align with.</param>
    /// <param name="L">The library of sequences to concatenate from.</param>
    /// <param name="symbolPenalty">The penalty for mismatching symbols (substitution).</param>
    /// <param name="gapPenalty">The penalty for introducing a gap (insertion or deletion).</param>
    /// <param name="gapSymbol">The symbol used to represent gaps in the alignment.</param>
    /// <returns>
    /// A tuple containing the aligned target sequence, the concatenated sequence, and the total alignment cost.
    /// </returns>
    public static (List<T> alignedTarget, List<T> alignedConcatenation, double cost) AlignTargetWithLibrary<T>(
        List<T> A, List<List<T>> L, double symbolPenalty, double gapPenalty, T gapSymbol)
        where T : notnull
    {
        int m = A.Count; // Length of target sequence A

        // DP array to store minimum alignment cost
        var dp = new double[m + 1];

        // Array to store the alignment sequences at each step
        var alignedTargetSequence = new List<T>[m + 1];
        var alignedLibrarySequence = new List<T>[m + 1];

        // Initialize DP array and alignment sequences
        for (int i = 0; i <= m; i++)
        {
            dp[i] = double.MaxValue;
            alignedTargetSequence[i] = [];
            alignedLibrarySequence[i] = [];
        }

        dp[0] = 0; // Base case: aligning an empty prefix has zero cost

        // Iterate over the length of the target sequence A
        for (int i = 1; i <= m; i++)
        {
            // For each prefix A[1..i], try aligning it with each sequence in L
            foreach (var B in L)
            {
                int lenB = B.Count;
                if (i < lenB)               
                    continue;
                
                // Get the substring of A that corresponds to the current sequence B
                var A_sub = A.GetRange(i - lenB, lenB);

                // Use the Needleman-Wunsch algorithm to get the alignment cost and aligned sequences
                var (alignedA, alignedB, cost) = ProteinSequenceAligning<T>.Align(A_sub, B, symbolPenalty, gapPenalty, gapSymbol);

                // If this alignment is better, update dp[i] and store the aligned sequences
                if (dp[i - lenB] + cost < dp[i])
                {
                    dp[i] = dp[i - lenB] + cost;

                    // Update aligned sequences by combining previous aligned sequence with the new alignment
                    alignedTargetSequence[i] = new List<T>(alignedTargetSequence[i - lenB]);
                    alignedTargetSequence[i].AddRange(alignedA);

                    alignedLibrarySequence[i] = new List<T>(alignedLibrarySequence[i - lenB]);
                    alignedLibrarySequence[i].AddRange(alignedB);
                }
            }
        }

        // The final answer includes the aligned sequences and the total alignment cost
        return (alignedTargetSequence[m], alignedLibrarySequence[m], dp[m]);
    }
}


