namespace Graphs.Coloring;

// The main idea behind color-coding is to randomly assign colors to the vertices of the graph,
// then look for colorful subgraphs that have vertices with distinct colors.
// By doing this, the algorithm reduces the problem of finding a subgraph isomorphism to a simpler problem of finding a colorful subgraph.
//
// The algorithm has a randomized aspect because the coloring of vertices is done randomly.
// While it does not guarantee finding the desired subgraph in a single iteration, the probability of finding it increases with the number of iterations.
// Probability that random coloring be a panchromatic one equals k!/k^k
//
// Color-coding can be used to solve various problems, such as finding simple paths of length k, finding cycles of length k,
// and detecting the presence of specific subgraph structures within a larger graph.
// In many cases, color-coding can provide a significant improvement in time complexity compared to naïve brute-force approaches,
// especially when combined with dynamic programming techniques.
//
// 1. Randomly color the vertices of the input graph using k distinct colors, where k is the desired size of the subgraph to be found (e.g., the length of the path or cycle).
// 2. Search for colorful subgraphs, i.e., subgraphs where all vertices have distinct colors.
// 3. Repeat steps 1 and 2 multiple times to increase the probability of finding the desired subgraph.
//
// size is k in the code

public static class ColorCoding
{
    private static readonly Random _random = new();

    public static double FindMinimumLengthColorfulPath(Graph graph, int size, double probabilityOfFailure)
    {
        double bestPath = double.MaxValue;
        int numberOfAttempts = (int)(Math.Pow(Math.E, size) / (Math.Sqrt(2 * Math.PI * size)) * Math.Log(1 / probabilityOfFailure));

        for (int i = 0; i < numberOfAttempts; i++)
        {
            var vertexColors = PaintRandomly(graph, size);

            DOTVisualizer.VisualizeGraph(graph);

            var currentBestPath = GetMinPanchromaticPath(graph, vertexColors, size);

            bestPath = Math.Min(bestPath, currentBestPath);
        }

        return bestPath is double.MaxValue ? -1 : bestPath;
    }

    private static Dictionary<Vertex, int> PaintRandomly(Graph graph, int size)
    {
        var vertexColors = new Dictionary<Vertex, int>();

        foreach (var vertex in graph)
        {
            int color = _random.Next(0, size );
            vertexColors[vertex] = color;
            vertex.Label = color.ToString();
        }

        return vertexColors;
    }

    private static double GetMinPanchromaticPath(Graph graph, Dictionary<Vertex, int> vertexColors, int size)
    {
        var results = new Dictionary<int, double>[graph.Count, 1 << size];

        for (int i = 0; i < graph.Count; i++)
        {
            for (int j = 0; j < (1 << size); j++)
            {
                results[i, j] = new Dictionary<int, double>();
            }
        }

        double minLength = double.MaxValue;

        foreach (var vertex in graph)
        {
            minLength = Math.Min(minLength, MinPathLength(graph, results, vertexColors, vertex, (1 << size) - 1));
        }

        return minLength;

    }

    private static double MinPathLength(Graph graph, Dictionary<int, double>[,] results, Dictionary<Vertex, int> vertexColors, Vertex vertex, int mask)
    {
        var vertexColor = vertexColors[vertex];
        var vertexResult = results[vertex.Index - 1, mask];

        if (vertexResult.ContainsKey(vertexColor))
            return vertexResult[vertexColor];

        // variable is an integer bitmask, where each bit position corresponds to a color.
        // If the bit at position i is set (equal to 1), it means that color i is still needed in the remaining path; otherwise,
        // if the bit is unset (equal to 0), it means that color i has already been included in the path or is not needed.
        int remainingColors = mask & ~(1 << vertexColor );
        if (remainingColors == 0)
        {
            vertexResult[vertexColor] = 0;
            return 0;
        }

        double minLength = double.MaxValue;
        foreach (var edge in graph.GetEdges(vertex))
        {
            double length = graph.GetEdgeLength(vertex, edge);
            // if the color of the edge is in the remaining colors
            if ((mask & (1 << vertexColors[edge])) != 0)
            {
                double subPathLength = MinPathLength(graph, results, vertexColors, edge, remainingColors);
                if (subPathLength != double.MaxValue)
                    minLength = Math.Min(minLength, subPathLength + length);
            }
        }

        vertexResult[vertexColor] = minLength;

        return minLength;
    }
}
