using Helpers;
using ResourceOptimization.Allocation;

namespace ExamplesRunning;

internal static class KnapsackExample
{
    public static void Run()
    {
        var values = ArrayHelper.GetUnsortedArray(5, 1, 20).ToList();
        var sizes = ArrayHelper.GetUnsortedArray(5, 1, 10).ToList();
        var capacity = 10;

        var result = KnapsackProblem.Choose(values, sizes, capacity);

        Viewer.ShowArray(values.ToArray());
        Viewer.ShowArray(sizes.ToArray());
        Viewer.ShowMatrix(result);
    }
}