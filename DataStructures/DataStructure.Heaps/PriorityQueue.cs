namespace DataStructures.Heaps;

public class PriorityQueue<TKey, TValue> where TKey : INumber<TKey>
{
    private readonly Heap<TKey, TValue> _heap = null!;

    public PriorityQueue(bool isNonDecreasing)
    {
        var heapOptions = new HeapOptions<TKey>
        {
            UseKeyTracking = true
        };

        if (isNonDecreasing)
            _heap = new HeapMin<TKey, TValue>(heapOptions);     
        else       
            _heap = new HeapMax<TKey, TValue>(heapOptions);       
    }

    public void Enqueue(TKey key, TValue value)
    {
        _heap.Insert(key, value);
    }

    public TValue Dequeue()
    {
        return _heap.ExtractValue();
    }

    public TValue Peek()
    {
        return _heap.Extremum.Value ?? 
            throw new InvalidOperationException("The queue is empty.");
    }

    public void UpdateKeyByValue(TKey key, TValue value)
    {
        _heap.ReplaceKeyByValue(value, key);
    }

    public void UpdateKey(TKey oldKey, TKey newKey)
    {
        _heap.ReplaceKey(oldKey, newKey);
    }
}