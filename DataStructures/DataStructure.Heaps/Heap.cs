namespace DataStructures.Heaps;

// A heap is a specialized tree-based data structure that satisfies the heap property.
// It is a complete binary tree, meaning all levels of the tree are fully filled except for possibly the last level, which is filled from left to right. 
// 
// There are two types of heaps: 
// 1. **Max-Heap**: In a max heap, for any given node I, the value of I is greater than or equal to the values of its children.
//    This property must be recursively true for all nodes in the binary tree.
//    The largest element in a max heap is stored at the root, and the subtree of any node also includes this property.
// 
// 2. **Min-Heap**: In a min heap, for any given node I, the value of I is less than or equal to the values of its children.
//    This property must be recursively true for all nodes in the binary tree.
//    The smallest element in a min heap is stored at the root, and the subtree of any node also includes this property.
// 
// Heaps are used in many algorithms, including sorting algorithms like heapsort and in constructing priority queues.
// Priority queues are commonly used in Dijkstra's algorithm for finding the shortest path in a graph, in load balancing and interrupt handling in an operating system, and in data compression algorithms.
// 
// Heaps are efficient for the following reasons: 
// 1. **Insertion**: Insertion of a new element takes O(log n) time.
//    The new element is initially appended to the end of the heap (as the last element of the array).
//    The heap property is restored by comparing the added element with its parent and moving the added element up a level (swapping positions with the parent).
//    The comparison is repeated until the heap property is restored (the added element is smaller than its parent). 
// 2. **Deletion**: Deletion of the root element also takes O(log n) time.
//    The root element is removed and the last element of the heap is moved to the root position.
//    The heap property is restored by comparing the new root with its children and moving it down a level (swapping positions with the larger child for a max heap or the smaller child for a min heap).
//    The comparison is repeated until the heap property is restored (the root is larger than its children in a max heap or smaller in a min heap). 
// 3. **Extremum (Max or Min) Fetch**: Fetching the maximum element in a max heap or the minimum element in a min heap takes O(1) time, as these elements are always at the root of the heap.
// 4. **Memory**: A heap can be stored efficiently in memory as an array.
//    Each element in the array represents a node in the tree, and the parent-child relationship can be calculated using indices.
// 
// In conclusion, heaps are a versatile data structure that are particularly useful when you need to repeatedly remove the object with the highest (or lowest) priority.
// They provide a good compromise of both operation efficiency and memory usage.


public abstract class Heap<TKey, TValue> where TKey : INumber<TKey>
{
    private const int InitialSize = 8;

    private int _length = 0;

    private HeapNode<TKey, TValue>[] _nodes;

    // Dictionary to keep track of the positions of the values in the heap
    private Dictionary<TValue, int> _positions = new();

    #region Constructors

    public Heap(int size)
    {
        _nodes = new HeapNode<TKey, TValue>[size];
    }

    public Heap()
    {
        _nodes = new HeapNode<TKey, TValue>[InitialSize];
    }

    #endregion

    #region public

    public HeapNode<TKey, TValue> this[int i]
    {
        get
        {
            return _nodes[i - 1];
        }
        set
        {
            _nodes[i - 1] = value;
        }
    }

    public int Length { get => _length; }

    public bool Empty()
    {
        return Length == 0;
    }

    public HeapNode<TKey, TValue> Extremum { get => this[1]; }

    public void Insert(TKey key, TValue value = default)
    {
        if (_length == _nodes.Length)
            IncreaseContainer();

        _length++;
        this[_length] = new()
        {
            Key = key,
            Value = value
        };
        _positions[value] = _length;

        SiftUp(_length);
    }

    public int GetLeftChildPosition(int pos)
    {
        int childPos = 2 * pos;

        return childPos > _length ? -1 : childPos;
    }

    public int GetRightChildPosition(int pos)
    {
        int childPos = 2 * pos + 1;

        return childPos > _length ? -1 : childPos;
    }

    public HeapNode<TKey, TValue> ExtractNode()
    {
        var minimum = this[1];

        Swap(1, _length);
        _length--;

        SiftDown(1);

        return minimum;
    }

    public TKey ExtractKey()
    {
        var node = ExtractNode();

        return node.Key;
    }

    public TValue ExtractValue()
    {
        var node = ExtractNode();

        return node.Value;
    }

    public void ReplaceKeyByValue(TValue value, TKey newKey)
    {
        if (!_positions.TryGetValue(value, out int position))
        {
            throw new InvalidOperationException($"Value {value} not found.");
        };

        this[position] = new()
        {
            Value = value,
            Key = newKey
        };

        SiftUp(position);
        SiftDown(position);
    }

    #endregion

    #region Service methods

    private void IncreaseContainer()
    {
        HeapNode<TKey, TValue>[] keys = new HeapNode<TKey, TValue>[_length * 2];
        Array.Copy(_nodes, 0, keys, 0, _nodes.Length);
        _nodes = keys;
    }

    protected int GetParentPosition(int childPosition)
    {
        if (_length == 1)
            return 1;

        return childPosition / 2;
    }

    protected void Swap(int left, int right)
    {
        _positions[this[right].Value ?? throw new NullReferenceException("Value can't be null")] = left;
        _positions[this[left].Value ?? throw new NullReferenceException("Value can't be null")] = right;
        (this[left], this[right]) = (this[right], this[left]);
    }

    protected abstract void SiftUp(int position);

    protected abstract void SiftDown(int position);

    #endregion
}
