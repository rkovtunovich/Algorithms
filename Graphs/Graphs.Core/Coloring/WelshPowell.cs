using Graphs.Core.Abstraction;
using Graphs.Core.Model;

namespace Graphs.Core.Coloring;

// The Welsh-Powell algorithm is a graph coloring algorithm used to color the vertices of a graph with the minimum number of colors possible.
// It's a greedy algorithm, meaning it makes the best choice at each step, and it works for undirected graphs. The algorithm proceeds as follows:

// 1. Find the degree of each vertex
// 2. List the vertices in order of descending degrees
// 3. Color the first vertex with color 1
// 4. Move down the list and color all the vertices not connected to the colored vertex, with the same color

public static class WelshPowell
{
    public static void Colorize(Graph graph)
    {
        var sortedVertices = graph.OrderBy(v => -graph.GetDegree(v)).ToList();
        var coloredVertices = new Dictionary<Vertex, int>();

        int color = 1;
        foreach (var vertex in sortedVertices)
        {
            if (coloredVertices.ContainsKey(vertex))
                continue;

            coloredVertices[vertex] = color;
            vertex.Label = $"color {color}";
            foreach (var other in sortedVertices)
            {
                if (!coloredVertices.ContainsKey(other) && !IsConnectedToColorized(graph, coloredVertices, other, color))
                {
                    coloredVertices[other] = color;
                    other.Label = $"color {color}";
                }
            }

            color++;
        }
    }

    private static bool IsConnectedToColorized(Graph graph, Dictionary<Vertex, int> coloredVertices, Vertex vertex, int currentColor)
    {
        var adjacent = graph.GetAdjacentEdges(vertex);

        foreach (var edge in adjacent)
        {
            if (coloredVertices.TryGetValue(edge, out int value) && value == currentColor)
                return true;
        }

        return false;
    }
}
