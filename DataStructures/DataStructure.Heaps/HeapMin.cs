namespace DataStructures.Heaps;

public class HeapMin<TKey, TValue> : BinaryHeap<TKey, TValue> where TKey : IComparable<TKey>
{
    public HeapMin(HeapOptions<TKey> options) : base(options)
    {
    }

    public HeapMin() : base(new HeapOptions<TKey>())
    {
    }

    protected override void SiftUp(int position)
    {
        if (IsRoot(position))
            return;

        int parent = GetParentPosition(position);

        if (_comparer.Compare(this[parent].Key, this[position].Key) <= 0)
            return;

        Swap(parent, position);

        SiftUp(parent);
    }

    protected override void SiftDown(int position)
    {
        int leftChildPos = GetLeftChildPosition(position);
        int rightChildPos = GetRightChildPosition(position);

        int nextPos = GetHeapifyDownPosition(position, leftChildPos, rightChildPos);

        if (nextPos == position)
            return;

        Swap(position, nextPos);
        SiftDown(nextPos);
    }

    private int GetHeapifyDownPosition(int parent, int left, int right) => (parent, left, right) switch
    {
        (_, -1, -1) => parent,
        (_, -1, _) when _comparer.Compare(this[right].Key, this[parent].Key) < 0 => right,
        (_, -1, _) when _comparer.Compare(this[right].Key, this[parent].Key) >= 0 => parent,
        (_, _, -1) when _comparer.Compare(this[left].Key, this[parent].Key) < 0 => left,
        (_, _, -1) when _comparer.Compare(this[left].Key, this[parent].Key) >= 0 => parent,
        (_, _, _) when _comparer.Compare(this[left].Key, this[parent].Key) < 0 && _comparer.Compare(this[left].Key, this[right].Key) <= 0 => left,
        (_, _, _) when _comparer.Compare(this[right].Key, this[parent].Key) < 0 && _comparer.Compare(this[right].Key, this[left].Key) < 0 => right,
        (_, _, _) => parent
    };
}
