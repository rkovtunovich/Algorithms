using Graphs.Abstraction;
using Graphs.GraphImplementation;
using Graphs.Model;

namespace Graphs.Search;
public static class BFS
{
    public static HashSet<Vertex> SearchConnected(Graph graph, Vertex originVertice)
    {
        var visited = new HashSet<Vertex>
        {
            originVertice
        };

        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);
            }
        }

        return visited;
    }

    public static HashSet<Vertex> MarkPaths(UndirectedGraph graph, Vertex originVertice)
    {
        int level = 0;

        originVertice.Distance = level;

        var visited = new HashSet<Vertex>
        {
            originVertice
        };

        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            level = (int)(current.Distance ?? 0) + 1;

            var edges = graph.GetEdges(current);

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

    public static void FindStronglyConnectedComponents(UndirectedGraph graph)
    {
        int component = 0;
        var visited = new HashSet<Vertex>();

        var queue = new Queue<Vertex>();

        foreach (var vertive in graph)
        {
            if (visited.Contains(vertive))
                continue;

            component++;

            vertive.Component = component;
            queue.Enqueue(vertive);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var edges = graph.GetEdges(current);

                foreach (var edge in edges)
                {
                    if (visited.Contains(edge))
                        continue;

                    edge.Component = component;

                    visited.Add(edge);
                    queue.Enqueue(edge);
                }
            }
        }
    }

    public static OrientedGraph GetSimpleShortestPathTree(UndirectedGraph graph, Vertex originVertice, out HashSet<Vertex> leaves)
    {
        var tree = new OrientedGraph($"simple_tree_{originVertice.Index}");
        graph.CopyVerticesTo(tree);

        leaves = new HashSet<Vertex>();
        leaves.UnionWith(tree);

        var visited = new List<Vertex>
        {
            originVertice
        };

        originVertice.Distance = 0;

        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            int level = (int)(current.Distance ?? 0) + 1;
            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (edge.Distance == level)
                {
                    tree.AddEdge(edge, current);
                    leaves.Remove(current);
                }

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

    public static OrientedGraph GetFullShortestPathTree(UndirectedGraph graph, Vertex originVertice, out HashSet<Vertex> leaves)
    {
        var tree = new OrientedGraph($"full_tree_{originVertice.Index}");
        graph.CopyVerticesTo(tree);

        leaves = new HashSet<Vertex>();
        leaves.UnionWith(tree);

        var visited = new List<Vertex>
        {
            originVertice
        };

        originVertice.Distance = 0;
        originVertice.Weight = 1;

        var queue = new Queue<Vertex>();
        queue.Enqueue(originVertice);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            int level = (int)(current.Distance ?? 0) + 1;
            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (edge.Distance == level)
                {
                    tree.AddEdge(edge, current);
                    tree.AddBelowNeighbor(current, edge);
                    edge.Weight += current.Weight;

                    leaves.Remove(current);
                }

                if (visited.Contains(edge))
                    continue;

                visited.Add(edge);
                queue.Enqueue(edge);

                if (edge.Equals(originVertice))
                    continue;

                tree.AddEdge(edge, current);
                edge.Distance = level;
                edge.Weight = current.Weight;

                tree.AddBelowNeighbor(current, edge);
            }
        }

        return tree;
    }

    #region Maximum Flow

    // Ford and Fullkerson algorithm
    public static double AugmentingPathSearch(Graph graph, Vertex source, Vertex target)
    {
        double maxFlow = 0;

        var residualGraph = graph.Clone();

        while (true)
        {
            var (isExist, path) = SearchPath(residualGraph, source, target);

            if (!isExist)
                break;

            double curFlow = 0;
            var current = target;

            while (true)
            {
                var parent = path[current.ArrayIndex()];

                var currLength = residualGraph.GetEdgeLength(parent, current);
                if(curFlow == 0)
                    curFlow = currLength;
                else
                    curFlow = Math.Min(curFlow, currLength);

                current = parent;
                if(current == source)
                    break;
            }

            current = target;
            while (true)
            {
                var parent = path[current.ArrayIndex()];

                residualGraph.ChangeEdgeLength(parent, current, -curFlow);
                residualGraph.ChangeEdgeLength(current, parent, curFlow);

                current = parent;
                if (current == source)
                    break;
            }

            maxFlow += curFlow;

            DOTVisualizer.VisualizeGraph(residualGraph);
        }

        return maxFlow;
    }

    public static (bool isExist, Vertex[] path) SearchPath(Graph graph, Vertex source, Vertex target)
    {
        var parents = new Vertex[graph.Count()];

        var visited = new HashSet<Vertex>
        {
            source
        };

        var queue = new Queue<Vertex>();
        queue.Enqueue(source);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            var edges = graph.GetEdges(current);

            foreach (var edge in edges)
            {
                if (visited.Contains(edge))
                    continue;

                parents[edge.ArrayIndex()] = current;

                if (edge == target)
                    return (true, parents); // path founded

                visited.Add(edge);
                queue.Enqueue(edge);
            }
        }

        return (false, parents);
    }

    #endregion

    #region Betweeness

    public static void CalculateBetweeness(UndirectedGraph graph)
    {
        foreach (var vertice in graph)
        {
            var tree = GetFullShortestPathTree(graph, vertice, out HashSet<Vertex> leaves);

            foreach (var leaf in leaves)
            {
                leaf.Weight ??= 1;
                leaf.Betweeness ??= 1;

                TraceNextTreeNode(tree, leaf);
            }
        }
    }

    private static void TraceNextTreeNode(OrientedGraph tree, Vertex vertice)
    {
        var edges = tree.GetEdges(vertice);

        foreach (var edge in edges)
        {
            double? currValue = 1;
            var belowNeigbors = tree.GetBelowNeighbors(edge);
            foreach (var neighbor in belowNeigbors)
            {
                currValue += neighbor.Betweeness * edge.Weight / neighbor.Weight;
            }

            edge.Betweeness = currValue;

            TraceNextTreeNode(tree, edge);
        }     
    }

    private static void TraceNextTreeNodeForSimpleTree(OrientedGraph tree, Vertex vertice)
    {
        vertice.Betweeness ??= 0;
        vertice.Betweeness++;

        var edges = tree.GetEdges(vertice);

        if (edges.Count == 0)
            return;

        TraceNextTreeNodeForSimpleTree(tree, edges.First());
    }

    public static void CalculateBetweenessSimple(UndirectedGraph graph)
    {
        foreach (var vertice in graph)
        {
            var tree = GetSimpleShortestPathTree(graph, vertice, out HashSet<Vertex> leaves);

            foreach (var leaf in leaves)
            {
                TraceNextTreeNodeForSimpleTree(tree, leaf);
            }
        }
    }

    #endregion
}
