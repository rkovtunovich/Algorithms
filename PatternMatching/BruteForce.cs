namespace PatternMatching;

public static class BruteForce
{
    // Returns the index of the first occurrence of pattern in text, or -1 if not found
    public static int BruteForceSearch(string text, string pattern)
    {
        int n = text.Length;
        int m = pattern.Length;
        for (int i = 0; i <= n - m; i++) // Loop over starting positions
        {
            int j = 0;
            while (j < m && text[i + j] == pattern[j]) // Loop over characters
            {
                j++;
            }

            if (j == m) // Found a match
                return i;
        }

        return -1; // Not found
    }
}
