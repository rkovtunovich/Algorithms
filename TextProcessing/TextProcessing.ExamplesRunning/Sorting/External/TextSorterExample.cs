using TextProcessing.Sorting.External;

namespace TextProcessing.ExamplesRunning.Sorting.External;

public static class TextSorterExample
{
    public static void Run(string[] args)
    {
        var inputFilePath = GetUserInputFilePath(args);
        var outputFilePath = GetUserOutputFilePath(args);

        TextSorter.SortFile(inputFilePath, outputFilePath);
    }

    static string? GetUserInputFilePath(string[] args)
    {
        string? filePath = null;

        // Parse command-line arguments
        if (args.Length >= 1)
            filePath = args[0];

        return filePath;
    }

    static string? GetUserOutputFilePath(string[] args)
    {
        string? filePath = null;

        // Parse command-line arguments
        if (args.Length >= 2)
            filePath = args[1];

        return filePath;
    }
}
