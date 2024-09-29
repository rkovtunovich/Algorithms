namespace TextProcessing.Sorting.External;

public class TextGenerator
{
    // Default values
    private static readonly string _defaultOutputFilePath = "source.txt";
    private static readonly long _defaultSizeInBytes = 10L * 1024 * 1024 * 1024; // Default to 10GB
    private static readonly string _defaultSeedFilePath = "external\\seed.txt";
    private static readonly long _defaultReportInterval = 100L * 1024 * 1024; // Report every 100MB

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
        Console.WriteLine($"Target size: {FileSizeHelper.Format(targetSizeInBytes.Value)}");

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

                Console.WriteLine($"Progress: {percentage:F2}% ({FileSizeHelper.Format(totalBytesWritten)} of {FileSizeHelper.Format(targetSizeInBytes.Value)})");
            }
        }

        Console.WriteLine($"File generation completed at {DateTime.Now}");
    }

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
