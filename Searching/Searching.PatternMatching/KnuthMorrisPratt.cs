using DataStructures.Lists;
namespace Searching.PatternMatching;

#region Description

// The Knuth-Morris-Pratt(KMP) algorithm is a string matching algorithm that efficiently searches for occurrences of a pattern in a text.
// It was invented by Donald Knuth, Vaughan Pratt, and James Morris in 1977.

// The KMP algorithm works by preprocessing the pattern to determine a partial match table, also called a failure function.
// This table is used during the matching process to avoid unnecessary comparisons between the pattern and the text.
// The failure function for a pattern is a table that stores the length of the longest proper suffix of each prefix of the pattern that is also a prefix of the pattern.
// In simpler terms, the failure function stores the length of the longest border of each prefix of the pattern.

// Using this table, the KMP algorithm can skip ahead in the text when a mismatch occurs, instead of starting over at the beginning of the pattern.
// Specifically, the algorithm uses the failure function to determine how many characters in the pattern can be skipped when a mismatch occurs.
// This allows the algorithm to achieve a time complexity of O(m+n), where m is the length of the pattern and n is the length of the text.

// Here is a high-level overview of the KMP algorithm:

// Preprocess the pattern to compute the failure function.
// Initialize two pointers, i and j, to 0.
// While i < n (the length of the text):
// If the current characters in the text and pattern match, increment both i and j.
// If j = m(the length of the pattern), a match has been found.Store the index of the match and update j using the failure function.
// If the current characters do not match and j > 0, update j using the failure function.
// If the current characters do not match and j = 0, increment i.
// The KMP algorithm is widely used in computer science and engineering, particularly in applications that involve searching large amounts of text for patterns.

#endregion

public static class KnuthMorrisPratt
{
    public static SequentialList<int> Match(string text, string pattern)
    {
        var matches = new SequentialList<int>();
        int n = text.Length;
        int m = pattern.Length;
        int[] failureFunction = ComputeFailureFunction(pattern);

        int i = 0;
        int j = 0;
        while (i < n)
        {
            if (text[i] == pattern[j])
            {
                i++;
                j++;
                if (j == m)
                {
                    matches.Add(i - j);
                    j = failureFunction[j - 1];
                }
            }
            else if (j > 0)
            {
                j = failureFunction[j - 1];
            }
            else
            {
                i++;
            }
        }

        return matches;
    }

    private static int[] ComputeFailureFunction(string pattern)
    {
        int m = pattern.Length;
        int[] failureFunction = new int[m];
        int i = 1;
        int j = 0;

        while (i < m)
        {
            if (pattern[i] == pattern[j])
            {
                failureFunction[i] = j + 1;
                i++;
                j++;
            }
            else if (j > 0)
            {
                j = failureFunction[j - 1];
            }
            else
            {
                failureFunction[i] = 0;
                i++;
            }
        }

        return failureFunction;
    }
}
