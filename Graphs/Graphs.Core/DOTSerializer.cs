using Graphs.Core.Model.Graphs;
using Graphs.Core.Model.Serialization;
using System.Drawing;
using System.Text;

namespace Graphs.Core;

public class DOTSerializer : ISerializer
{
    private readonly HashSet<Vertex> _importantVertices = [];

    private readonly HashSet<Vertex> _importantEdges = [];

    public Color ImportantVertexColor { get; set; } = Color.Green;

    public Color ImportantEdgeColor { get; set; } = Color.Green;

    public void AddImportantVertex(Vertex? vertex)
    {
        if (vertex is not null)
            _importantVertices.Add(vertex);
    }

    public void AddImportantEdges(HashSet<Vertex>? vertices)
    {
        if (vertices is null)
            return;

        foreach (Vertex vertex in vertices)
            _importantEdges.Add(vertex);
    }

    #region Serialization

    public string Serialize(GraphBase graph)
    {
        var builder = new StringBuilder();

        foreach (var vertex in graph)
        {
            if (graph.IsVariableEdgeLength())
            {
                var edges = graph.GetAdjacentEdges(vertex);

                foreach (var edge in edges)
                {
                    builder.Append($"\t{vertex} {GetEdgeLine(graph)} ");
                    string line = $"\t\t{edge}";

                    if (graph.IsVariableEdgeLength())
                        line += $" [label = \"{graph.GetEdgeLength(vertex, edge):0.00}\"];";

                    builder.Append(line);
                }

                if (edges.Count == 0)
                    builder.Append($"\t{vertex} ");

                builder.Append($"\t {AddImportantEdgesFormat(vertex)}");
                builder.Append("\n");
            }
            else
            {
                builder.AppendLine($"\t{vertex} {GetEdgeLine(graph)} {{");

                foreach (var edge in graph.GetAdjacentEdges(vertex))
                {
                    string line = $"\t\t{edge}";

                    if (graph.IsVariableEdgeLength())
                        line += $" [label = \"{graph.GetEdgeLength(vertex, edge)}\"];";

                    builder.AppendLine(line);
                }

                builder.AppendLine($"\t}} {AddImportantEdgesFormat(vertex)}");
            }
        }

        var vertices = builder.ToString();

        var dot = $"{GetTypeOfGraph(graph)} {graph.Name} {{ \n" +
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

    private static string GetTypeOfGraph(GraphBase _graph)
    {
        return _graph.IsOriented() ? "digraph" : "strict graph";
    }

    private static string GetEdgeLine(GraphBase _graph)
    {
        return _graph.IsOriented() ? "->" : "--";
    }

    #endregion

    #region Deserialization

    public GraphBase Deserialize(string serializedGraph, DeserializationOptions? options = null)
    {
        if (IsOriented(serializedGraph))
            return DeserializeOriented(serializedGraph);
        else
            return DeserializeNonOriented(serializedGraph, options);
    }

    private bool IsOriented(string serializedGraph)
    {
        return serializedGraph.StartsWith("digraph");
    }

    private GraphBase DeserializeOriented(string serializedGraph)
    {
        string[] lines = serializedGraph.Split('\n');
        string graphName = lines[0].Split(' ')[1];
        var graph = new OrientedGraph(graphName.Trim() + "_new");

        HashSet<Vertex> uniqueVertices = [];
        Dictionary<Vertex, List<string>> nonParsedEdges = [];

        // First pass to add vertices
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string line = lines[i].Trim().Replace("\"", string.Empty);

            string[] parts = line.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                continue;

            var firstPart = parts[0];
            Vertex vertex = GetVertex(firstPart);

            uniqueVertices.Add(vertex);
            graph.AddVertex(vertex);

            var edges = new List<string>();

            foreach (var part in parts)
            {
                var edge = part.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);

                // there are no edges
                if (edge.Length == 1)
                    continue;

                edges.Add(edge[1].Trim());
            }

            nonParsedEdges.Add(vertex, edges);
        }

        // Second pass to add edges
        foreach (var edge in nonParsedEdges)
        {
            var vertex = edge.Key;
            var edgeParts = edge.Value;

            for (var i = 0; i < edgeParts.Count; i++)
            {
                var edgeDescription = edgeParts[i].Trim(']').Split('[');

                var edgeVertex = GetVertex(edgeDescription[0].Trim());
                graph.AddEdge(vertex, edgeVertex);

                if (edgeDescription.Length == 1)
                    continue;

                var edgeLength = edgeDescription[1].Trim().Split('=')[1].Trim();
                graph.SetEdgeLength(vertex, edgeVertex, double.Parse(edgeLength));

            }
        }

        return graph;
    }

    private static Vertex GetVertex(string firstPart)
    {
        Vertex vertex;

        if (!firstPart.Contains("->"))
        {
            var vertexIndex = int.Parse(firstPart);
            vertex = new Vertex(vertexIndex);
        }
        else
        {
            string[] edgeParts = firstPart.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
            var vertexIndex = int.Parse(edgeParts[0].Trim());
            vertex = new Vertex(vertexIndex);
        }

        return vertex;
    }

    private GraphBase DeserializeNonOriented(string serializedGraph, DeserializationOptions? options = null)
    {
        if (options is not null && options.IsVariableLength)
            return DeserializeNonOrientedVariableLength(serializedGraph);
        else
            return DeserializeNonOrientedFixedLength(serializedGraph);
    }

    private GraphBase DeserializeNonOrientedFixedLength(string serializedGraph)
    {
        var graph = new UndirectedGraph(GetGraphName(serializedGraph));

        var lines = serializedGraph.Split('\n');

        foreach (var line in lines)
        {
            if (!line.Contains("--"))
                continue;

            // TO DO
            var vertices = line.Split("--");

            var vertex1 = vertices[0].Trim();
            var vertex2 = vertices[1].Trim();
        }

        return graph;
    }

    private GraphBase DeserializeNonOrientedVariableLength(string serializedGraph)
    {
        var graph = new UndirectedVariableEdgeLengthGraph(GetGraphName(serializedGraph));

        var lines = serializedGraph.Split('\n');

        foreach (var line in lines)
        {
            if (!line.Contains("--"))
                continue;

            var edges = line.Split(";", StringSplitOptions.RemoveEmptyEntries);

            // edge is in the format: "1" -- "2" [label = "2,00"]

            foreach (var edge in edges)
            {
                if (!edge.Contains("--"))
                    continue;

                var edgeParts = edge.Split("--");

                var vertex1 = edgeParts[0].Replace("\"", string.Empty).Trim();
                var secondPart = edgeParts[1].Split('[');
                var vertex2 = secondPart[0].Replace("\"", string.Empty).Trim();

                // length is in the format: [label = "2,00"]
                var length = secondPart[1].Split('=')[1].Replace("\"", string.Empty).Trim(']');

                graph.AddEdge(new Vertex(int.Parse(vertex1)), new Vertex(int.Parse(vertex2)), double.Parse(length));
            }
        }

        return graph;
    }

    private string GetGraphName(string serializedGraph)
    {
        var lines = serializedGraph.Split('\n');
        var firstLine = lines[0];

        if (!firstLine.Contains("graph"))
            return "unnamed";

        var parts = firstLine.Split(' ');

        if (parts[0] == "digraph")
            return parts[1];
        if (parts[0] == "strict")
            return parts[2];

        return "unnamed";
    }

    #endregion
}
