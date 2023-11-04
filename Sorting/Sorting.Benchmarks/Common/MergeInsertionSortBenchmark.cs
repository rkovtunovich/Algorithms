using Helpers;
using Sorting.Common;

namespace Sorting.Benchmarks.Common;

[MarkdownExporter]
[MemoryDiagnoser]
public class MergeInsertionSortBenchmark
{
    private int[] array;
    private int[] localArray;

    [Params(100, 1_000, 10_000, 100_000, 1000_000)]
    public int N; // size of the array

    [Params(16, 32, 64, 128)]
    public int k; // k parameter for merge-insertion sort

    [GlobalSetup]
    public void GlobalSetup()
    {
        array = ArrayHelper.GetUnsortedArray(N);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        localArray = (int[])array.Clone();
    }

    [Benchmark]
    public void Sort()
    {
        MergeInsertionSort.Sort(ref localArray, k);
    }
}
