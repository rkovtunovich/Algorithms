﻿using Helpers.Space;
using Spacing.Common.TwoDimension;

namespace ExamplesRunning;

public static class Distance2DExample
{
    public static void Run()
    {
        var points = SpaceHelper.GetSetOf2DPoints(10, 20, 20);

        var point = ClosestPointsFinder.GetClosestPoints(points);

        Console.WriteLine(point);

        Console.Read();
    }
}
