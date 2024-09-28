namespace Searching.MediansAndOrderStatistics;

/// <summary>
/// This class implements an algorithm to find the Weighted Median of a set of elements.
/// The weighted median is the element such that the sum of the weights of all elements
/// smaller than the median is less than or equal to 1/2, and the sum of the weights 
/// of all elements greater than the median is less than or equal to 1/2.
/// </summary>
public static class WeightedMedian
{
    /// <summary>
    /// Finds the weighted median of a set of values and their corresponding weights.
    /// The algorithm works in O(n) time by utilizing a selection algorithm 
    /// to partition the values and compute their cumulative weights.
    /// </summary>
    /// <param name="values">Array of elements whose weighted median needs to be found.</param>
    /// <param name="weights">Array of corresponding weights for each value.</param>
    /// <returns>The weighted median value.</returns>
    public static double Find(double[] values, double[] weights)
    {
        // Check if the input arrays have the same length.
        if (values.Length != weights.Length)
            throw new ArgumentException("Values and weights must have the same length.");

        // Check if the arrays are non-empty.
        if (values.Length == 0 || weights.Length == 0)
            throw new ArgumentException("Values and weights must not be empty.");

        // Call the recursive helper function to find the weighted median.
        return WeightedMedianHelper(values, weights, 0, values.Length - 1);
    }

    /// <summary>
    /// Recursively finds the weighted median in the specified subarray.
    /// This method partitions the array based on the pivot and checks the total weight 
    /// of elements to the left and right of the pivot to determine the weighted median.
    /// </summary>
    /// <param name="values">Array of elements.</param>
    /// <param name="weights">Array of weights corresponding to the elements.</param>
    /// <param name="left">Left boundary index of the subarray.</param>
    /// <param name="right">Right boundary index of the subarray.</param>
    /// <returns>The weighted median value.</returns>
    private static double WeightedMedianHelper(double[] values, double[] weights, int left, int right)
    {
        // Base case: If the subarray has one element, return it (as it's the median).
        if (left == right)
            return values[left];

        // Step 1: Use a linear-time selection algorithm (RSelect) to find the pivot.
        var span = values.AsSpan(left, right - left + 1);
        int orderStatistics = (right - left) / 2;
        (var pivotIndex, _) = RSelect<double>.Find(span, orderStatistics); // Find pivot using Randomized-Select.

        // Step 2: Partition the array around the pivot and calculate the total weights.
        double totalWeightLeft = 0;
        double totalWeightRight = 0;
        int partitionIndex = Partition(values, weights, left, right, pivotIndex); // Partition the array.

        // Calculate the total weight on the left of the partition (elements smaller than pivot).
        for (int i = left; i < partitionIndex; i++)
            totalWeightLeft += weights[i];

        // Calculate the total weight on the right of the partition (elements greater than pivot).
        for (int i = partitionIndex + 1; i <= right; i++)
            totalWeightRight += weights[i];

        // Step 3: Check the weighted median conditions.
        // If the total weight on the left is less than 1/2 and adding the pivot's weight reaches or exceeds 1/2,
        // then the pivot is the weighted median.
        if (totalWeightLeft < 0.5 && totalWeightLeft + weights[partitionIndex] >= 0.5)
        {
            return values[partitionIndex];  // The pivot is the weighted median.
        }
        // If the total weight on the left exceeds 1/2, the weighted median is in the left part.
        else if (totalWeightLeft >= 0.5)
        {
            return WeightedMedianHelper(values, weights, left, partitionIndex - 1);
        }
        // If the total weight on the right is less than 1/2, the weighted median is in the right part.
        else
        {
            return WeightedMedianHelper(values, weights, partitionIndex + 1, right);
        }
    }

    /// <summary>
    /// Partitions the values and weights arrays around a pivot.
    /// Elements smaller than the pivot are moved to the left, and elements greater than
    /// or equal to the pivot are moved to the right. The weights are swapped accordingly.
    /// </summary>
    /// <param name="values">Array of values to partition.</param>
    /// <param name="weights">Array of weights corresponding to the values.</param>
    /// <param name="left">Left boundary index of the subarray to partition.</param>
    /// <param name="right">Right boundary index of the subarray to partition.</param>
    /// <param name="pivotIndex">Index of the pivot element.</param>
    /// <returns>The final index of the pivot after partitioning.</returns>
    private static int Partition(double[] values, double[] weights, int left, int right, int pivotIndex)
    {
        // Get the value of the pivot and move it to the end of the subarray.
        double pivotValue = values[pivotIndex];
        (values[pivotIndex], values[right], weights[pivotIndex], weights[right])
            = (values[right], values[pivotIndex], weights[right], weights[pivotIndex]);

        int storeIndex = left;
        // Partition the array by moving elements smaller than the pivot to the left.
        for (int i = left; i < right; i++)
        {
            if (values[i] < pivotValue)
            {
                (values[i], values[storeIndex], weights[i], weights[storeIndex])
                    = (values[storeIndex], values[i], weights[storeIndex], weights[i]);

                storeIndex++;
            }
        }

        // Move the pivot element to its final sorted position.
        (values[storeIndex], values[right], weights[storeIndex], weights[right])
            = (values[right], values[storeIndex], weights[right], weights[storeIndex]);

        return storeIndex;  // Return the final position of the pivot.
    }
}
