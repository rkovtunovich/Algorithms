using Graphs.GraphImplementation;
using Graphs.Model;

namespace MaximumCoverage;

public static class GreedyInfluence
{
    private static readonly Random _random = new();

    public static List<Vertex> FindInfluencers(OrientedGraph graph, int influencersInitCount, double influenceDegree)
    {
        // Initialize the set of influencers to be empty
        var influencers = new List<Vertex>();

        // For a fixed number of iterations
        for (int i = 0; i < influencersInitCount; i++)
        {
            Vertex? bestVertex = null;
            int maxIncrease = -1;

            // For each node in the network that is not already in the set of influencers
            foreach (var node in graph)
            {
                if (influencers.Contains(node))
                    continue;

                int increase = GetInfluenceIncrease(graph, influencers, node, influenceDegree);

                // Select the candidate node that results in the largest increase in spread
                if (increase > maxIncrease)
                {
                    maxIncrease = increase;
                    bestVertex = node;
                }

            }

            // Add the selected candidate node to the set of influencers
            if(bestVertex is not null)
                influencers.Add(bestVertex);
        }

        return influencers;
    }

    private static int GetInfluenceIncrease(OrientedGraph graph, List<Vertex> influencers, Vertex candidateNode, double influenceDegree)
    {
        // Simulate the spread of influence that would occur if the candidate node were added to the set of influencers
        var influencedNodes = new HashSet<Vertex>(influencers)
        {
            candidateNode
        };
        var queue = new Queue<Vertex>(influencedNodes);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            foreach (var neighborNode in graph.GetEdges(currentNode))
            {
                if (influencedNodes.Contains(neighborNode))
                    continue;

                if (IsInfluenced(influenceDegree))
                {
                    influencedNodes.Add(neighborNode);
                    queue.Enqueue(neighborNode);
                }
            }
        }

        // Calculate the increase in spread that would result from adding the candidate node to the set of influencers
        return influencedNodes.Count - influencers.Count;
    }

    private static bool IsInfluenced(double influenceDegree)
    {
        var weight = _random.NextDouble();

        return weight >= influenceDegree;
    }
}
