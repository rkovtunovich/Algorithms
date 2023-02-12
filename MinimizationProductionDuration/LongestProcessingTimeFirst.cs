using DataStructures.Heaps;
using Sorting;

namespace MinimizationProductionDuration;

public static class LongestProcessingTimeFirst
{
    public static Dictionary<int,List<int>> GetMachineLoading(int machineNumber, List<int> jobs)
    {
        var loading = new Dictionary<int, List<int>>();
        var currentLoadingLengths = new HeapMin<int, int>();

        for (int i = 1; i <= machineNumber; i++)
        {
            loading[i] = new List<int>();
            currentLoadingLengths.Insert(0, i);
        }

        QuickSort.Sort(ref jobs);

        for (int i = jobs.Count - 1; i >= 0  ; i--)
        {
            var minLoaded = currentLoadingLengths.ExtractNode();
            loading[minLoaded.Value].Add(jobs[i]);

            currentLoadingLengths.Insert(minLoaded.Key + jobs[i], minLoaded.Value);
        }

        return loading;
    }
}
