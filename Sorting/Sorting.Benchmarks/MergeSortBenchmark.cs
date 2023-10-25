using Sorting.Common;

namespace Sorting.Benchmarks;

[MarkdownExporter]
[MemoryDiagnoser]
public class MergeSortBenchmark
{
    private int[] array;
    private int[] localArray;

    [Params(100, 1_000, 10_000, 100_000)]
    public int N; // size of the array

    [GlobalSetup]
    public void GlobalSetup()
    {
        array = Helpers.ArrayHelper.GetUnsortedArray(N);
    }

    [IterationSetup]
    public void IterationSetup()
    {
        localArray = (int[])array.Clone();
    }

    [Benchmark]
    public void Sort()
    {
        MergeSort.Sort(ref localArray);
    }
}
