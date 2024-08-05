using MathAlgo.Models;

namespace Sorting.QuickSort;

// The problem presented is about sorting intervals when we don't have precise information about the values but rather have intervals within which the values fall.
// Problem Statement:
//  Given an array of closed intervals [a, b], where a and b are integers, where a <= b, sort the intervals based on their start value.
//  These intervals represent the possible values of numbers, but we don't know the exact values.
// Goal:
//  To sort the intervals such there exists a permutation of the intervals {i1, i2, ..., in}
//  for which we can find values cj in [ai, bi] satisfying c1<=c2<=...<cn 
//
//The fuzzy sort algorithm benefits from partial overlapInterval because it reduces the complexity of comparisons
// Time complexity: O(n log n) in average, when all intervals are overlapInterval with each other we have O(n)
// When intervals overlap, they have at least one common value, so we satisfy the condition c1<=c2<=...<cn regardless of the order of the intervals
public static class IntervalsFuzzySort
{
    // A random number generator to be used for selecting pivot indices
    private static readonly Random _random = new();

    // Sorts an array of intervals using QuickSort algorithm
    public static void Sort(List<Interval> intervals)
    {
        if (intervals.Count is 0)
            return;

        FuzzySortInternal(intervals, 0, intervals.Count - 1);
    }

    static void FuzzySortInternal(List<Interval> intervals, int p, int r)
    {
        if (p >= r)      
            return;
        
        (int q, int t) = RandomizedPartition(intervals, p, r);

        FuzzySortInternal(intervals, p, q - 1);
        FuzzySortInternal(intervals, t + 1, r);
    }

    static (int, int) RandomizedPartition(List<Interval> intervals, int p, int r)
    {
        int i = _random.Next(p, r + 1);
        Swap(intervals, i, r);

        return Partition(intervals, p, r);
    }

    static (int, int) Partition(List<Interval> intervals, int p, int r)
    {
        // The pivot interval is the last element in the array
        var pivotInterval = intervals[r];

        // borderBefore is the border between intervals that are before the pivot and intervals that overlap with the pivot
        int borderBefore = p - 1;

        // borderOverlap is the border between intervals that are before the pivot and intervals that overlap with the pivot
        int borderOverlap = p - 1;

        // We should track the overlap interval across the loop
        // we need to be sure that all the interval that we identified as overlapInterval have at least one common value
        // so actual overlap interval can be narrowed down through the loop
        var overlapInterval = pivotInterval;

        for (int j = p; j < r; j++)
        {
            if (intervals[j].Before(overlapInterval))
            {
                borderOverlap++;
                borderBefore++;

                if(borderBefore != borderOverlap)
                    Swap(intervals, borderOverlap, borderBefore);

                if (borderOverlap != j)                
                    Swap(intervals, j, borderBefore);                
            }
            else if (intervals[j].Overlaps(overlapInterval))
            {
                borderOverlap++;

                if (borderOverlap != j)
                    Swap(intervals, borderOverlap, j);

                // Update the overlapInterval interval
                // The overlapInterval interval should have the maximum start and minimum end values
                if (overlapInterval.Start < intervals[j].Start)
                    overlapInterval = new Interval(intervals[j].Start, overlapInterval.End);

                if (overlapInterval.End > intervals[j].End)
                    overlapInterval = new Interval(overlapInterval.Start, intervals[j].End);
            }
        }

        // Move the pivot to its final position
        Swap(intervals, borderOverlap + 1, r);

        return (borderBefore + 1, borderOverlap + 1);
    }

    static void Swap(List<Interval> intervals, int i, int j)
    {
        (intervals[j], intervals[i]) = (intervals[i], intervals[j]);
    }
}
