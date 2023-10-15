using Graphs.GraphImplementation;

namespace Graphs.Search;

// Here's a brief overview of the steps in the CPM algorithm:
// 
// 1. Topologically order the graph. If it fails, then exit.
//    This step ensures that the graph is a directed acyclic graph (DAG) and produces a linear ordering of the vertices
//    such that all directed edges point from earlier to later vertices in the order.
// 2. Following the topological order, search activity edges sequentially and calculate the earliest possible starting times of events.
//    This step calculates the earliest start times of each activity by considering the earliest completion time of their prerequisite activities.
// 3. Following the topological order, search activity edges reversely and calculate the latest possible starting times of events.
//    This step calculates the latest start times of each activity by considering the latest start times of the activities that depend on them without delaying the project.
// 4. Calculate the earliest possible starting times and latest possible starting times of various activities.
//    This step computes the earliest and latest start times for each activity based on the results obtained in steps 2 and 3.
// 5. Obtain the critical events and critical activities.
//    The critical path consists of the activities with the same earliest and latest start times,
//    meaning that they cannot be delayed without affecting the project's completion time.

public static class CriticalPathMethod
{
    public static void ExecuteCPM(OrientedGraph graph)
    {
        // Step 1: Topologically order the graph. If it fails then exit.
        var topologicalOrder = TopologicalOrdering.SortTopologically(graph);

        // Step 2: Following the topological order, search activity edges sequentially and calculate the earliest possible starting times of events.
        var earliestStartTimes = new double[graph.Count];
        foreach (var vertex in topologicalOrder)
        {
            var edges = graph.GetEdges(vertex);
            foreach (var edge in edges)
            {
                double edgeLength = graph.GetEdgeLength(vertex, edge);
                earliestStartTimes[edge.ArrayIndex()] = Math.Max(earliestStartTimes[edge.ArrayIndex()], earliestStartTimes[vertex.ArrayIndex()] + edgeLength);
            }
        }

        // Step 3: Following the topological order, search activity edges reversely and calculate the latest possible starting times of events.
        var latestStartTimes = new double[graph.Count];
        for (int i = graph.Count - 1; i >= 0; i--)
        {
            latestStartTimes[i] = double.MaxValue;
        }

        int lastIndex = topologicalOrder.Length - 1;
        latestStartTimes[topologicalOrder[lastIndex].ArrayIndex()] = earliestStartTimes[topologicalOrder[lastIndex].ArrayIndex()];

        for (int i = graph.Count - 1; i >= 0; i--)
        {
            var vertex = topologicalOrder[i];
            var edges = graph.GetEdges(vertex);
            foreach (var edge in edges)
            {
                double edgeLength = graph.GetEdgeLength(vertex, edge);
                latestStartTimes[vertex.ArrayIndex()] = Math.Min(latestStartTimes[vertex.ArrayIndex()], latestStartTimes[edge.ArrayIndex()] - edgeLength);
            }
        }

        // Step 4: Calculate the earliest possible starting times and latest possible starting times of various activities.
        // Step 5: Obtain the critical events and critical activities.
        var criticalPath = new LinkedList<Vertex>();
        for (int i = 0; i < graph.Count; i++)
        {
            if (earliestStartTimes[i] == latestStartTimes[i])
            {
                criticalPath.AddLast(topologicalOrder[i]);
            }
        }

        // Print results
        Console.WriteLine("Earliest Start Times:");
        for (int i = 0; i < graph.Count; i++)
        {
            Console.WriteLine($"Vertex {i + 1}: {earliestStartTimes[i]}");
        }

        Console.WriteLine("\nLatest Start Times:");
        for (int i = 0; i < graph.Count; i++)
        {
            Console.WriteLine($"Vertex {i + 1}: {latestStartTimes[i]}");
        }

        Console.WriteLine("\nCritical Path:");
        foreach (var vertex in criticalPath)
        {
            Console.Write($"{vertex.Index} -> ");
        }
    }
}
