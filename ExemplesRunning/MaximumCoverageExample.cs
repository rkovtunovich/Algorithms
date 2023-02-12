﻿using Helpers;
using MaximumCoverage;
using View;

namespace ExamplesRunning;

internal class MaximumCoverageExample
{
    internal static void Run()
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
}
