using System.Numerics;

namespace DataStructures.Heaps
    ;

public abstract class Heap<TKey, TValue> where TKey : INumber<TKey>
{
    private const int InitialSize = 8;

    private int _length = 0;

    private HeapNode<TKey, TValue>[] _nodes;

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

    public HeapNode<TKey, TValue> Extremum { get => this[1]; }

    public void Insert(TKey key, TValue value = default)
    {
        if (_length == _nodes.Length)
            IncreaseContainer();

        _length++;
        this[_length] = new() { 
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
        var mimimum = this[1];

        Swap(1, _length);
        _length--;

        SiftDown(1);

        return mimimum;
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
        if(!_positions.TryGetValue(value, out int position)){
            throw new InvalidOperationException($"Value {value} not found.");
        };

        this[position] = new() { 
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
