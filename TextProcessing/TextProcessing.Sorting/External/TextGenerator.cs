namespace TextProcessing.Sorting.External;

public class TextGenerator
{
    // Default values
    private static readonly string _defaultOutputFilePath = "source.txt";
    private static readonly long _defaultSizeInBytes = 1L * 1024 * 1024 * 1024;//1L * 1024 * 1024 * 1024; // Default to 1GB
    private static readonly string _defaultSeedFilePath = "seed.txt";
    private static readonly long _defaultReportInterval = 10L * 1024 * 1024; // Report every 50MB

    public static void GenerateFile(string? outputFilePath, long? targetSizeInBytes, string? seedFilePath)
    {
        if (targetSizeInBytes is null)
        {
            Console.WriteLine("No target size specified. Using default size (1GB).");
            targetSizeInBytes = _defaultSizeInBytes;
        }

        if (string.IsNullOrWhiteSpace(outputFilePath))
        {
            Console.WriteLine($"No output file path specified. Using default output file path ({_defaultOutputFilePath}).");
            outputFilePath = _defaultOutputFilePath;
        }

        if (string.IsNullOrWhiteSpace(seedFilePath))
        {
            Console.WriteLine($"No seed file path specified. Using default seed file path ({_defaultSeedFilePath}).");
            seedFilePath = _defaultSeedFilePath;
        }

        Console.WriteLine($"Starting file generation at {DateTime.Now}");
        Console.WriteLine($"Generating file: {outputFilePath}");
        Console.WriteLine($"Target size: {FormatSize(targetSizeInBytes.Value)}");

        var random = new Random();
        var stringOptions = LoadSeedStrings(seedFilePath);

        using var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8, 65536);

        long totalBytesWritten = 0;
        long lastReportedSize = 0;

        while (totalBytesWritten < targetSizeInBytes)
        {
            int number = random.Next(1, int.MaxValue);
            string text = stringOptions[random.Next(stringOptions.Count)];

            string line = $"{number}. {text}";
            writer.WriteLine(line);

            // Estimate the bytes written
            totalBytesWritten += Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;

            // Report progress
            if (totalBytesWritten - lastReportedSize >= _defaultReportInterval)
            {
                lastReportedSize = totalBytesWritten;
                var percentage = (double)totalBytesWritten / targetSizeInBytes * 100;

                Console.WriteLine($"Progress: {percentage:F2}% ({FormatSize(totalBytesWritten)} of {FormatSize(targetSizeInBytes.Value)})");
            }
        }

        Console.WriteLine($"File generation completed at {DateTime.Now}");
    }

    public static bool ParseSize(string sizeStr, out long sizeInBytes)
    {
        sizeInBytes = 0;
        sizeStr = sizeStr.Trim().ToUpper();
        long multiplier = 1;

        if (sizeStr.EndsWith("GB"))
        {
            multiplier = 1024L * 1024 * 1024;
            sizeStr = sizeStr[..^2];
        }

        else if (sizeStr.EndsWith("MB"))
        {
            multiplier = 1024L * 1024;
            sizeStr = sizeStr[..^2];
        }

        else if (sizeStr.EndsWith("KB"))
        {
            multiplier = 1024L;
            sizeStr = sizeStr[..^2];
        }

        if (long.TryParse(sizeStr, out long size))
        {
            sizeInBytes = size * multiplier;
            return true;
        }

        return false;
    }

    public static string FormatSize(long sizeInBytes) => sizeInBytes switch
    {
        >= 1024L * 1024 * 1024 => $"{sizeInBytes / (1024L * 1024 * 1024)} GB",
        >= 1024L * 1024 => $"{sizeInBytes / (1024L * 1024)} MB",
        >= 1024L => $"{sizeInBytes / 1024L} KB",
        _ => $"{sizeInBytes} bytes"
    };

    static List<string> LoadSeedStrings(string seedStringsFile)
    {
        try
        {
            var seedStrings = new List<string>();
            using (var reader = new StreamReader(seedStringsFile))
            {
                string? line;
                while ((line = reader.ReadLine()) is not null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        seedStrings.Add(line.Trim());
                }
            }

            if (seedStrings.Count is 0)
                throw new InvalidOperationException("No seed strings found in the seed file.");

            return seedStrings;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading seed strings file: {ex.Message}");
            throw;
        }
    }
}
