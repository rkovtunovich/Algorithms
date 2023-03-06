using Graphs.Abstraction;
using Graphs.Model;
using System.Drawing;
using System.Text;

namespace Graphs;
public class DOTSerializer : ISerializer
{
    private readonly Graph _graph;

    private readonly HashSet<Vertex> _importantVertices = new();

    private readonly HashSet<Vertex> _importantEdges = new();

    public Color ImportantVertexColor { get; set; } = Color.Green;

    public Color ImportantEdgeColor { get; set; } = Color.Green;

    public DOTSerializer(Graph graph)
    {
        _graph = graph;
    }

    public void AddImportantVertex(Vertex vertex)
    {
        _importantVertices.Add(vertex);
    }

    public void AddImportantEdges(Vertex vertex)
    {
        _importantEdges.Add(vertex);
    }

    public void AddImportantEdges(HashSet<Vertex> vertices)
    {
        foreach (Vertex vertex in vertices)
            _importantEdges.Add(vertex);
    }

    #region Serialization

    public string Serialize()
    {
        var builder = new StringBuilder();

        foreach (var vertex in _graph)
        {
            if(_graph.IsVariableEdgeLength())
            {
                var edges = _graph.GetEdges(vertex);

                foreach (var edge in edges)
                {
                    builder.Append($"\t{vertex} {GetEdgeLine(_graph)} ");
                    string line = $"\t\t{edge}";

                    if (_graph.IsVariableEdgeLength())
                        line += $" [label = \"{_graph.GetEdgeLength(vertex, edge):0.00}\"];";

                    builder.Append(line);
                }

                if(edges.Count == 0)
                    builder.Append($"\t{vertex} ");

                builder.Append($"\t {AddImportantEdgesFormat(vertex)}");
                builder.Append("\n");
            }
            else
            {
                builder.AppendLine($"\t{vertex} {GetEdgeLine(_graph)} {{");

                foreach (var edge in _graph.GetEdges(vertex))
                {
                    string line = $"\t\t{edge}";

                    if (_graph.IsVariableEdgeLength())
                        line += $" [label = \"{_graph.GetEdgeLength(vertex, edge)}\"];";

                    builder.AppendLine(line);
                }

                builder.AppendLine($"\t}} {AddImportantEdgesFormat(vertex)}");
            }
        }

        var vertices = builder.ToString();

        var dot = $"{GetTypeOfGraph(_graph)} {_graph.Name} {{ \n" +
            $"{vertices}" +
            $"{AddImportantVerticesFormatting()}" +
            $"}}";

        return dot;
    }

    private string AddImportantVerticesFormatting()
    {
        if (_importantVertices.Count == 0)
            return "";

        string format = $"{string.Join(',', _importantVertices)} [color = {ImportantVertexColor.Name}, style = bold]";

        return format;
    }

    private string AddImportantEdgesFormat(Vertex vertex)
    {
        if (_importantEdges.Count == 0)
            return "";

        if (_importantEdges.Contains(vertex))
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

    #endregion

    public void SaveToFile(string fileName, string dotString)
    {
        using var streamWriter = new StreamWriter(fileName);
        streamWriter.Write(dotString);
        streamWriter.Close();
    }
}
