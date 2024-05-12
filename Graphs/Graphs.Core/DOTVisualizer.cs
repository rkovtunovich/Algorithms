using Graphs.Core.Model.Graphs;
using System.Diagnostics;

namespace Graphs.Core;
public static class DOTVisualizer
{
    public static readonly string WorkingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    public static void VisualizeDotString(string inputFile, string outputFileName)
    {
        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Windows\\system32\\cmd.exe",
                WorkingDirectory = WorkingDirectory,
                Arguments = $"/c dot -Tsvg {inputFile} > {outputFileName}"
            }
        };
        p.Start();
        p.WaitForExit();
    }

    public static void VisualizeGraph(GraphBase graph, Vertex? importantVertex = null, HashSet<Vertex>? importantEdges = null)
    {

        var dotSerializer = new DOTSerializer();
        dotSerializer.AddImportantEdges(importantEdges);
        dotSerializer.AddImportantVertex(importantVertex);
        var dotString = dotSerializer.Serialize(graph);

        var dotFileName = $"{WorkingDirectory}\\serializations\\{graph.Name}.txt";
        GraphFileManager.SaveToFile(dotFileName, dotString);

        var svgFileName = $"{WorkingDirectory}\\visualizations\\{graph.Name}.svg";
        VisualizeDotString(dotFileName, svgFileName);
    }  
}