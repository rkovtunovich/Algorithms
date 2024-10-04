namespace Searching.MediansAndOrderStatistics;

/// <summary>
/// This class implements an algorithm to find the Weighted Median of a set of elements.
/// The weighted median is the element such that the sum of the weights of all elements
/// smaller than the median is less than or equal to 1/2, and the sum of the weights 
/// of all elements greater than the median is less than or equal to 1/2.
/// </summary>
public static class WeightedMedian
{
    private static Random _random = new();

    /// <summary>
    /// Finds the weighted median of a set of values and their corresponding weights.
    /// The algorithm works in O(n) time by utilizing a selection algorithm 
    /// to partition the values and compute their cumulative weights.
    /// </summary>
    /// <param name="values">Array of elements whose weighted median needs to be found.</param>
    /// <param name="weights">Array of corresponding weights for each value.</param>
    /// <returns>The weighted median value.</returns>
    public static WeightedValue Find(WeightedValue[] weightedValues)
    {
        // Check if the input arrays are non-empty.
        if (weightedValues.Length is 0)
            throw new ArgumentException("Values and weights must not be empty.");

        // Normalize the weights to ensure they sum up to 1.
        NormalizeWeights(weightedValues);

        // Call the recursive helper function to find the weighted median.
        return WeightedMedianHelper(weightedValues, 0, weightedValues.Length - 1);
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
    private static WeightedValue WeightedMedianHelper(WeightedValue[] weightedValues, int left, int right, double weightBelow = 0, double weightAbove = 0)
    {
        // Base case: If the subarray has one element, return it (as it's the median).
        if (left == right)
            return weightedValues[left];

        // Step 1: Use a linear-time selection algorithm (RSelect) to find the pivot.
        // Use a custom RSelect that returns the pivot index
        int pivotIndex = GetPivotIndex(weightedValues, left, right);

        // Step 2: Partition the array around the pivot and calculate the total weights.
        int partitionIndex = Partition(weightedValues, left, right, pivotIndex); // Partition the array

        Console.WriteLine($"Pivot: {weightedValues[partitionIndex].Value}");

        double totalWeightLeft = 0;
        double totalWeightRight = 0;

        // Calculate the total weight on the left of the partition (elements smaller than pivot).
        for (int i = left; i < partitionIndex; i++)
            totalWeightLeft += weightedValues[i].Weight;

        // Calculate the total weight on the right of the partition (elements greater than pivot).
        for (int i = partitionIndex + 1; i <= right; i++)
            totalWeightRight += weightedValues[i].Weight;

        // Step 3: Check the weighted median conditions.
        // If the total weight on the left is less than 1/2 and adding the pivot's weight reaches or exceeds 1/2,
        // then the pivot is the weighted median.
        var partitionWeight = weightedValues[partitionIndex].Weight;
        if (totalWeightLeft + weightBelow < 0.5 && totalWeightLeft + weightBelow + partitionWeight >= 0.5)
        {
            return weightedValues[partitionIndex];  // The pivot is the weighted median.
        }
        // If the total weight on the left exceeds 1/2, the weighted median is in the left part.
        else if (totalWeightLeft + weightBelow >= 0.5)    
        {
            return WeightedMedianHelper(weightedValues, left, partitionIndex - 1, weightBelow, totalWeightRight + weightAbove);
        }
        // If the total weight on the right is less than 1/2, the weighted median is in the right part.
        else
        {
            return WeightedMedianHelper(weightedValues, partitionIndex + 1, right, totalWeightLeft + partitionWeight + weightBelow, weightAbove);
        }
    }

    private static int GetPivotIndex(WeightedValue[] weightedValues, int left, int right)
    {
        var list = weightedValues[left..(right + 1)];
        int orderStatistics = (right - left) / 2;
        var element = RSelect<WeightedValue>.Find(list, orderStatistics); // Find pivot using Randomized-Select.
        int pivotIndex = Array.IndexOf(weightedValues, element, left); // Get the index of the pivot.

        return pivotIndex;
    }

    private static void Swap(WeightedValue[] array, int i, int j)
    {
        if (i != j)
            (array[j], array[i]) = (array[i], array[j]);
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
    private static int Partition(WeightedValue[] weightedValues, int left, int right, int pivotIndex)
    {
        // Get the value of the pivot and move it to the end of the subarray.
        var pivotValue = weightedValues[pivotIndex];
        Swap(weightedValues, pivotIndex, right);

        int storeIndex = left;
        // Partition the array by moving elements smaller than the pivot to the left.
        for (int i = left; i < right; i++)
        {
            if (weightedValues[i].CompareTo(pivotValue) < 0)
            {
                Swap(weightedValues, i, storeIndex);
                storeIndex++;
            }
        }

        // Move the pivot element to its final sorted position.
        Swap(weightedValues, storeIndex, right);

        return storeIndex;  // Return the final position of the pivot.
    }

    private static void NormalizeWeights(WeightedValue[] weightedValues)
    {
        double totalWeight = 0;
        foreach (var value in weightedValues)
        {
            if (value.Weight < 0)
                throw new ArgumentException("Weights must be non-negative.");

            totalWeight += value.Weight;
        }
            

        if(totalWeight is 0)
            throw new ArgumentException("Total weight must be greater than zero.");

        if (totalWeight is 1)
            return;

        for (int i = 0; i < weightedValues.Length; i++)
            weightedValues[i].Weight /= totalWeight;
    }
}
