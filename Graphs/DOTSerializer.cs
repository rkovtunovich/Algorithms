using Graphs.Abstraction;
using System.Drawing;
using System.Text;

namespace Graphs;
public class DOTSerializer<T> : ISerializer<T>
{
    private readonly Graph<T> _graph;

    private readonly HashSet<Vertice<T>> _importantVetices = new();
    private readonly HashSet<Vertice<T>> _importantEdges = new();

    public Color ImportantVericeColor { get; set; } = Color.Green;
    public Color ImportantEdgeColor { get; set; } = Color.Green;

    public DOTSerializer(Graph<T> graph)
    {
        _graph = graph;
    }

    public void AddImportantVertice(Vertice<T> verice)
    {
        _importantVetices.Add(verice);
    }

    public void AddImportantEdges(Vertice<T> verice)
    {
        _importantEdges.Add(verice);
    }

    public void AddImportantEdges(HashSet<Vertice<T>> verices)
    {
        foreach (Vertice<T> verice in verices)
            _importantEdges.Add(verice);
    }

    public string Seralize()
    {
        var builder = new StringBuilder();

        foreach (var verice in _graph)
        {
            builder.AppendLine($"\t{verice} -- {{");

            foreach (var connection in _graph.GetEdges(verice))
            {
                builder.AppendLine($"\t\t{connection}");
            }

            builder.AppendLine($"\t}} {AddImportantEdgesFormat(verice)}");
        }

        var vertices = builder.ToString();

        var dot = $"strict graph {_graph.Name} {{ \n" +
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
    
    private string AddImportantEdgesFormat(Vertice<T> vertice)
    {
        if (_importantEdges.Count == 0)
            return "";

        if (_importantEdges.Contains(vertice))
            return $"[color = {ImportantEdgeColor.Name}, style = bold]";
        else
            return "";
    }
}
