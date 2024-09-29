namespace DataStructures.Heaps;

public abstract class BinaryHeap<TKey, TValue> : Heap<TKey, TValue> where TKey : notnull
{
    private const int d = 2;

    protected BinaryHeap(HeapOptions<TKey> options) : base(options)
    {
        if (options.Degree != d)
            throw new ArgumentException("Binary heap must have degree 2", nameof(options.Degree));
    }

    protected BinaryHeap(TKey[] array, HeapOptions<TKey> options) : base(options)
    {
        _nodes = new HeapNode<TKey, TValue>[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            _nodes[i] = new HeapNode<TKey, TValue> { Key = array[i], Value = default };
        }

        Heapify(_nodes);
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

    public void Heapify(HeapNode<TKey, TValue>[] nodes)
    {
        _nodes = nodes;
        _length = nodes.Length;

        for (int i = _length / 2; i >= 0; i--)
        {
            SiftDown(i);
        }
    }
}

