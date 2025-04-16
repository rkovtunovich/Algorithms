namespace TextProcessing.PatternMatching;

public static class Interleaving
{
    public static bool IsInterleaving(string sequence, string patternX, string patternY)
    {
        // Handle trivial cases: both patterns empty.
        if (string.IsNullOrEmpty(patternX) && string.IsNullOrEmpty(patternY))       
            return string.IsNullOrEmpty(sequence);
       
        // If one of the patterns is empty, then the entire sequence must be a prefix
        // of the infinite repetition of the other.
        if (string.IsNullOrEmpty(patternX))        
            return IsPrefixOfRepeatedPattern(sequence, patternY);
        
        if (string.IsNullOrEmpty(patternY))    
            return IsPrefixOfRepeatedPattern(sequence, patternX);
        
        // If both patterns are identical, then—by your specification—the only valid sequence
        // is one that comes from a single prefix of the repeated pattern (no interleaving of two parts).
        // This prevents cases like "1011" for pattern "10" because:
        //   infinite repetition of "10" is "101010...", and its valid prefixes are "", "1", "10", "101", "1010", ...
        //   "1011" is not one of these.
        if (patternX == patternY)     
            return IsPrefixOfRepeatedPattern(sequence, patternX);
        
        // Otherwise, patterns are different – so use the 2D DP approach.
        int lenS = sequence.Length;
        int lenX = patternX.Length;
        int lenY = patternY.Length;
        bool[,] dp = new bool[lenS + 1, lenS + 1];

        // Base case: no characters processed, no letters assigned.
        dp[0, 0] = true;

        for (int i = 0; i < lenS; i++)
        {
            for (int usedX = 0; usedX <= i; usedX++)
            {
                if (!dp[i, usedX])
                    continue;
                int usedY = i - usedX;
                char c = sequence[i];

                // Option 1: assign sequence[i] to patternX.
                if (patternX[usedX % lenX] == c)
                    dp[i + 1, usedX + 1] = true;

                // Option 2: assign sequence[i] to patternY.
                if (patternY[usedY % lenY] == c)
                    dp[i + 1, usedX] = true;
            }
        }

        // If any state at the end is reachable then the sequence is valid.
        for (int usedX = 0; usedX <= lenS; usedX++)
        {
            if (dp[lenS, usedX])
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if 'candidate' is a prefix of an infinite repetition of 'pattern'.
    /// For example, candidate = "0101", pattern = "01" returns true,
    /// candidate = "1011", pattern = "10" returns false.
    /// </summary>
    private static bool IsPrefixOfRepeatedPattern(string candidate, string pattern)
    {
        int pLen = pattern.Length;
        for (int i = 0; i < candidate.Length; i++)
        {
            int modPos = i % pLen;
            if (candidate[i] != pattern[modPos])
                return false;
        }

        return true;
    }
}

