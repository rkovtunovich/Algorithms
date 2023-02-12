using Helpers;
using KnapsackProblem;
using View;

namespace ExamplesRunning;

internal static class KnapsackExample
{
    public static void Run()
    {
        var values = ArrayHelper.GetUnsortedArray(5, 1, 20).ToList();
        var sizes = ArrayHelper.GetUnsortedArray(5, 1, 10).ToList();
        var capasity = 10;

        var result = Knapsack.Choose(values, sizes, capasity);

        Viewer.ShowArray(values.ToArray());
        Viewer.ShowArray(sizes.ToArray());
        MatrixHelper.Show(result);
    }
}

