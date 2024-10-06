namespace TextProcessing.Common;

public class PrettyPrinting
{
    /// <summary>
    /// Finds the optimal partition of words into lines with the minimum total slack cost.
    /// The slack cost is the square of the number of extra spaces at the end of each line.
    /// </summary>
    /// <param name="text">The text to format.</param>
    /// <param name="L">Maximum line length.</param>
    /// <returns>A tuple containing the formatted text and the total cost.</returns>
    public static (string formattedText, int cost) PrettyPrint(string text, int L)
    {
        // Split the text into words and convert them to integers
        var textSpan = text.AsSpan();
        var words = SplitTextToWords(textSpan, L);

        int n = words.Count;
        var dp = new int[n + 1];
        var split = new int[n + 1]; // To store where each line starts for reconstruction

        // Initialize dp array with a large number to minimize
        Array.Fill(dp, int.MaxValue);
        dp[0] = 0; // Base case, no words means no cost

        // Iterate through each word and compute the minimum cost
        for (int i = 1; i <= n; i++)
        {
            int lineLength = 0; // Line length to calculate the current line
            for (int j = i; j >= 1; j--)
            {
                var currentWordLength = words[j - 1].Length;

                // Add current word length
                lineLength += currentWordLength;

                // If line length exceeds maximum, stop
                if (lineLength > L)
                    break;

                // Calculate slack and cost only if the line is valid
                // Add spaces only if there are more words to follow in the line
                int existingWordsInLine = i - j;
                if (existingWordsInLine > 0)
                    lineLength++;  // Add the spaces between words, except for the last word

                // check if the line length exceeds the maximum after adding the spaces
                if (lineLength > L)
                    break;

                // Calculate slack
                int slack = L - lineLength;

                // Update dp[i] if this configuration is better
                int cost = dp[j - 1] + slack * slack;
                if (cost < dp[i])
                {
                    dp[i] = cost;
                    split[i] = j; // Store the split point
                }
            }
        }

        // Reconstruct the lines based on the split points
        var linesLengths = ReconstructLines(split, n);

        // Format the text based on the split points
        var builder = new StringBuilder();
        var currentWordIndex = 0;
        for (int i = 0; i < linesLengths.Count; i++)
        {
            // use the current line length data and the textSpan to get the actual words
            var line = linesLengths[i];
            for (int j = 0; j < line.Count; j++)
            {
                var word = words[currentWordIndex].GetWordText(textSpan);
                builder.Append(word);
                if (j < line.Count - 1)
                    builder.Append(' ');

                currentWordIndex++;
            }

            if (i < linesLengths.Count - 1)
                builder.AppendLine();
        }

        string formattedText = builder.ToString();

        return (formattedText, dp[n]);
    }

    /// <summary>
    /// Reconstructs the lines based on the split points stored in the dp array.
    /// </summary>
    private static List<List<int>> ReconstructLines(int[] split, int n)
    {
        var result = new List<List<int>>();
        int i = n;
        while (i > 0)
        {
            var line = new List<int>();
            for (int j = split[i] - 1; j < i; j++)        
                line.Add(j);
            
            result.Insert(0, line);
            i = split[i] - 1;
        }

        return result;
    }

    /// <summary>
    /// Splits the text into words.
    /// </summary>
    /// <param name="text">The text to split.</param>
    /// <param name="L">The maximum line length.</param>
    /// <returns>A list of words.</returns>
    private static List<Word> SplitTextToWords(ReadOnlySpan<char> text, int L)
    {
        // if we have new lines in the text, we need to ignore them
        var words = new List<Word>();

        int startIndex = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (!char.IsWhiteSpace(text[i]))           
                continue;
            
            if (i > startIndex)
            {
                var word = new Word(startIndex, i - 1);
                if (word.Length > L)
                    throw new ArgumentException($"Word '{word.GetWordText(text)}' exceeds the maximum line length of {L}");

                words.Add(word);
            }
               
            startIndex = i + 1;
        }

        if (startIndex < text.Length)
        {
            var word = new Word(startIndex, text.Length - 1);
            if (word.Length > L)
                throw new ArgumentException($"Word '{word.GetWordText(text)}' exceeds the maximum line length of {L}");

            words.Add(word);
        }           

        return words;
    }
}
