namespace TextProcessing.Sorting.External;

class LineComparer : IComparer<LineData>
{
    public int Compare(LineData x, LineData y)
    {
        // Compare String parts first
        int stringCompare = string.Compare(x.TextPart, y.TextPart, StringComparison.Ordinal);
        if (stringCompare is not 0)
            return stringCompare;

        // If String parts are equal, compare Number parts
        return x.NumberPart.CompareTo(y.NumberPart);
    }
}