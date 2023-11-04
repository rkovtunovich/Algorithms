namespace Sorting.Benchmarks.Common;

[MarkdownExporter]
[MemoryDiagnoser]
public class BubbleSortBenchmark
{
    private int[] _array = null!;
    private int[] _localArray = null!;

    [Params(100, 1_000, 10_000, 100_000)]
    public int N; // size of the array

    [GlobalSetup]
    public void GlobalSetup()
    {
        _array = Helpers.ArrayHelper.GetUnsortedArray(N);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _localArray = (int[])_array.Clone();
    }

    [Benchmark]
    public void Sort_Array()
    {
        BubbleSort.Sort(_localArray);
    }
}
