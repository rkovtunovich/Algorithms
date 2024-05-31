using DataStructures.HashTables;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

public static class InfectionSpreading
{
    public static bool CheckInfectionSpreading(List<(int i, int j, int time)> connections, int a, int b, int startTime, int endTime, int population)
    {
        var graph = new UndirectedVariableEdgeLengthGraph("population");
        for (int i = 1; i <= population; i++)
        {
            graph.AddVertex(new(i));
        }

        foreach (var (i, j, t) in connections)
        {
            graph.AddEdge(new(i), new(j), t);
        }

        var visited = new SimpleHashSet<Vertex>();

        var queue = new Queue<Vertex>();
        queue.Enqueue(new(a));

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            visited.Add(current);

            if (current.Equals(new(b)))
                return true;

            var neighbors = graph.GetAdjacentEdges(current);

            foreach (var vertex in neighbors)
            {
                if (visited.Contains(vertex))
                    continue;

                var length = graph.GetEdgeLength(current, vertex);

                if (length < startTime || length > endTime)
                    continue;

                queue.Enqueue(vertex);
            }
        }

        return false;
    }
}
