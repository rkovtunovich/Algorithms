﻿using Graphs.GraphImplementation;

namespace Graphs.Search;
public static class BFS<T>
{
    public static HashSet<Vertice<T>> SearchConnected(UndirectedGraph<T> graph, Vertice<T> originVertice)
    {
        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();
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

    public static HashSet<Vertice<T>> MarkPaths(UndirectedGraph<T> graph, Vertice<T> originVertice)
    {
        int level = 0;

        originVertice.Distance = level;

        var visited = new HashSet<Vertice<T>>
        {
            originVertice
        };

        var queue = new Queue<Vertice<T>>();
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

    public static void FindingConnectedComponents(UndirectedGraph<T> graph)
    {
        int component = 0;
        var visited = new HashSet<Vertice<T>>();

        var queue = new Queue<Vertice<T>>();

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

    public static OrientedGraph<T> GetSimpleShortestPathTree(UndirectedGraph<T> graph, Vertice<T> originVertice, out HashSet<Vertice<T>> leaves)
    {
        var tree = new OrientedGraph<T>($"simple_tree_{originVertice.Index}");
        graph.CopyVerticesTo(tree);
        
        leaves = new HashSet<Vertice<T>>();
        leaves.UnionWith(tree);

        var visited = new List<Vertice<T>>
        {
            originVertice
        };

        originVertice.Distance = 0;

        var queue = new Queue<Vertice<T>>();
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

    public static OrientedGraph<T> GetFullShortestPathTree(UndirectedGraph<T> graph, Vertice<T> originVertice, out HashSet<Vertice<T>> leaves)
    {
        var tree = new OrientedGraph<T>($"full_tree_{originVertice.Index}");
        graph.CopyVerticesTo(tree);

        leaves = new HashSet<Vertice<T>>();
        leaves.UnionWith(tree);

        var visited = new List<Vertice<T>>
        {
            originVertice
        };

        originVertice.Distance = 0;
        originVertice.Weight = 1;

        var queue = new Queue<Vertice<T>>();
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
            }
        }

        return tree;
    }

    #region Betweeness

    public static void CalculateBetweeness(UndirectedGraph<T> graph)
    {
        foreach (var vertice in graph)
        {
            var tree = GetFullShortestPathTree(graph, vertice, out HashSet<Vertice<T>> leaves);

            foreach (var leaf in leaves)
            {
                leaf.Betweeness ??= 1;
                leaf.Weight ??= 1;
                TraceNextTreeNode(tree, leaf);
            }

            DOTVisualizer.VisualizeGraph(tree);
        }
    }

    private static void TraceNextTreeNode(OrientedGraph<T> tree, Vertice<T> vertice)
    {
        var edges = tree.GetEdges(vertice);

        foreach (var edge in edges)
        {
            edge.Betweeness ??= 1;
            edge.Betweeness += vertice.Betweeness * edge.Weight / vertice.Weight;

            TraceNextTreeNode(tree, edge);
        }     
    }

    private static void TraceNextTreeNodeForSimpleTree(OrientedGraph<T> tree, Vertice<T> vertice)
    {
        vertice.Betweeness ??= 0;
        vertice.Betweeness++;

        var edges = tree.GetEdges(vertice);

        if (edges.Count == 0)
            return;

        TraceNextTreeNodeForSimpleTree(tree, edges.First());
    }

    public static void CalculateBetweenessSimple(UndirectedGraph<T> graph)
    {
        foreach (var vertice in graph)
        {
            var tree = GetSimpleShortestPathTree(graph, vertice, out HashSet<Vertice<T>> leaves);

            foreach (var leaf in leaves)
            {
                TraceNextTreeNodeForSimpleTree(tree, leaf);
            }

            DOTVisualizer.VisualizeGraph(tree);
        }
    }

    #endregion
}
