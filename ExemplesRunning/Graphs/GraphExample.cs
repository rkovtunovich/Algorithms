using Graphics;
using Graphs;
using Helpers;
using OpenTK.Windowing.Desktop;
using Svg;
using System.Diagnostics;
using System.Drawing;
using View;

namespace ExemplesRunning.Graphs;
internal class GraphExample
{
    private static readonly string _workingDirectory = @"C:\repos\learning\Algo\ExemplesRunning\Graphs\files";

    internal static void Run()
    {
        var graph = GraphHelper.GenerateNonOriented(20);

        var origin = graph.First();
        var connected = BFS<int>.SearchConnected(graph, origin);

        var deegreDistributionsCount = graph.GetDedreeDistributionsCount();
        Viewer.ShowArray(deegreDistributionsCount);

        var deegreDistributionsFraction = graph.GetDedreeDistributionsFraction();
        Viewer.ShowArray(deegreDistributionsFraction);

        var deegreDistributionsCumulative = graph.GetDegreeDistributionsCumulative();
        Viewer.ShowArray(deegreDistributionsCumulative);

        var dotSerializer = new DOTSerializer<int>(graph);
        dotSerializer.AddImportantVertice(origin);
        dotSerializer.AddImportantEdges(connected);
        var dotString = dotSerializer.Seralize();

        var dotFileName = $"{_workingDirectory}\\dot.txt";

        using var streamWriter = new StreamWriter(dotFileName);
        streamWriter.Write(dotString);
        streamWriter.Close();

        string outputFileName = "output.svg";

        Process p = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                FileName = "C:\\Windows\\system32\\cmd.exe",
                WorkingDirectory = _workingDirectory,
                Arguments = @"/c dot -Tsvg dot.txt > " + outputFileName
            }
        };
        p.Start();
        p.WaitForExit();

        Console.ReadKey();
    }
}
