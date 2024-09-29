using TextProcessing.Sorting.External;

namespace TextProcessing.ExamplesRunning.Sorting.External;

public static class TextGeneratorExample
{
    public static void Run(string[] args)
    {
        string? outputFilePath = GetUserOutputFilePath(args);
        long? targetSizeInBytes = GetUserFileSize(args);
        string? seedFilePath = GetUserSeedFilePath(args);

        TextGenerator.GenerateFile(outputFilePath, targetSizeInBytes, seedFilePath);
    }

static string? GetUserOutputFilePath(string[] args)
    {
        string? filePath = null;

        // Parse command-line arguments
        if (args.Length >= 1)
            filePath = args[0];

        return filePath;
    }

    static long? GetUserFileSize(string[] args)
    {
        if (args.Length < 2)
            return null;

        if (!FileSizeHelper.ParseSize(args[1], out long sizeInBytes))
            throw new ArgumentException("Invalid size format. Use numbers followed by optional unit (e.g., 100MB, 1GB).");

        return sizeInBytes;
    }

    static string? GetUserSeedFilePath(string[] args)
    {
        string? seedFilePath = null;

        // Parse command-line arguments
        if (args.Length >= 3)
            seedFilePath = args[2];

        return seedFilePath;
    }
}
