namespace TextProcessing.Sorting.External;

class LineComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x is null)
            return y is null ? 0 : -1;

        if (y is null)
            return 1;

        // Parse lines into Number and String parts
        ParseLine(x, out var numberX, out var stringX);
        ParseLine(y, out var numberY, out var stringY);

        // Compare String parts first
        int stringCompare = string.Compare(stringX, stringY, StringComparison.Ordinal);
        if (stringCompare is not 0)
            return stringCompare;

        // If String parts are equal, compare Number parts
        return numberX.CompareTo(numberY);
    }

    static void ParseLine(string? line, out int number, out string text)
    {
        if (line is null)
        {
            number = 0;
            text = string.Empty;
            return;
        }

        int dotIndex = line.IndexOf('.');
        if (dotIndex > 0 && int.TryParse(line.AsSpan(0, dotIndex), out number))
        {
            text = line[(dotIndex + 1)..].Trim();
        }
        else
        {
            number = 0;
            text = line;
        }
    }
}
