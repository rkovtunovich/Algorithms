namespace Searching.MediansAndOrderStatistics;

public class ClosestNeighborsSearch
{
    /// <summary>
    /// Finds the k closest neighbors to a given target in a sequence of integers.
    /// </summary>
    /// <param name="sequence">The input list of integers.</param>
    /// <param name="target">The target value to find the closest neighbors to.</param>
    /// <param name="k">The number of closest neighbors to find.</param>
    /// <returns>A list of k closest neighbors to the target.</returns>
    public static List<int> Find(List<int> sequence, int target, int k)
    {
        if (sequence.Count is 0)
            return [];

        // Step 1: Compute the absolute differences with the target
        var differences = sequence.Select(x => Math.Abs(x - target)).ToList();

        // Step 2: Find the k-th smallest element in the differences array using RSelect
        var kthSmallestDifference = RSelect.Find(differences.ToArray(), k - 1);

        // Step 3: Find all elements in the original sequence that have a difference less than or equal to the k-th smallest difference
        var result = new List<int>();
        for (int i = 0; i < sequence.Count; i++)
        {
            if (Math.Abs(sequence[i] - target) <= kthSmallestDifference)
                result.Add(sequence[i]);
        }

        return result;
    }
}
