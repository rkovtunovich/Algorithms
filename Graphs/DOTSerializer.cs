﻿using Graphs.Abstraction;
using System.Drawing;
using System.Text;

namespace Graphs;
public class DOTSerializer : ISerializer
{
    private readonly Graph _graph;

    private readonly HashSet<Vertice> _importantVetices = new();

    private readonly HashSet<Vertice> _importantEdges = new();

    public Color ImportantVericeColor { get; set; } = Color.Green;

    public Color ImportantEdgeColor { get; set; } = Color.Green;

    public DOTSerializer(Graph graph)
    {
        _graph = graph;
    }

    public void AddImportantVertice(Vertice verice)
    {
        _importantVetices.Add(verice);
    }

    public void AddImportantEdges(Vertice verice)
    {
        _importantEdges.Add(verice);
    }

    public void AddImportantEdges(HashSet<Vertice> verices)
    {
        foreach (Vertice verice in verices)
            _importantEdges.Add(verice);
    }

    public string Seralize()
    {
        var builder = new StringBuilder();

        foreach (var verice in _graph)
        {
            builder.AppendLine($"\t{verice} {GetEdgeLine(_graph)} {{");

            foreach (var connection in _graph.GetEdges(verice))
            {
                builder.AppendLine($"\t\t{connection}");
            }

            builder.AppendLine($"\t}} {AddImportantEdgesFormat(verice)}");
        }

        var vertices = builder.ToString();

        var dot = $"{GetTypeOfGraph(_graph)} {_graph.Name} {{ \n" +
            $"{vertices}" +
            $"{AddImportantVetricesFormatting()}" +
            $"}}";

        return dot;
    }

    private string AddImportantVetricesFormatting()
    {
        if (_importantVetices.Count == 0)
            return "";

        string format = $"{string.Join(',', _importantVetices)} [color = {ImportantVericeColor.Name}, style = bold]";

        return format;
    }
    
    private string AddImportantEdgesFormat(Vertice vertice)
    {
        if (_importantEdges.Count == 0)
            return "";

        if (_importantEdges.Contains(vertice))
            return $"[color = {ImportantEdgeColor.Name}, style = bold]";
        else
            return "";
    }

    private static string GetTypeOfGraph(Graph _graph)
    {
        return _graph.IsOriented() ? "digraph" : "strict graph";
    }

    private static string GetEdgeLine(Graph _graph)
    {
        return _graph.IsOriented() ? "->" : "--";
    }

    public void SaveToFile(string fileName, string dotString)
    {
        using var streamWriter = new StreamWriter(fileName);
        streamWriter.Write(dotString);
        streamWriter.Close();
    }
}
