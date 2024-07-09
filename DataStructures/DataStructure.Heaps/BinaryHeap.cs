namespace DataStructures.Heaps;

public abstract class BinaryHeap<TKey, TValue> : Heap<TKey, TValue> where TKey : IComparable<TKey>
{
    private const int d = 2;

    protected BinaryHeap(int size) : base(size, d)
    {
    }

    protected BinaryHeap() : base(d)
    {
    }

    protected override int GetParentPosition(int childPosition)
    {
        if (_length is 1)
            return 0;

        // division by 2 is equivalent to right shift by 1
        return childPosition >> 1;
    }

    public int GetLeftChildPosition(int pos)
    {
        int childPos = (pos << 1) + 1;

        return childPos >= _length ? -1 : childPos;
    }

    public int GetRightChildPosition(int pos)
    {
        int childPos = (pos << 1) + 2;

        return childPos >= _length ? -1 : childPos;
    }
}

