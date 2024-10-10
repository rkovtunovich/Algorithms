namespace Searching.Common;

public static class MaxSumSubArray
{
    public static (int start, int end, int sum) Find(int[] arr)
    {
        if (arr is null || arr.Length is 0)       
            throw new ArgumentException("Array is empty or null");
        
        return FindRec(arr, 0, arr.Length - 1);
    }

    private static (int, int, int) FindRec(int[] arr, int start, int end)
    {
        if (start == end)
            return (start, end, arr[start]);

        int mid = start + (end - start) / 2;

        (int leftStart, int leftEnd, int leftSum) = FindRec(arr, start, mid);
        (int rightStart, int rightEnd, int rightSum) = FindRec(arr, mid + 1, end);
        (int crossStart, int crossEnd, int crossSum) = FindCrossingSum(arr, start, mid, end);

        if (leftSum >= rightSum && leftSum >= crossSum)
            return (leftStart, leftEnd, leftSum);
        else if (rightSum >= leftSum && rightSum >= crossSum)
            return (rightStart, rightEnd, rightSum);
        else
            return (crossStart, crossEnd, crossSum);
    }

    private static (int crossStart, int crossEnd, int crossSum) FindCrossingSum(int[] arr, int start, int mid, int end)
    {
        int leftSum = int.MinValue;
        int sum = 0;
        int leftIndex = mid;
        for (int i = mid; i >= start; i--)
        {
            sum += arr[i];
            if (sum > leftSum)
            {
                leftSum = sum;
                leftIndex = i;
            }
        }

        int rightSum = int.MinValue;
        sum = 0;
        int rightIndex = mid + 1;
        for (int i = mid + 1; i <= end; i++)
        {
            sum += arr[i];
            if (sum > rightSum)
            {
                rightSum = sum;
                rightIndex = i;
            }
        }

        return (leftIndex, rightIndex, leftSum + rightSum);
    }

    public static (int start, int end, int sum) FindBruteForce(int[] arr)
    {
        if (arr is null || arr.Length is 0)
            throw new ArgumentException("Array is empty or null");

        var (start, end, maxSum) = FindBruteForce(arr, 0, arr.Length - 1);

        return (start, end, maxSum);
    }

    private static (int start, int end, int sum) FindBruteForce(int[] arr, int start, int end)
    {
        if (arr is null || arr.Length is 0)
            throw new ArgumentException("Array is empty or null");

        int maxSum = int.MinValue;
        int startIndex = 0;
        int endIndex = 0;
        for (int i = start; i <= end; i++)
        {
            int sum = 0;
            for (int j = i; j <= end; j++)
            {
                sum += arr[j];
                if (sum > maxSum)
                {
                    maxSum = sum;
                    startIndex = i;
                    endIndex = j;
                }
            }
        }

        return (startIndex, endIndex, maxSum);
    }

    public static (int start, int end, int sum) FindHybrid(int[] arr, int threshold = 10)
    {
        if (arr is null || arr.Length is 0)
            throw new ArgumentException("Array is empty or null");

        return FindHybridRec(arr, 0, arr.Length - 1);
    }

    private static (int start, int end, int sum) FindHybridRec(int[] arr, int v1, int v2, int threshold = 10)
    {
        if (v1 == v2)
            return (v1, v2, arr[v1]);

        if (v2 - v1 < threshold)
            return FindBruteForce(arr, v1, v2);

        int mid = v1 + (v2 - v1) / 2;

        (int leftStart, int leftEnd, int leftSum) = FindHybridRec(arr, v1, mid);
        (int rightStart, int rightEnd, int rightSum) = FindHybridRec(arr, mid + 1, v2);
        (int crossStart, int crossEnd, int crossSum) = FindCrossingSum(arr, v1, mid, v2);

        if (leftSum >= rightSum && leftSum >= crossSum)
            return (leftStart, leftEnd, leftSum);
        else if (rightSum >= leftSum && rightSum >= crossSum)
            return (rightStart, rightEnd, rightSum);
        else
            return (crossStart, crossEnd, crossSum);
    }

    /// <summary>
    /// Find the maximum sum subarray using Kadane's algorithm.
    /// Initially was implemented as profit calculation in stock market.
    /// Time complexity: O(n)
    /// Space complexity: O(1)
    /// </summary>
    /// <param name="array">The input array</param>
    /// <returns>A tuple containing the start index, end index, and the sum of the maximum sum subarray</returns>
    /// <exception cref="ArgumentException">Thrown when the input array is empty or null</exception>
    public static (int start, int end, int sum) FindKadane(int[] array)
    {
        if (array is null || array.Length is 0)
            throw new ArgumentException("Array is empty or null");

        int maxSum = int.MinValue;
        int sum = 0;
        int startIndex = 0;
        int endIndex = 0;
        int tempStartIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            // Add current element to the sum
            sum += array[i];
            if (sum > maxSum)   
            {
                maxSum = sum;

                // Set the start and end index
                startIndex = tempStartIndex;
                endIndex = i;
            }

            // If sum is negative, then it is better to start from next element
            if (sum < 0)
            {
                sum = 0;

                // Reset the start index
                tempStartIndex = i + 1;
            }
        }

        return (startIndex, endIndex, maxSum);
    }
}
