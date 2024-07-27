namespace Spacing.Common.TwoDimension;

public class HiddenSurfaceRemoval
{
    public static List<Line> FindVisibleLines(List<Line> lines)
    {
        // Sort lines by their slopes
        lines = lines.OrderBy(l => l.Slope).ToList();

        // Handle parallel lines by removing the ones with smaller intercepts
        lines = HandleParallelLines(lines);

        // Find visible lines using a recursive approach
        var visibleLines = FindVisibleLinesRecursively(lines, 0, lines.Count - 1);

        return visibleLines;
    }

    private static List<Line> FindVisibleLinesRecursively(List<Line> lines, int start, int end)
    {
        // Base cases
        var linesCount = end - start + 1;
        if (linesCount is 1)        
            return [lines[start]];

        // Split the list into two halves and process them recursively
        var middle = start + (end - start) / 2;

        var leftVisibleLines = FindVisibleLinesRecursively(lines, start, middle);
        var rightVisibleLines = FindVisibleLinesRecursively(lines, middle + 1, end);

        // Merge the results from the two halves
        return MergeVisibleLines(leftVisibleLines, rightVisibleLines);
    }

    private static List<Line> MergeVisibleLines(List<Line> leftLines, List<Line> rightLines)
    {
        var totalLines = leftLines.Count + rightLines.Count;

        // Initialize mergedLines with the first two lines
        var mergedLines = new List<Line>
        {
            leftLines[0],
            leftLines.Count > 1 ? leftLines[1] : rightLines[0]
        };

        // Initialize the number of visible lines
        int numVisible = mergedLines.Count;

        for (int k = numVisible; k < totalLines; k++)
        {
            var currentLine = k < leftLines.Count ? leftLines[k] : rightLines[k - leftLines.Count];
            var lastLine = mergedLines[numVisible - 1];
            var nextToLastLine = mergedLines[numVisible - 2];

            // Calculate the intersection points of the current line with the last two lines
            double prevIntersect = (lastLine.YIntercept - nextToLastLine.YIntercept) / (nextToLastLine.Slope - lastLine.Slope);
            double currIntersect = (currentLine.YIntercept - nextToLastLine.YIntercept) / (nextToLastLine.Slope - currentLine.Slope);

            // If the current line intersects before the last visible line, it means the last visible line is hidden
            // Remove the last visible line and update the number of visible lines
            // Repeat this process until the current line intersects after the last visible line
            // or there is only one visible line left
            while (currIntersect < prevIntersect)
            {
                mergedLines.RemoveAt(numVisible - 1);
                numVisible--;

                if (numVisible is 1)
                    break;

                lastLine = mergedLines[numVisible - 1];
                nextToLastLine = mergedLines[numVisible - 2];
                prevIntersect = (lastLine.YIntercept - nextToLastLine.YIntercept) / (nextToLastLine.Slope - lastLine.Slope);
                currIntersect = (currentLine.YIntercept - nextToLastLine.YIntercept) / (nextToLastLine.Slope - currentLine.Slope);
            }

            mergedLines.Add(currentLine);
            numVisible++;
        }

        return mergedLines;
    }

    private static List<Line> HandleParallelLines(List<Line> lines)
    {
        if(lines.Count <= 1)
            return lines;

        var noParallelLines = new List<Line>();
        
        var currentLineIndex = 0;
        var nextLineIndex = 1;

        while (nextLineIndex < lines.Count)
        {
            var currentLine = lines[currentLineIndex];
            var nextLine = lines[nextLineIndex];

            // If the slopes are equal, keep the one with the highest intercept
            while (currentLine.Slope == nextLine.Slope)
            {
                if (currentLine.YIntercept < nextLine.YIntercept)
                {
                    currentLineIndex = nextLineIndex;
                    currentLine = nextLine;
                }

                nextLineIndex++;

                if (nextLineIndex >= lines.Count)
                {
                    noParallelLines.Add(currentLine);
                    return noParallelLines;
                }           
                    
                nextLine = lines[nextLineIndex];
            }

            noParallelLines.Add(currentLine);
            currentLineIndex = nextLineIndex;
            nextLineIndex++;

            // add the last line if it's not parallel to the previous one
            if (nextLineIndex >= lines.Count)
                noParallelLines.Add(nextLine);
        }

        return noParallelLines;
    }
}
