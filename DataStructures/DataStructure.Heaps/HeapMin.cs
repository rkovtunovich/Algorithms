namespace DataStructures.Heaps;

public class HeapMin<TKey, TValue> : BinaryHeap<TKey, TValue> where TKey : IComparable<TKey>
{
    protected override void SiftUp(int position)
    {
        if (IsRoot(position))
            return;

        int parent = GetParentPosition(position);

        if (this[parent].Key.CompareTo(this[position].Key) <= 0)
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
        (_, -1, _) when this[right].Key.CompareTo(this[parent].Key) < 0 => right,
        (_, -1, _) when this[right].Key.CompareTo(this[parent].Key) >= 0 => parent,
        (_, _, -1) when this[left].Key.CompareTo(this[parent].Key) < 0 => left,
        (_, _, -1) when this[left].Key.CompareTo(this[parent].Key) >= 0 => parent,
        (_, _, _) when this[left].Key.CompareTo(this[parent].Key) < 0 && this[left].Key.CompareTo(this[right].Key) <= 0 => left,
        (_, _, _) when this[right].Key.CompareTo(this[parent].Key) < 0 && this[right].Key.CompareTo(this[left].Key) < 0 => right,
        (_, _, _) => parent
    };
}
