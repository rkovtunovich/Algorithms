namespace Searching.MajorityElementProblem;

public class DivideAndConquerApproach<T> where T : IEquatable<T>
{
    // Public method to find the majority element in an array
    public T? FindMajorityElement(T[] arr)
    {
        if (arr is null || arr.Length is 0)
            return default;

        return FindMajorityElementRec(arr, 0, arr.Length - 1);
    }

    // Public method to check if there is a majority element in an array
    public bool HasMajorityElement(T[] arr)
    {
        if (arr is null || arr.Length is 0)
            return false;

        T? majorityElement = FindMajorityElement(arr);

        if (majorityElement is null)
            return false;

        if (Equals(majorityElement, default(T)))
            return false;

        // Verify if the found element is indeed the majority
        return CountInRange(arr, 0, arr.Length - 1, majorityElement) > arr.Length / 2;
    }

    // Private recursive method to find the majority element in a range
    private T? FindMajorityElementRec(T[] arr, int left, int right)
    {
        // Base case: only one element in the range
        if (left == right)
            return arr[left];

        // Divide the array into two halves
        int mid = left + (right - left) / 2;

        // Find the majority element in the left half
        T? leftMajority = FindMajorityElementRec(arr, left, mid);
        // Find the majority element in the right half
        T? rightMajority = FindMajorityElementRec(arr, mid + 1, right);

        // If left half has no majority, return the right half's majority
        if (leftMajority is null)
            return rightMajority;

        // If right half has no majority, return the left half's majority
        if (rightMajority is null)
            return leftMajority;

        // If both halves agree on the majority element, return it
        if (leftMajority.Equals(rightMajority))
            return leftMajority;

        // Count occurrences of the left and right majorities in the current range
        int leftCount = CountInRange(arr, left, right, leftMajority);
        int rightCount = CountInRange(arr, left, right, rightMajority);

        // Return the element with the higher count if it is a majority
        if (leftCount > (right - left + 1) / 2)
            return leftMajority;

        if (rightCount > (right - left + 1) / 2)
            return rightMajority;

        // No majority element found in this range
        return default;
    }

    // Private method to count occurrences of a candidate in a range
    private int CountInRange(T[] arr, int left, int right, T candidate)
    {
        int count = 0;
        for (int i = left; i <= right; i++)
        {
            if (arr[i].Equals(candidate))
                count++;
        }

        return count;
    }
}