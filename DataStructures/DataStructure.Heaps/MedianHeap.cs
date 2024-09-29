namespace DataStructures.Heaps;

/// <summary>
/// The Median class maintains the median of a stream of numbers efficiently using two heaps:
/// a max-heap for the smaller half of the numbers and a min-heap for the larger half.
/// 
/// Algorithm Description:
/// 1. **Initialization**:
///     - Two heaps are used:
///       - `_heapMax`: A max-heap to store the smaller half of the numbers.
///       - `_heapMin`: A min-heap to store the larger half of the numbers.
/// 
/// 2. **Add Method**:
///     - When a new number is added:
///         a. If the max-heap `_heapMax` is empty or the new number is less than or equal to the maximum element of `_heapMax`,
///            the new number is added to `_heapMax`.
///         b. Otherwise, the new number is added to `_heapMin`.
///     - After adding the new number, the sizes of the two heaps are checked:
///         a. If `_heapMax` has two more elements than `_heapMin`, the root of `_heapMax` (the largest element) is moved to `_heapMin`.
///         b. If `_heapMin` has two more elements than `_heapMax`, the root of `_heapMin` (the smallest element) is moved to `_heapMax`.
///     - This ensures that the heaps are balanced such that the size difference between them is at most one.
/// 
/// 3. **Current Property**:
///     - The `Current` property returns the current median:
///         a. If `_heapMax` has more elements or an equal number of elements as `_heapMin`, the median is the root of `_heapMax`.
///         b. If `_heapMin` has more elements, the median is the root of `_heapMin`.
/// 
/// Efficiency:
/// - Insertion of a new element takes O(log n) time due to the heap operations.
/// - Retrieving the current median takes O(1) time, as it simply involves accessing the root of one of the heaps.
/// 
/// This approach is particularly useful for real-time data streams where the median needs to be maintained dynamically and efficiently.
/// </summary>
public class MedianHeap<TKey, TValue> where TKey : INumber<TKey>
{
    private readonly HeapMin<TKey, TValue> _heapMin;
    private readonly HeapMax<TKey, TValue> _heapMax;

    public MedianHeap()
    {
        _heapMin = new(new());
        _heapMax = new(new());
    }

    public void Add(TKey key)
    {
        if (key <= _heapMax.Extremum.Key)
            _heapMax.Insert(key);
        else
            _heapMin.Insert(key);

        int lengthDiff = _heapMax.Length - _heapMin.Length;

        if (lengthDiff is 2)
        {
            _heapMin.Insert(_heapMax.ExtractKey());
        }
        else if (lengthDiff is -2)
        {
            _heapMax.Insert(_heapMin.ExtractKey());
        }
    }

    public TKey Current { get => _heapMax.Length >= _heapMin.Length ? _heapMax.Extremum.Key : _heapMin.Extremum.Key; }
}
