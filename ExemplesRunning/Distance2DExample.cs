﻿using Helpers.Space;
using Distance;

namespace ExemplesRunning;

public static class Distance2DExample
{

    public static void Run()
    {
        var points = SpaceHelper.GetSetOf2DPoints(10, 20, 20);

        var point = TwoDimension.GetClosestPoints(points);

        Console.WriteLine(point);

        Console.Read();
    }

}
