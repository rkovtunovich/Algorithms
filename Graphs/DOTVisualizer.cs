using Graphs.Abstraction;
using System.Diagnostics;

namespace Graphs;
public static class DOTVisualizer
{
    private static readonly string _workingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    public static void VisualizeDotString(string inputFile, string outputFileName)
    {
        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Windows\\system32\\cmd.exe",
                WorkingDirectory = _workingDirectory,
                Arguments = $"/c dot -Tsvg {inputFile} > {outputFileName}"
            }
        };
        p.Start();
        p.WaitForExit();
    }

    public static void VisualizeGraph(Graph graph, Vertex? importantVertex = null, HashSet<Vertex>? importantEdges = null) {

        var dotSerializer = new DOTSerializer();
        dotSerializer.AddImportantEdges(importantEdges);
        dotSerializer.AddImportantVertex(importantVertex);
        var dotString = dotSerializer.Serialize(graph);

        var dotFileName = $"{_workingDirectory}\\serializations\\{graph.Name}.txt";
        SaveToFile(dotFileName, dotString);

        var svgFileName = $"{_workingDirectory}\\visualizations\\{graph.Name}.svg";
        VisualizeDotString(dotFileName, svgFileName);
    }

    public static void SaveToFile(string fileName, string dotString)
    {
        using var streamWriter = new StreamWriter(fileName);
        streamWriter.Write(dotString);
        streamWriter.Close();
    }

    public static string ReadFromFile(string fileName)
    {
        var dotFileName = $"{_workingDirectory}\\serializations\\{fileName}.txt";

        using var streamReader = new StreamReader(dotFileName);
        var dotString = streamReader.ReadToEnd();
        streamReader.Close();

        return dotString;
    }
}
