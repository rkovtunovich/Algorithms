using Searching.Common;
using Helpers;
using Sorting.Common;

namespace ExamplesRunning;

internal static class FindingExample
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

    internal static void RunFindingLocalMatrixMinimum()
    {
        int[][] matrix = MatrixHelper.CreateQuadratic<int>(5);
        MatrixHelper.FillRandomly(ref matrix, 0, 25);

        Viewer.ShowMatrix(matrix);

        var mins = LocalMatrixMinimum.Find(matrix);

        foreach (var m in mins)
        {
            Console.WriteLine($"{m[0]}:{m[1]} is {m[2]}");
        }

        Console.Read();
    }

    internal static void RunFindingOrderStatistics()
    {
        var array = ArrayHelper.GetUnsortedArray(10, 0, 10);

        Viewer.ShowArray(array);

        int stat = RSelect.Find(array, 4);

        QuickSort.Sort(array);
        Viewer.ShowArray(array);

        Console.WriteLine(stat);

    }

    internal static void RunFindingMaxSumSubArray()
    {
        var array = ArrayHelper.GetUnsortedArray(10, -10, 10);

        Viewer.ShowArray(array);

        var result = MaxSumSubArray.Find(array);

        Console.WriteLine($"Start: {result.start}, End: {result.end}, Sum: {result.sum}");

        Console.Read();
    }
}

