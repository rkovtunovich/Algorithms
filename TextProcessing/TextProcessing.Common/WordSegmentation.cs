
namespace TextProcessing.Common;

public class WordSegmentation
{
    /// <summary>
    /// Segments the string y to maximize the total quality of its constituent words.
    /// </summary>
    /// <param name="y">The input string to segment.</param>
    /// <param name="quality">A function that returns the quality of a given word.</param>
    /// <returns>A tuple with the list of segmented words and the maximum total quality.</returns>
    public static (List<string> Segmentation, int MaxQuality) SegmentString(string y, Func<string, int> quality)
    {
        int n = y.Length;
        var dp = new int[n + 1];
        var prev = new int[n + 1]; // To track the split points

        // Initialize base case
        dp[0] = 0;
        prev[0] = -1; // No predecessor for the empty string

        // Iterate through the string to fill dp[] table
        for (int i = 1; i <= n; i++)
        {
            dp[i] = int.MinValue; // Initialize with a very low value
            // Check all possible words ending at position i (from j to i)
            for (int j = 0; j < i; j++)
            {
                string word = y.Substring(j, i - j); // Substring from j to i-1
                int currentQuality = quality(word); // Get quality of the word

                // Update dp[i] if the segmentation ending at i is better
                if (dp[j] + currentQuality > dp[i])
                {
                    dp[i] = dp[j] + currentQuality;
                    prev[i] = j; // Store the split point
                }
            }
        }

        // Backtrack to find the optimal segmentation
        var segmentation = new List<string>();
        int current = n;
        while (current > 0)
        {
            int splitPoint = prev[current];
            segmentation.Add(y[splitPoint..current]);
            current = splitPoint;
        }

        segmentation.Reverse(); // Reverse the list to get the segmentation in the correct order

        return (segmentation, dp[n]);
    }
}
