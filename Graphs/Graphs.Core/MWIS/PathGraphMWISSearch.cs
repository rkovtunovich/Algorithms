﻿using DataStructures.HashTables;
using Graphs.Core.Model;
using Graphs.Core.Model.Graphs;

namespace Graphs.Core.MWIS;

public static class PathGraphMWISSearch
{
    private static int _counterRecCall = 0;

    public static SimpleHashSet<Vertex> Find(GraphBase pathGraph)
    {
        var set = new SimpleHashSet<Vertex>();
        var vertices = pathGraph.ToList();

        var MWISRec = FindRec(vertices, vertices.Count - 1);

        var calculations = FindIter(vertices);
        var MWISIter = calculations[vertices.Count];
        ReconstructionSet(vertices, calculations, set);

        return set;
    }

    private static double FindRec(List<Vertex> vertices, int position)
    {
        _counterRecCall++;

        if (position is -1)
            return 0;
        if (position is 0)
            return vertices[position].Weight ?? 0;

        var s1 = FindRec(vertices, position - 1);
        var s2 = FindRec(vertices, position - 2);

        return Math.Max(s1, s2 + vertices[position].Weight ?? 0);
    }

    private static double[] FindIter(List<Vertex> vertices)
    {
        var calculations = new double[vertices.Count + 1];
        calculations[0] = 0;
        calculations[1] = vertices[0].Weight ?? 0;

        for (int i = 2; i < calculations.Length; i++)
        {
            calculations[i] = Math.Max(calculations[i - 1], calculations[i - 2] + vertices[i - 1].Weight ?? 0);
        }

        return calculations;
    }

    private static void ReconstructionSet(List<Vertex> vertices, double[] calculations, SimpleHashSet<Vertex> set)
    {
        int i = calculations.Length - 1;

        while (i >= 2)
        {
            if (calculations[i - 1] >= (calculations[i - 2] + vertices[i - 1].Weight ?? 00))
                i--;
            else
            {
                set.Add(vertices[i - 1]);
                i -= 2;
            }
        }

        if (i is 1)
            set.Add(vertices[i - 1]);
    }
}
