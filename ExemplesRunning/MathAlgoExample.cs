using MathAlgo;

namespace ExamplesRunning;
internal static class MathAlgoExample
{
    internal static void RunMedianExample()
    {
        //var keys = new List<int>() { 4, 9, 4, 13, 12, 8, 11, 9, 4 };
        var keys = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        //var keys = new List<int>() { 5, 4, 3, 2, 1 };

        var median = new Median<int, int>();

        foreach (var key in keys)
        {
            median.Add(key);
        }

        Console.WriteLine(median.Current);
        Console.ReadLine();
    }
}
