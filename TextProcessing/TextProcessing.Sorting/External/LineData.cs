namespace TextProcessing.Sorting.External;

public struct LineData
{
    public int NumberPart { get; set; }

    public string TextPart { get; set; } = null!;

    public LineData(string line)
    {
        ParseLine(line);
    }

    private void ParseLine(string? line)
    {
        if (line is null)
        {
            NumberPart = 0;
            TextPart = string.Empty;
            return;
        }

        int dotIndex = line.IndexOf('.');
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

    public override string? ToString()
    {
        return $"{NumberPart}. {TextPart}";
    }
}
