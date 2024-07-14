namespace Searching.MajorityElementProblem;

// The Boyer-Moore Voting Algorithm is designed to find the majority element in an array,
// where a majority element is defined as an element that appears more than half the time in the array.
// This algorithm operates in linear time (O(n)) and uses constant space (O(1)).
//
// Majority Element:
// 
// A majority element is defined as an element that appears more than n/2 times in the sequence.
// This condition ensures that there is at most one majority element because if an element appears more than half the time,
// no other element can appear more frequently.
//
// When to Use the Boyer-Moore Voting Algorithm
// Strict Majority Requirement: The Boyer-Moore Voting Algorithm should be used when you need to determine
// if there is an element that appears more than n/2 times.
// Verification Needed: Since the algorithm assumes the existence of a majority element,
// a verification step is necessary to confirm if the candidate truly meets the majority condition.
// Practical Applications of Majority Element Condition
// Voting Systems: To determine if a candidate has received more than half of the votes.
// Consensus Algorithms: In distributed systems where a consensus requires more than half of the nodes to agree on a value.
// Sensor Networks: To identify a dominant reading if more than half of the sensors report the same value.
//
// How the Algorithm Works
// Initialization:
// 
// Start with a candidate element and a counter set to zero.
// Voting Phase:
// 
// Traverse the array. For each element:
// If the counter is zero, set the current element as the candidate and set the counter to one.
// If the current element is the same as the candidate, increment the counter.
// If the current element is different, decrement the counter.
// Verification Phase:
// 
// After one pass through the array, the candidate is the potential majority element.
// A second pass is needed to confirm if the candidate is indeed the majority element by counting its occurrences.

public class BoyerMooreVoting<T> where T : IEquatable<T>
{
    // Public method to find the majority element in an array
    public T? FindMajorityElement(T[] arr)
    {
        if (arr is null || arr.Length is 0)
            return default;

        T? majorityElement = default;
        int count = 0;

        // Voting Phase
        foreach (T element in arr)
        {
            // If the counter is zero, set the current element as the candidate and set the counter to one
            if (count is 0)
            {
                majorityElement = element;
                count = 1;
            }
            // If the current element is the same as the candidate, increment the counter
            else if (Equals(majorityElement, element))
            {
                count++;
            }
            // If the current element is different, decrement the counter
            else
            {
                count--;
            }
        }

        // Verification Phase
        return count > 0 ? majorityElement : default;
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

    // Private method to count the occurrences of an element in a range
    private int CountInRange(T[] arr, int left, int right, T? element)
    {
        int count = 0;

        for (int i = left; i <= right; i++)
        {
            if (Equals(arr[i], element))
                count++;
        }

        return count;
    }
}
