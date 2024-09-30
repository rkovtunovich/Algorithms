namespace TextProcessing.Sorting.External;

// Data structure to hold the parsed number and text parts from each line
public struct LineData
{
    public int NumberPart { get; set; }     // The numeric part of the line
    public string TextPart { get; set; } = null!;  // The text part of the line

    // Constructor that parses the line into its number and text components
    public LineData(string line)
    {
        ParseLine(line);
    }

    // Parse a line in the format "Number. String"
    private void ParseLine(string? line)
    {
        if (line is null)
        {
            NumberPart = 0;
            TextPart = string.Empty;
            return;
        }

        int dotIndex = line.IndexOf('.');
        // Extract the number and text parts
        if (dotIndex > 0 && int.TryParse(line.AsSpan(0, dotIndex), out int number))
        {
            TextPart = line[(dotIndex + 1)..].Trim();
            NumberPart = number;
        }
        else
        {
            NumberPart = 0;
            TextPart = line;
        }
    }

    // Return the line as a string in the original format
    public override string? ToString()
    {
        return $"{NumberPart}. {TextPart}";
    }
}