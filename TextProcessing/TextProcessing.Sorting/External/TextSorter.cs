using DataStructures.Heaps;
using System.Diagnostics;

namespace TextProcessing.Sorting.External;

public class TextSorter
{
    // Default file paths and chunk size for sorting
    private static readonly string _defaultInputFilePath = "source.txt";
    private static readonly string _defaultOutputFilePath = "sorted.txt";
    private static readonly string _tempFolder = "temp";
    private static readonly long _defaultChunkSize = 500L * 1024 * 1024; // 500MB chunk size for sorting
    private static readonly int _defaultReportInterval = 100 * 1024 * 1024; // Report progress every 100MB

    // Buffer sizes for reading/writing files
    private static readonly int _bufferSizeSortingPhase = 4 * 1024 * 1024; // 4MB buffer size for sorting
    private static readonly int _bufferSizeMergingPhase = 1024 * 1024; // 1MB buffer size for merging

    // Main method to sort the file
    public static void SortFile(string? inputFilePath, string? outputFilePath, long? chunkSize = null)
    {
        // Use default input/output paths if none are provided
        if (string.IsNullOrWhiteSpace(inputFilePath))
        {
            Console.WriteLine($"No input file path specified. Using default input file path ({_defaultInputFilePath}).");
            inputFilePath = _defaultInputFilePath;
        }

        if (!File.Exists(inputFilePath))
            throw new FileNotFoundException("Input file not found.", inputFilePath);

        if (string.IsNullOrWhiteSpace(outputFilePath))
        {
            Console.WriteLine($"No output file path specified. Using default output file path ({_defaultOutputFilePath}).");
            outputFilePath = _defaultOutputFilePath;
        }

        if(chunkSize is null)
        {
            Console.WriteLine($"No chunk size specified. Using default chunk size: {FileSizeHelper.Format(_defaultChunkSize)}");
            chunkSize = _defaultChunkSize;
        }

        var sizeInBytes = new FileInfo(inputFilePath).Length;

        Console.WriteLine($"Starting file sort at {DateTime.Now} with target size: {FileSizeHelper.Format(sizeInBytes)}");
        Console.WriteLine($"Input file: {inputFilePath}");
        Console.WriteLine($"Output file: {outputFilePath}");
        Console.WriteLine($"Chunk size: {FileSizeHelper.Format(chunkSize.Value)}");

        var stopwatch = Stopwatch.StartNew();

        // Create a temporary folder for chunk files
        Directory.CreateDirectory(_tempFolder);

        // Split the input file into sorted chunks
        var chunkFiles = SplitAndSortChunks(inputFilePath, _tempFolder, chunkSize.Value);

        // Merge sorted chunks into the output file
        MergeChunks(chunkFiles, outputFilePath, sizeInBytes);

        // Clean up temporary files
        Directory.Delete(_tempFolder, true);

        Console.WriteLine($"File sort completed at {DateTime.Now}");
        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
    }

    // Split the input file into sorted chunks and save them to temporary files
    static List<string> SplitAndSortChunks(string inputFile, string tempFolder, long maxChunkSize)
    {
        var chunkFiles = new List<string>(); // Store paths to the sorted chunk files
        var lines = new List<LineData>();    // Buffer for storing lines before sorting
        var currentChunkSize = 0L;           // Track the size of the current chunk
        var chunkCount = 0;                  // Count the number of chunks created

        var stopwatchTotal = Stopwatch.StartNew();
        var stopwatchChunk = Stopwatch.StartNew();

        var sortingTasks = new List<Task>(); // List of tasks for parallel sorting

        using (var reader = new StreamReader(inputFile))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Add each line to the buffer and update the chunk size
                lines.Add(new LineData(line));
                currentChunkSize += Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;

                // If the chunk size exceeds the max, sort and save the chunk
                if (currentChunkSize < maxChunkSize)
                    continue;

                chunkCount++;
                var chunkFile = Path.Combine(tempFolder, $"chunk_{chunkCount}.txt");
                chunkFiles.Add(chunkFile);

                // Reset for next chunk
                var linesToSort = lines.ToList();
                lines.Clear();
                currentChunkSize = 0;

                Console.WriteLine($"Chunk {chunkCount} ready for sorting: {chunkFile} ({stopwatchChunk.Elapsed})");
                stopwatchChunk.Restart();

                // Sort and save the chunk in a separate task  
                sortingTasks.Add(Task.Run(() => SortAndSaveChunk(linesToSort, chunkFile)));
            }

