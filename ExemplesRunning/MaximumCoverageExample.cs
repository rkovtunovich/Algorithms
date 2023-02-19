using Graphs;
using Graphs.Generators;
using Graphs.GraphImplementation;
using Helpers;
using MaximumCoverage;
using View;

namespace ExamplesRunning;

internal class MaximumCoverageExample
{
    internal static void RunGreedyCoverage()
    {
        var budget = 5;
        var sets = new List<List<int>>();
        var random = new Random();

        for (int i = 0; i < budget + budget / 2; i++)
        {
           var set = ArrayHelper.GetUnsortedArray(random.Next(1, 11), 1, 20, true).ToList();
            sets.Add(set);
        }

        int counter = 0;
        foreach (var set in sets)
        {
            Console.WriteLine($"set:{++counter}");
            Viewer.ShowArray(set.ToArray());
        }

        var result = GreedyCoverage.GetCoverage(sets, budget);
        Console.WriteLine("coverage:");

        foreach (var setIndex in result)
        {
            Console.WriteLine($"{setIndex + 1}");
        }
    }

    internal static void RunGreedyInfluence()
    {
        var participantsCount = 10;
        var influencerCount = 3;
        var influenceDegree = 0.5;
        var generator = new OrientedGraphGenerator(participantsCount, 0.35);

        var graph = generator.Generate("oriented_influence") as OrientedGraph;
        
        DOTVisualizer.VisualizeGraph(graph);

        var result = GreedyInfluence.FindInfluencers(graph, influencerCount, influenceDegree);

        foreach (var index in result)
        {
            Console.WriteLine($"{index}");
        }
    }
}
