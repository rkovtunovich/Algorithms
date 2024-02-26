using Graphs.Core.GraphImplementation;
using System.Drawing;
using System.Text;

namespace Graphs.Core;

public class DOTSerializer : ISerializer
{
    private readonly HashSet<Vertex> _importantVertices = new();

    private readonly HashSet<Vertex> _importantEdges = new();

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

    public string Serialize(Graph graph)
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

    private static string GetTypeOfGraph(Graph _graph)
    {
        return _graph.IsOriented() ? "digraph" : "strict graph";
    }

    private static string GetEdgeLine(Graph _graph)
    {
        return _graph.IsOriented() ? "->" : "--";
    }

    #endregion

    #region Deserialization

    public Graph Deserialize(string serializedGraph)
    {
        if (IsOriented(serializedGraph))
            return DeserializeOriented(serializedGraph);
        else
            return DeserializeNonOriented(serializedGraph);
    }

    private bool IsOriented(string serializedGraph)
    {
        return serializedGraph.StartsWith("digraph");
    }

    private Graph DeserializeOriented(string serializedGraph)
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

    private Graph DeserializeNonOriented(string serializedGraph)
    {
        var graph = new UndirectedGraph(GetGraphName(serializedGraph));

        var lines = serializedGraph.Split('\n');

        foreach (var line in lines)
        {
            if (line.Contains("--"))
            {
                var vertices = line.Split("--");

                var vertex1 = vertices[0].Trim();
                var vertex2 = vertices[1].Trim();

                // TO DO
            }
        }

        return graph;
    }

    private string GetGraphName(string serializedGraph)
    {
        var lines = serializedGraph.Split('\n');

        foreach (var line in lines)
        {
            if (line.Contains("graph"))
            {
                var graphName = line.Split(' ')[1].Trim();

                return graphName;
            }
        }

        return "";
    }

    #endregion
}
