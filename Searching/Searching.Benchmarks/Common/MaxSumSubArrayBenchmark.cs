namespace Searching.Benchmarks.Common;

[MarkdownExporter]
[MemoryDiagnoser]
public class MaxSumSubArrayBenchmark
{
    private int[] _arr;

    [Params(10, 41, 100)]
    public int N;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _arr = new int[N];
        for (int i = 0; i < N; i++)
        {
            _arr[i] = i;
        }
    }

    [Benchmark]
    public void Find()
    {
        MaxSumSubArray.Find(_arr);
    }

    [Benchmark]
    public void FindBruteForce()
    {
        MaxSumSubArray.FindBruteForce(_arr);
    }

    [Benchmark]
    public void FindHybrid()
    {
        MaxSumSubArray.FindHybrid(_arr, 8);
    }

    [Benchmark]
    public void FinsKadane()
    {
        MaxSumSubArray.FindKadane(_arr);
    }
}
