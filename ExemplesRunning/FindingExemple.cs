﻿using Finding;
using Helpers;
using Sorting;
using View;

namespace ExemplesRunning;

internal static class FindingExemple
{
    public static void RunFindingMaxUnimodalArray()
    {
        var unimodalArray = ArrayHelper.GetUnimodalArray(12);
        Viewer.ShowArray(unimodalArray);

        int maxItem = MaxUnimodalArray.Find(unimodalArray);

        Console.WriteLine(maxItem);

        Console.Read();
    }

    public static void RunFindingMatchArrayIndexToValue()
    {
        var array = ArrayHelper.GetSortedArray(15, -3, 12);
        //var array = new[] { -2, -2, 5, 7, 9, 9, 9, 10, 10, 11 };
        //var array = new[] { -3, -3, -1, 0, 3, 3, 5, 8, 9, 11 };
        Viewer.ShowArray(array);

        var result = MatchIndexToArrayValue.IsExist(array);

        Console.WriteLine(result);

        Console.Read();
    }

    internal static void RunFindinfLocalMatrixMimimum()
    {
        int[][] matrix = MatrixHelper.CreateQuadratische<int>(5);
        MatrixHelper.FillRandomly(ref matrix, 0, 25);

        MatrixHelper.Show<int>(matrix);

        var mins = LocalMatrixMinimum.Find(matrix);

        foreach (var m in mins)
        {
            Console.WriteLine($"{m[0]}:{m[1]} is {m[2]}");
        }

        Console.Read();
    }

    internal static void RunFindingOederStatistics()
    {
        var array = ArrayHelper.GetUnsortedArray(10, 0, 10);

        Viewer.ShowArray(array);

        int stat = RSelect.Find(array,4);

        QuickSort.Sort(ref array);
        Viewer.ShowArray(array);

        Console.WriteLine(stat);

    }
}

