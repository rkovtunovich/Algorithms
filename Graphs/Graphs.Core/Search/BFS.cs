using Graphs.Core.Model.Graphs;

namespace Graphs.Core.Search;
public static class BFS
{
    // The SearchConnected function takes a Graph and a Vertex as input.
    public static HashSet<Vertex> SearchConnected(GraphBase graph, Vertex originVertex)
    {
        // Initialize a HashSet with the origin vertex to keep track of visited vertices.
        var visited = new HashSet<Vertex>
        {
            originVertex
        };

        // Initialize a queue with the origin vertex for breadth-first search.
        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertex);

        // Continue the search as long as there are vertices in the queue.
        while (queue.Count > 0)
        {
            // Dequeue a vertex for consideration.
            var current = queue.Dequeue();

            // Get a list of all vertices directly reachable from the current vertex.
            var edges = graph.GetAdjacentEdges(current);

            // Iterate over each vertex in the list of reachable vertices.
            foreach (var edge in edges)
            {
                // If the vertex has been visited before, skip this iteration.
                if (visited.Contains(edge))
                    continue;

                // If the vertex hasn't been visited before, mark it as visited and enqueue it for future consideration.
                visited.Add(edge);
                queue.Enqueue(edge);
            }
        }

        // Once all connected vertices have been visited, return the set of visited vertices.
        return visited;
    }

    public static HashSet<Vertex> MarkPaths(UndirectedGraph graph, Vertex originVertex)
    {
        int level = 0;

        originVertex.Distance = level;

        var visited = new HashSet<Vertex>
        {
            originVertex
        };

        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertex);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            level = (int)(current.Distance ?? 0) + 1;

            var edges = graph.GetAdjacentEdges(current);

            if (edges.Count == 0)
                continue;

            var edgeNode = edges.First;
            if (edgeNode is null)
                continue;

            while (edgeNode is not null)
            {
                var edge = edgeNode.Value;

                if (visited.Contains(edge))
                {
                    edgeNode = edgeNode.Next;
                    continue;
                }

                edge.Distance = level;

                visited.Add(edge);
                queue.Enqueue(edge);

                edgeNode = edgeNode.Next;
            }
        }

        return visited;
    }

    // A modified breadth-first search (BFS) algorithm to find strongly connected components in an undirected graph.
    // The components are identified by assigning a unique component number to each vertex and edge.
    // 
    // The algorithm works as follows: 
    // 1. It initializes a component counter to 0, and a HashSet to store visited vertices.
    // 2. It uses a queue to manage the order of vertices to be visited.
    // 3. The function iterates over each vertex in the graph.
    // 4. If the vertex has been visited before, it skips the current iteration.
    // 5. The component counter is incremented, and the current vertex is assigned the component number.
    // 6. The current vertex is enqueued for processing.
    // 7. The algorithm enters a while loop that continues as long as there are vertices in the queue.
    // 8. Inside the loop, a vertex is dequeued for consideration.
    // 9. The algorithm retrieves the edges connected to the current vertex.
    // 10. It iterates over each edge.
    // 11. If the edge has been visited before, it skips the current iteration.
    // 12. The component number is assigned to the current edge.
    // 13. The edge is marked as visited and enqueued for further processing.
    // 14. Once all vertices have been visited, the function completes.
    // 
    // The result is that each vertex and edge in the graph is assigned a component number, indicating the strongly connected component it belongs to.
    // This algorithm helps identify and label the components in the graph.
    public static int FindStronglyConnectedComponents(UndirectedGraph graph)
    {
        int component = 0; // Counter to keep track of the component number.
        var visited = new HashSet<Vertex>(); // HashSet to store visited vertices.

        var queue = new Queue<Vertex>(); // Queue to manage the order of vertices to be visited.

        // Iterate over each vertex in the graph.
        foreach (var vertex in graph)
        {
            // If the vertex has been visited before, skip this iteration.
            if (visited.Contains(vertex))
                continue;

            component++; // Increment the component counter.

            vertex.Component = component; // Assign the component number to the current vertex.
            queue.Enqueue(vertex); // Enqueue the vertex for processing.

            // Continue the process as long as there are vertices in the queue.
            while (queue.Count > 0)
            {
                var current = queue.Dequeue(); // Dequeue a vertex for consideration.

                var edges = graph.GetAdjacentEdges(current); // Get the list of edges connected to the current vertex.

                // Iterate over each edge connected to the current vertex.
                foreach (var edge in edges)
                {
                    // If the edge has been visited before, skip this iteration.
                    if (visited.Contains(edge))
                        continue;

                    edge.Component = component; // Assign the component number to the current edge.

                    visited.Add(edge); // Mark the edge as visited.
                    queue.Enqueue(edge); // Enqueue the edge for further processing.
                }
            }
        }

        // Return the total number of components in the graph.
        return component;
    }

    // This code is a method that computes a shortest path tree (SPT) from a given origin vertex in a graph.
    // The resulting tree is an oriented graph where each edge points from a node to its parent node in the SPT.
    // It also keeps track of the leaf nodes (i.e., nodes with no children) in the SPT.
    // The method uses a breadth-first search (BFS) approach to traverse the graph.
    // 
    // Here's an in-depth explanation of the function:
    // 
    // 1. The function `GetSimpleShortestPathTree` takes three parameters:
    //      `graph` (the undirected graph to compute the SPT from),
    //      `originVertex` (the vertex to start the SPT from),
    //      `leaves` (an output parameter that will contain the leaf nodes in the SPT).
    // 
    // 2. `tree` is an instance of `OrientedGraph` that will contain the SPT.
    //      It is initialized with all vertices from `graph`.
    // 
    // 3. `leaves` is a HashSet that is initially filled with all vertices of `tree`.
    //      As the algorithm progresses, vertices will be removed from `leaves` as they are found to have child nodes.
    // 
    // 4. `visited` is a list that keeps track of all the vertices that have been visited so far.
    //      This is initialized with `originVertex`.
    // 
    // 5. The distance from `originVertex` to itself is set to 0.
    // 
    // 6. `queue` is a Queue that is used to manage the order of vertices to be visited.
    //      The algorithm starts by enqueuing the `originVertex`.
    // 
    // 7. The `while` loop continues as long as there are still vertices in the queue to be visited.
    // 
    // 8. Inside the loop, it dequeues a vertex `current` from the queue.
    //      This vertex is the current vertex being considered. 
    // 
    // 9. The `level` variable is calculated as the current distance of the vertex incremented by one.
    //      This represents the distance of the vertices directly connected to `current`.
    // 
    // 10. `edges` is a list of all vertices directly reachable from the `current` vertex, obtained by calling `graph.GetEdges(current)`.
    // 
    // 11. Then, it iterates over each vertex `edge` in `edges`.
    //      If the distance of `edge` equals `level`, it means `edge` is a child of `current` in the SPT.
    //      An edge from `edge` to `current` is added to `tree`, and `current` is removed from `leaves` as it is not a leaf node.
    // 
    // 12. If `edge` has been visited before, it skips the rest of this iteration.
    // 
    // 13. Otherwise, it marks `edge` as visited, enqueues `edge` for future consideration, adds an edge from `edge` to `current` in `tree`, and sets the distance of `edge` to `level`.
    // 
    // 14. Once the queue is empty, it means that all reachable vertices from `originVertex` have been visited, and the function returns the SPT as `tree`.
    // 
    // This method essentially creates a tree of shortest paths from the `originVertex` to all other vertices in the graph.
    // The `leaves` set collects all nodes that don't have any child nodes in this tree (i.e., the end points of all paths).
    // The BFS approach ensures that the shortest paths are found first.
    // The `Distance` property of each vertex in the tree gives the shortest distance from the `originVertex` to that vertex.
    public static OrientedGraph GetSimpleShortestPathTree(UndirectedGraph graph, Vertex originVertex, out HashSet<Vertex> leaves)
    {
        var tree = new OrientedGraph($"simple_tree_{originVertex.Index}");
        graph.CopyVerticesTo(tree);

        leaves = new HashSet<Vertex>();
        leaves.UnionWith(tree);

        var visited = new List<Vertex>
        {
            originVertex
        };

        originVertex.Distance = 0;

        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertex);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            int level = (int)(current.Distance ?? 0) + 1;
            var edges = graph.GetAdjacentEdges(current);

            foreach (var edge in edges)
            {
                if (edge.Distance == level)
                    leaves.Remove(current);

                if (visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);

                tree.AddEdge(edge, current);
                edge.Distance = level;
            }
        }

        return tree;
    }

    /// <summary>
    /// This function builds a shortest path tree (SPT) from a given undirected graph, starting from a specific vertex. 
    /// An SPT is a subgraph of the original graph where all vertices are connected by the shortest path from the origin vertex. 
    /// This algorithm uses breadth-first search (BFS) to traverse the graph. The function also identifies the leaf nodes of the tree (i.e., nodes with no children).
    /// </summary>
    /// <param name="graph">The UndirectedGraph to create shortest path tree for.</param>
    /// <param name="originVertex">Start position of tree.</param>
    /// <param name="leaves">The HashSet of leaves, containing all terminating Vertices, to be returned.</param>
    /// <returns>The generated OrientedGraph shortest path tree.</returns>
    public static OrientedGraph GetFullShortestPathTree(UndirectedGraph graph, Vertex originVertex, out HashSet<Vertex> leaves)
    {
        var tree = new OrientedGraph($"full_tree_{originVertex.Index}");
        graph.CopyVerticesTo(tree);

        // A HashSet is initialized to store the leaf nodes. Initially, it is assumed that all nodes are leaf nodes (since no edges have been added yet).
        leaves = new HashSet<Vertex>();
        leaves.UnionWith(tree);

        // A List is used to keep track of visited vertices.
        // The starting vertex is marked as visited.
        var visited = new List<Vertex>
        {
            originVertex
        };

        originVertex.Distance = 0;
        originVertex.Weight = 1;

        // A Queue is used to hold the vertices yet to be visited, following the BFS approach.
        // The origin vertex is the first to be enqueued.
        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertex);

        // The main BFS loop starts.
        // It continues until all vertices have been visited.
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // The current distance level is calculated and the edges connected to the current vertex are retrieved.
            int level = (int)(current.Distance ?? 0) + 1;
            var edges = graph.GetAdjacentEdges(current);

            // If the distance of the edge equals the current level, the edge is added to the tree, connecting it to the current node.
            // The weight of the edge is updated and the current node is removed from the set of leaf nodes.
            foreach (var edge in edges)
            {
                if (edge.Distance == level)
                {
                    tree.AddEdge(edge, current);
                    tree.AddIncomeEdge(current, edge);
                    edge.Weight += current.Weight;

                    leaves.Remove(current);
                }

                if (visited.Contains(edge))
                    continue; // If the edge has been visited before, the loop continues to the next edge.

                // The edge is marked as visited and enqueued for future processing.
                visited.Add(edge);
                queue.Enqueue(edge);

                if (edge.Equals(originVertex))
                    continue;

                // The edge is added to the tree, and its distance and weight are updated.
                // The edge is also added as a neighbor below the current node.
                tree.AddEdge(edge, current);
                edge.Distance = level;
                edge.Weight = current.Weight;

                tree.AddIncomeEdge(current, edge);
            }
        }

        return tree;
    }

    public static int GetNumberOfShortestPaths(UndirectedGraph graph, Vertex start, Vertex end)
    {
        var tree = GetFullShortestPathTree(graph, start, out HashSet<Vertex> leaves);

        return (int)(end.Weight ?? 0);
    }

    #region Betweeness

    public static void CalculateBetweenness(UndirectedGraph graph)
    {
        foreach (var vertex in graph)
        {
            var tree = GetFullShortestPathTree(graph, vertex, out HashSet<Vertex> leaves);

            foreach (var leaf in leaves)
            {
                leaf.Weight ??= 1;
                leaf.Betweenness ??= 1;

                TraceNextTreeNode(tree, leaf);
            }
        }
    }

    private static void TraceNextTreeNode(OrientedGraph tree, Vertex vertex)
    {
        var edges = tree.GetAdjacentEdges(vertex);

        foreach (var edge in edges)
        {
            double? currValue = 1;
            var belowNeighbors = tree.IncomeEdges[edge];
            foreach (var neighbor in belowNeighbors)
            {
                currValue += neighbor.Betweenness * edge.Weight / neighbor.Weight;
            }

            edge.Betweenness = currValue;

            TraceNextTreeNode(tree, edge);
        }
    }

    private static void TraceNextTreeNodeForSimpleTree(OrientedGraph tree, Vertex vertex)
    {
        vertex.Betweenness ??= 0;
        vertex.Betweenness++;

        var edges = tree.GetAdjacentEdges(vertex);

        if (edges.Count == 0)
            return;

        TraceNextTreeNodeForSimpleTree(tree, edges.First());
    }

    public static void CalculateBetweennessSimple(UndirectedGraph graph)
    {
        foreach (var vertex in graph)
        {
            var tree = GetSimpleShortestPathTree(graph, vertex, out HashSet<Vertex> leaves);

            foreach (var leaf in leaves)
            {
                TraceNextTreeNodeForSimpleTree(tree, leaf);
            }
        }
    }

    #endregion
}