            // Handle the last chunk if there are remaining lines
            if (lines.Count > 0)
            {
                chunkCount++;
                string chunkFile = Path.Combine(tempFolder, $"chunk_{chunkCount}.txt");
                chunkFiles.Add(chunkFile);

                Console.WriteLine($"Last chunk ready for sorting: {chunkFile}");

                // Sort and save the last chunk in the main thread
                SortAndSaveChunk(lines, chunkFile);
            }
        }

        // Wait for all sorting tasks to complete
        Task.WaitAll([.. sortingTasks]);

        Console.WriteLine($"Split and sort completed in {stopwatchTotal.Elapsed}");

        return chunkFiles;
    }

    // Sort a chunk of lines and save to a temporary file
    static void SortAndSaveChunk(List<LineData> lines, string chunkFile)
    {
        var stopwatch = Stopwatch.StartNew();

        // Sort lines using the LineComparer
        lines.Sort(new LineComparer());

        // Write sorted lines to the chunk file
        using var writer = new StreamWriter(chunkFile, false, Encoding.UTF8, _bufferSizeSortingPhase);
        foreach (var line in lines)
            writer.WriteLine(line);

        Console.WriteLine($"Chunk sorted and saved in {stopwatch.Elapsed}: {chunkFile}");
    }

    // Merge sorted chunk files into a single output file
    static void MergeChunks(List<string> chunkFiles, string outputFile, long targetSizeInBytes)
    {
        Console.WriteLine($"Merging {chunkFiles.Count} sorted chunks into the output file: {outputFile}");

        var stopwatch = Stopwatch.StartNew();

        var totalBytesWritten = 0L; // Track total bytes written to the output file
        var lastReportedSize = 0L;  // For progress reporting

        var readers = new List<StreamReader>(); // List of readers for each chunk
        try
        {
            // Initialize stream readers for each chunk
            foreach (var chunkFile in chunkFiles)
                readers.Add(new StreamReader(chunkFile));

            using var writer = new StreamWriter(outputFile, false, Encoding.UTF8, _bufferSizeMergingPhase);
            // Min-heap to store the smallest line from each chunk
            var heap = new HeapMin<LineData, int>(new HeapOptions<LineData>
            {
                Capacity = chunkFiles.Count,
                Comparer = new LineComparer()
            });
            var currentLines = new string[readers.Count];

            // Read the first line from each reader
            for (int i = 0; i < readers.Count; i++)
            {
                if (readers[i].EndOfStream)
                    continue;

                currentLines[i] = readers[i].ReadLine();

                heap.Insert(new LineData(currentLines[i]), i);
            }

            // Merge the smallest lines from the heap
            while (!heap.Empty)
            {
                var minEntry = heap.ExtractNode();
                var minLine = minEntry.Key;
                var readerIndex = minEntry.Value;

                // Write the smallest line to the output file
                var line = minLine.ToString();
                writer.WriteLine(line);

                totalBytesWritten += Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;

                // Report progress
                if (totalBytesWritten - lastReportedSize >= _defaultReportInterval)
                {
                    lastReportedSize = totalBytesWritten;
                    var percentage = (double)totalBytesWritten / targetSizeInBytes * 100;

                    Console.WriteLine($"Progress: {percentage:F2}% ({FileSizeHelper.Format(totalBytesWritten)} of {FileSizeHelper.Format(targetSizeInBytes)})");
                }

                // Read the next line from the chunk that just had the smallest line
                if (!readers[readerIndex].EndOfStream)
                {
                    currentLines[readerIndex] = readers[readerIndex].ReadLine();

                    heap.Insert(new LineData(currentLines[readerIndex]), readerIndex);
                }
            }

            Console.WriteLine($"Chunks merged in {stopwatch.Elapsed}");
        }
        finally
        {
            // Close all readers
            foreach (var reader in readers)
                reader.Dispose();
        }
    }
}
