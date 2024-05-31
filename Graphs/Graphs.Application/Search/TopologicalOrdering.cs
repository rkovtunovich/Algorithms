using DataStructures.Lists;
using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;

namespace Graphs.Application.Search;

// Topological ordering, or topological sorting,
// is an algorithm used to linearly order the vertices of a directed acyclic graph (DAG) in such a way that for every directed edge (u, v) from vertex u to vertex v,
// vertex u comes before vertex v in the ordering.
// A topological ordering is only possible for DAGs, as the presence of a cycle would make a linear order impossible.
// 
// Topological ordering has various applications, such as scheduling tasks with dependencies,
// determining the compilation order of a set of modules with dependencies,
// and finding a valid sequence of courses to take in a curriculum with prerequisites.
// 
// There are several algorithms to perform topological sorting,
// but one of the most common and straightforward methods is based on depth-first search (DFS).
// Here's a step-by-step description of the DFS-based topological sorting algorithm:
// 
// 1. Create an empty stack to store the sorted vertices.
// 2. Initialize a set or a list of visited nodes.
// 3. For each unvisited node in the graph, perform a modified DFS:
//      a. Visit the node and mark it as visited.
//      b. Recursively visit all neighbors of the node that haven't been visited yet.
//      c. Once all neighbors of the node have been visited, push the node onto the stack.
// 4. After visiting all nodes in the graph, the stack contains the topological order of the vertices.
//    Pop the vertices from the stack one by one to obtain the topological ordering.
public static class TopologicalOrdering
{
    // This method returns a topological ordering of the vertices in a directed acyclic graph (DAG).
    public static Vertex[] SortTopologically(GraphBase graph)
    {
        int curLabel = graph.Count();

        var vertices = new Vertex[curLabel];

        var visited = new HashSet<Vertex>();

        foreach (var item in graph)
        {
            if (visited.Contains(item))
                continue;

            TopoSort(graph, item, visited, ref curLabel, vertices);
        }

        return vertices;
    }

    private static void TopoSort(GraphBase graph, Vertex current, HashSet<Vertex> visited, ref int curLabel, Vertex[] vertices)
    {
        visited.Add(current);

        var edges = graph.GetAdjacentEdges(current);

        foreach (var edge in edges)
        {
            if (!visited.Contains(edge))
                TopoSort(graph, edge, visited, ref curLabel, vertices);
        }

        current.Distance = curLabel--;
        vertices[curLabel] = current;
    }

    public static (bool isCycle, SequentialList<Vertex> vertices) TrySortTopologicallyOrGetCycle(GraphBase graph)
    {
        int curLabel = graph.Count();

        var vertices = new Vertex[curLabel];

        var visited = new HashSet<Vertex>();
        var cycle = new HashSet<Vertex>();
        var cycleFound = false;

        foreach (var item in graph)
        {
            if (visited.Contains(item))
                continue;

            TopoSortOrCycle(graph, item, visited, ref curLabel, vertices, cycle, ref cycleFound);

            if (cycleFound)
                break;
        }

        if (cycleFound)
            return (true, cycle.ToSequentialList());

        return (false, vertices.ToSequentialList());
    }

    private static Vertex? GetVertexWithoutIncomingEdges(GraphBase graph)
    {
        var hasIncomingEdges = new bool[graph.Count];

        foreach (var vertex in graph)
        {
            var edges = graph.GetAdjacentEdges(vertex);

            foreach (var edge in edges)
            {
                hasIncomingEdges[edge.Index - 1] = true;
            }
        }

        for (int i = 1; i <= hasIncomingEdges.Length; i++)
        {
            if (!hasIncomingEdges[i - 1])
                return graph[i];
        }

        return null;
    }

    private static void TopoSortOrCycle(GraphBase graph, Vertex current, HashSet<Vertex> visited, ref int curLabel, Vertex[] vertices, HashSet<Vertex> cycle, ref bool cycleFound)
    {
        visited.Add(current);
        cycle.Add(current);

        var edges = graph.GetAdjacentEdges(current);

        foreach (var edge in edges)
        {
            // TO DO : better way is to find the index in the cycle, because the cycle may not close on the first element
            // and return the cycle from that index to the end
            if (cycle.Contains(edge))
            {
                cycleFound = true;
                return;
            }

            if (!visited.Contains(edge))
                TopoSortOrCycle(graph, edge, visited, ref curLabel, vertices, cycle, ref cycleFound);

            if (cycleFound)
                return;
        }

        current.Distance = curLabel--;
        vertices[curLabel] = current;

        cycle.Remove(current);
    }
}