namespace DataStructures.Heaps;

// A heap is a specialized tree-based data structure that satisfies the heap property.
// It is a complete d-ary tree, meaning all levels of the tree are fully filled except for possibly the last level, which is filled from left to right. 
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

public abstract class Heap<TKey, TValue> where TKey : notnull, IComparable<TKey>
{
    private int _d;

    private const int InitialSize = 8;

    protected int _length = 0;

    protected HeapNode<TKey, TValue>[] _nodes;

    // Dictionary to keep track of the positions of the values in the heap
    private Dictionary<TValue, int> _positionsByValue = [];

    private Dictionary<TKey, int> _positionsByKey = [];

    #region Constructors

    public Heap(int size, int d)
    {
        _nodes = new HeapNode<TKey, TValue>[size];
        _d = d;
    }

    public Heap(int d)
    {
        _nodes = new HeapNode<TKey, TValue>[InitialSize];
        _d = d;
    }

    #endregion

    #region public

    public HeapNode<TKey, TValue> this[int i]
    {
        get
        {
            return _nodes[i];
        }
        set
        {
            _nodes[i] = value;
        }
    }

    public int Degree { get => _d; }

    public int Length { get => _length; }

    public bool Empty => Length is 0;

    public HeapNode<TKey, TValue> Extremum { get => this[0]; }

    public void Insert(TKey key, TValue value = default)
    {
        if (_length == _nodes.Length)
            IncreaseContainer();

        this[_length] = new()
        {
            Key = key,
            Value = value
        };

        if (value is not null)
            _positionsByValue[value] = _length;

        _positionsByKey[key] = _length;

        SiftUp(_length);

        _length++;
    }

    public HeapNode<TKey, TValue> ExtractNode()
    {
        var minimum = Extremum;

        _length--;
        Swap(0, _length);

        SiftDown(0);

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
        if (!_positionsByValue.TryGetValue(value, out int position))       
            throw new InvalidOperationException($"Value {value} not found.");
        
        this[position] = new()
        {
            Value = value,
            Key = newKey
        };

        SiftUp(position);
        SiftDown(position);
    }

    public void ReplaceKey(TKey oldKey, TKey newKey)
    {
        if (!_positionsByKey.TryGetValue(oldKey, out int position))
            throw new InvalidOperationException($"Key {oldKey} not found.");

        this[position] = new()
        {
            Value = this[position].Value,
            Key = newKey
        };

        SiftUp(position);
        SiftDown(position);
    }

    public void RemoveByValue(TValue value)
    {
        if (!_positionsByValue.TryGetValue(value, out int position))
            throw new InvalidOperationException($"Value {value} not found.");

        Swap(position, _length);
        _length--;

        SiftDown(position);
    }

    public void RemoveByKey(TKey key)
    {
        if (!_positionsByKey.TryGetValue(key, out int position))
            throw new InvalidOperationException($"Key {key} not found.");

        Swap(position, _length);
        _length--;

        SiftDown(position);
    }

    #endregion

    #region Service methods

    protected bool IsRoot(int position)
    {
        return position is 0;
    }

    private void IncreaseContainer()
    {
        HeapNode<TKey, TValue>[] keys = new HeapNode<TKey, TValue>[_length * 2];
        Array.Copy(_nodes, 0, keys, 0, _nodes.Length);
        _nodes = keys;
    }

    protected int[] GetChildrenPositions(int position)
    {
        var children = new int[_d];
        for (int i = 1; i <= _d; i++)
        {
            var childPosition = position * _d + i;
            if (childPosition <= _length)
                children[i - 1] = childPosition;
            else
                children[i - 1] = -1;
        }

        return children;
    }

    protected virtual int GetParentPosition(int childPosition)
    {
        if(childPosition <= _d)
            return 0;

        return (childPosition - 1) / _d;
    }

    protected HeapNode<TKey, TValue>[] GetChildren(int position)
    {
        var children = new HeapNode<TKey, TValue>[_d];
        for (int i = 0; i < _d; i++)
        {
            var childPosition = position * _d + i;
            if (childPosition <= _length)
                children[i] = this[childPosition];
        }

        return children;
    }

    protected void Swap(int left, int right)
    {
        var rightValue = this[right].Value;
        if (rightValue is not null)
            _positionsByValue[rightValue] = left;

        _positionsByKey[this[left].Key] = right;

        var leftValue = this[left].Value;
        if (leftValue is not null)
            _positionsByValue[leftValue] = right;

        _positionsByKey[this[right].Key] = left;

        (this[left], this[right]) = (this[right], this[left]);
    }

    protected abstract void SiftUp(int position);

    protected abstract void SiftDown(int position);

    #endregion
}
