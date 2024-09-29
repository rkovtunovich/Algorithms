using DataStructures.Heaps;
using System.Diagnostics;

namespace TextProcessing.Sorting.External;

public class TextSorter
{
    private static readonly string _defaultInputFilePath = "source.txt";
    private static readonly string _defaultOutputFilePath = "sorted.txt";
    private static readonly string _tempFolder = "temp";
    private static readonly long _defaultChunkSize = 50L * 1024 * 1024; // 50MB
    private static readonly int _defaultBufferSize = 64 * 1024; // 64KB

    public static void SortFile(string? inputFilePath, string? outputFilePath)
    {
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

        Console.WriteLine($"Starting file sort at {DateTime.Now}");
        Console.WriteLine($"Input file: {inputFilePath}");
        Console.WriteLine($"Output file: {outputFilePath}");

        var stopwatch = Stopwatch.StartNew();

        // Create a temporary folder for chunk files
        Directory.CreateDirectory(_tempFolder);

        // Split the input file into sorted chunks
        var chunkFiles = SplitAndSortChunks(inputFilePath, _tempFolder, _defaultChunkSize);

        // Merge sorted chunks into the output file
        MergeChunks(chunkFiles, outputFilePath);

        // Clean up temporary files
        Directory.Delete(_tempFolder, true);

        Console.WriteLine($"File sort completed at {DateTime.Now}");
        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
    }

    static List<string> SplitAndSortChunks(string inputFile, string tempFolder, long maxChunkSize)
    {
        var chunkFiles = new List<string>();
        var lines = new List<LineData>();
        long currentChunkSize = 0;
        int chunkCount = 0;

        var stopwatch = Stopwatch.StartNew();

        var sortingTasks = new List<Task>();

        using (var reader = new StreamReader(inputFile))
        {
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                lines.Add(new LineData(line));
                currentChunkSize += Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;

                if (currentChunkSize < maxChunkSize)               
                    continue;
                
                chunkCount++;
                string chunkFile = Path.Combine(tempFolder, $"chunk_{chunkCount}.txt");
                chunkFiles.Add(chunkFile);

                // Reset for next chunk
                var linesToSort = lines.ToList();
                lines.Clear();
                currentChunkSize = 0;

                Console.WriteLine($"Chunk {chunkCount} ready for sorting: {chunkFile}");

                // Sort and save the chunk in a separate task  
                sortingTasks.Add(Task.Run(() => SortAndSaveChunk(linesToSort, chunkFile)));
            }

            // Handle the last chunk
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

        Console.WriteLine($"Split and sort completed in {stopwatch.Elapsed}");

        return chunkFiles;
    }

    static void SortAndSaveChunk(List<LineData> lines, string chunkFile)
    {
        var stopwatch = Stopwatch.StartNew();

        lines.Sort(new LineComparer());

        using var writer = new StreamWriter(chunkFile, false, Encoding.UTF8, _defaultBufferSize);
        foreach (var line in lines)
            writer.WriteLine(line);

        Console.WriteLine($"Chunk sorted and saved in {stopwatch.Elapsed}: {chunkFile}");
    }

    static void MergeChunks(List<string> chunkFiles, string outputFile)
    {
        Console.WriteLine($"Merging {chunkFiles.Count} sorted chunks into the output file: {outputFile}");

        var stopwatch = Stopwatch.StartNew();

        var readers = new List<StreamReader>();
        try
        {
            // Initialize stream readers for each chunk
            foreach (var chunkFile in chunkFiles)
                readers.Add(new StreamReader(chunkFile));

            using var writer = new StreamWriter(outputFile, false, Encoding.UTF8, _defaultBufferSize);
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

            while (!heap.Empty)
            {
                var minEntry = heap.ExtractNode();
                var minLine = minEntry.Key;
                var readerIndex = minEntry.Value;

                // Write the smallest line
                writer.WriteLine(minLine);

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
