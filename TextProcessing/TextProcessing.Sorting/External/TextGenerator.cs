namespace TextProcessing.Sorting.External;

public class TextGenerator
{
    // Default output file path, size, seed file path, and progress reporting interval
    private static readonly string _defaultOutputFilePath = "source.txt";
    private static readonly string _defaultSeedFilePath = "external\\seed.txt";

    private static readonly long _defaultSizeInBytes = 100L * 1024 * 1024 * 1024; // Default to 100GB
    private static readonly long _defaultReportInterval = 500L * 1024 * 1024; // Report every 500MB

    private static readonly int _bufferSize = 4 * 1024 * 1024; // 4MB buffer size for writing

    // Main method to generate the file
    public static void GenerateFile(string? outputFilePath, long? targetSizeInBytes, string? seedFilePath)
    {
        // If no file size specified, use the default value
        if (targetSizeInBytes is null)
        {
            Console.WriteLine($"No target size specified. Using default size ({FileSizeHelper.Format(_defaultSizeInBytes)}).");
            targetSizeInBytes = _defaultSizeInBytes;
        }

        // If no output file path specified, use the default value
        if (string.IsNullOrWhiteSpace(outputFilePath))
        {
            Console.WriteLine($"No output file path specified. Using default output file path ({_defaultOutputFilePath}).");
            outputFilePath = _defaultOutputFilePath;
        }

        // If no seed file path specified, use the default value
        if (string.IsNullOrWhiteSpace(seedFilePath))
        {
            Console.WriteLine($"No seed file path specified. Using default seed file path ({_defaultSeedFilePath}).");
            seedFilePath = _defaultSeedFilePath;
        }

        Console.WriteLine($"Starting file generation at {DateTime.Now}");
        Console.WriteLine($"Generating file: {outputFilePath}");
        Console.WriteLine($"Target size: {FileSizeHelper.Format(targetSizeInBytes.Value)}");

        var random = new Random();
        // Load the possible string parts from the seed file
        var stringOptions = LoadSeedStrings(seedFilePath);

        // Open a StreamWriter with a buffer to write the generated lines
        using var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8, _bufferSize);

        long totalBytesWritten = 0; // Track the total bytes written
        long lastReportedSize = 0;  // Track size for progress reporting

        // Continue generating lines until the target file size is reached
        while (totalBytesWritten < targetSizeInBytes)
        {
            // Generate a random number part for the line
            int number = random.Next(1, int.MaxValue);
            // Randomly select a string part from the seed file
            string text = stringOptions[random.Next(stringOptions.Count)];

            // Create the line with number and string
            string line = $"{number}. {text}";
            writer.WriteLine(line);

            // Estimate the bytes written for the line (including newline)
            totalBytesWritten += Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;

            // Report progress every _defaultReportInterval bytes (100MB by default)
            if (totalBytesWritten - lastReportedSize >= _defaultReportInterval)
            {
                lastReportedSize = totalBytesWritten;
                var percentage = (double)totalBytesWritten / targetSizeInBytes * 100;

                // Show progress in percentage and bytes written
                Console.WriteLine($"Progress: {percentage:F2}% ({FileSizeHelper.Format(totalBytesWritten)} of {FileSizeHelper.Format(targetSizeInBytes.Value)})");
            }
        }

        Console.WriteLine($"File generation completed at {DateTime.Now}");
    }

    // Load possible string options from the seed file (one string per line)
    static List<string> LoadSeedStrings(string seedStringsFile)
    {
        try
        {
            var seedStrings = new List<string>();
            using (var reader = new StreamReader(seedStringsFile))
            {
                string? line;
                // Read each non-empty line and add to the list of string options
                while ((line = reader.ReadLine()) is not null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        seedStrings.Add(line.Trim());
                }
            }

            // If no valid strings were found in the seed file, throw an error
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
