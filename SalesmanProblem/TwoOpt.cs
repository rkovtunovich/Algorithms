using DataStructures.Lists;
using Graphs.Abstraction;
using Graphs.Model;

namespace SalesmanProblem;

public static class TwoOpt
{
    public static void Optimize(SequentialList<Vertex> tour, Graph graph)
    {
        bool isImprovement = true;
        int size = tour.Count;

        while (isImprovement)
        {
            isImprovement = false;
            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 2; j < size; j++)
                {
                    // Calculate the benefit of swapping edges (i, i+1) and (j, j+1)
                    var a = tour[i];
                    var b = tour[i + 1];
                    var c = tour[j];
                    var d = tour[(j + 1) % size];

                    var distanceAC = graph.GetEdgeLength(a, c);
                    var distanceBD = graph.GetEdgeLength(b, d);
                    var distanceAB = graph.GetEdgeLength(a, b);
                    var distanceCD = graph.GetEdgeLength(c, d);

                    var delta = distanceAC + distanceBD - distanceAB - distanceCD;

                    // If the swap improves the tour, perform it
                    if (delta >= 0)
                        continue;

                    (tour[i + 1], tour[j]) = (tour[j], tour[i + 1]);

                    isImprovement = true;
                }
            }
        }
    }
}



