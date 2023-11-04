using Sorting.Insertion;

namespace Sorting.Benchmarks.Insertion;

[MarkdownExporter]
[MemoryDiagnoser]
public class InsertionSortBenchmark
{
    private int[] _array = null!;
    private int[] _localArray = null!;
    private int[] _localArray2 = null!;

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
        _localArray2 = (int[])_array.Clone();
    }

    [Benchmark]
    public void Sort_Array()
    {
        InsertionSort.Sort(_localArray, false);
    }

    [Benchmark]
    public void Sort_Span()
    {
        InsertionSort.Sort(_localArray2.AsSpan(), false);
    }
}
