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

    public static void VisualizeGraph(Graph graph) {

        var dotSerializer = new DOTSerializer(graph);
        var dotString = dotSerializer.Seralize();

        var dotFileName = $"{_workingDirectory}\\{graph.Name}.txt";
        dotSerializer.SaveToFile(dotFileName, dotString);

        VisualizeDotString(dotFileName, $"{graph.Name}.svg");
    }
}
