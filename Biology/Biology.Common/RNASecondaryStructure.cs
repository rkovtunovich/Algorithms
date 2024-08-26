namespace Biology.Common;

public class RNASecondaryStructure
{
    // Problem definition: Given an RNA sequence, composed of the nucleotides (A, C, G, U),
    // find the maximum number of base pairs that can be formed by the sequence.
    // A base pair is a pair of nucleotides that are complementary to each other.
    // The nucleotides 'A' and 'U' are complementary to each other, as are 'C' and 'G'.
    // The base pairs must be non-overlapping, meaning that if a base pair is formed between nucleotides i and j,
    // no other base pair can be formed between nucleotides i and j or any nucleotides between i and j (no crossing)
    // The function should return the maximum number of base pairs that can be formed and the indices of the nucleotides that form the base pairs.
    //
    // Solution: The problem can be solved using dynamic programming.
    // 1. Define dp state: dp[i, j] stores the maximum number of base pairs that can be formed by the subsequence rna[i..j]
    // 2. Base case: dp[i, i] = 0, dp[i, j] = 0 if j < i
    // 3. Transition: for each subsequence rna[i..j], the maximum number of base pairs can be formed by either:
    //    a. Not forming a base pair between nucleotides i and j: dp[i, j] = dp[i, j - 1]
    //    b. Forming a base pair between j and a nucleotide k, where i <= k < j and rna[j] and rna[k] are complementary:
    // Recurrence relation: dp[i, j] = max(dp[i, j - 1], max(1 + dp[i, k - 1] + dp[k + 1, j])) for i <= k < j
    public static List<(int, int)> FindMaxBasePairs(string rna, int minLoopLength = 4)
    {
        // rna length
        int n = rna.Length;

        // dp[i, j] stores the maximum number of base pairs that can be formed by the subsequence rna[i..j]
        int[,] dp = new int[n, n];

        // traceBack[i, j] stores the index of the nucleotide that forms a base pair with the nucleotide at index j in the subsequence rna[i..j]
        int[,] traceBack = new int[n, n];

        // Fill the dp table
        for (int len = 1; len < n; len++)
        {
            for (int i = 0; i < n - len; i++)
            {
                int j = i + len;
                dp[i, j] = dp[i, j - 1]; // Not forming a base pair between nucleotides i and j
                traceBack[i, j] = -1;

                if(j - i <= minLoopLength)
                    continue; // Skip if the loop length is less than the minimum loop length

                for (int k = i; k <= j; k++)
                {
                    if (!AreComplementary(rna[k], rna[j]))                   
                        continue;
                    
                    int val = 1 + (k - 1 > i ? dp[i + 1, k - 1] : 0) + (k + 1 < j ? dp[k + 1, j] : 0);
                    if (val > dp[i, j])
                    {
                        dp[i, j] = val;
                        traceBack[i, j] = k;
                    }
                }
            }
        }

        var basePairs = BacktrackPairs(traceBack, 0, n - 1);

        return basePairs;
    }

    private static bool AreComplementary(char a, char b) => (a, b) switch
    {
        ('A', 'U') => true,
        ('U', 'A') => true,
        ('C', 'G') => true,
        ('G', 'C') => true,
        _ => false
    };

    private static List<(int, int)> BacktrackPairs(int[,] traceback, int i, int j)
    {
        var pairs = new List<(int, int)>();

        if (i >= j) 
            return pairs;

        int k = traceback[i, j];

        if (k != -1)
        {
            // Add the valid pair (k, j)
            pairs.Add((k, j));

            // Recursively add pairs from left and right parts
            pairs.AddRange(BacktrackPairs(traceback, i, k - 1));
            pairs.AddRange(BacktrackPairs(traceback, k + 1, j - 1));
        }
        else
        {
            // No valid pair, move to the next character
            pairs.AddRange(BacktrackPairs(traceback, i, j - 1));
        }

        return pairs;
    }
}
