using DataStructures.Heaps;
using Sorting.QuickSort;

namespace Scheduling.Common;

// The algorithm appears to implement the Longest Processing Time First (LPTF) heuristic for machine scheduling.
// The primary goal is to distribute a list of jobs across a certain number of machines in such a way that the workload is balanced as much as possible.
//
// 1. Initialize Data Structures: A dictionary called loading is used to keep track of which jobs are assigned to each machine.
// A minimum heap (currentLoadingLengths) is used to keep track of the workload on each machine.
// 
// 2. Initialize Machines: The loop initializes each machine with an empty list of jobs and adds it to the heap with a "load" of zero.
// 
// 3. Sort Jobs: The list of jobs is sorted in ascending order. This allows for processing the jobs in descending order, i.e., longest processing time first.
// 
// 4. Assign Jobs: The loop iterates over the sorted list of jobs in reverse. In each iteration, it:
//      Finds the machine with the least current workload.
//      Assigns the current job to that machine.
//      Updates the workload for that machine.
//
// 5. Return the Result: Finally, the algorithm returns the loading dictionary, which shows which jobs are assigned to each machine.
// 
// The heap data structure is utilized to find the least loaded machine efficiently.
// The HeapMin class implements a minimum heap where the machine with the least workload can be extracted in logarithmic time.

public static class LongestProcessingTimeFirst
{
    // The main function that schedules jobs on machines
    public static Dictionary<int, List<int>> GetMachineLoading(int machineNumber, List<int> jobs)
    {
        // Initialize a dictionary to hold machine IDs and their corresponding jobs
        var loading = new Dictionary<int, List<int>>();

        // Initialize a minimum heap to keep track of current loading lengths of machines
        var currentLoadingLengths = new HeapMin<int, int>();

        // Initialize machines with empty job lists and add them to the heap with zero loading
        for (int i = 1; i <= machineNumber; i++)
        {
            loading[i] = new List<int>();
            currentLoadingLengths.Insert(0, i);
        }

        // Sort the jobs list in ascending order
        QuickSortClassic.Sort(ref jobs);

        // Loop through the jobs list in reverse order (i.e., starting from the longest job)
        for (int i = jobs.Count - 1; i >= 0; i--)
        {
            // Extract the machine that is currently the least loaded
            var minLoaded = currentLoadingLengths.ExtractNode();

            // Assign the current job to the least loaded machine
            loading[minLoaded.Value].Add(jobs[i]);

            // Update the loading length of the machine in the heap
            currentLoadingLengths.Insert(minLoaded.Key + jobs[i], minLoaded.Value);
        }

        // Return the final machine-job mapping
        return loading;
    }
}

