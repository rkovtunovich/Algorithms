using System.Numerics;

namespace DataStructures;

public class Heap<TKey> where TKey : INumber<TKey>
{
    private const int InitialSize = 8;

    private int _length = 0;

    private TKey[] _keys;

    #region Constructors

    public Heap(int size)
    {
        _keys = new TKey[size];
    }

    public Heap()
    {
        _keys = new TKey[InitialSize];
    }

    #endregion

    #region public

    public TKey this[int i]
    {
        get { return _keys[i - 1]; }
        private set { _keys[i - 1] = value; }
    }

    public int Length { get => _length; }

    public TKey Root { get => _keys[0]; }

    public void Insert(TKey key)
    {
        if (_length == _keys.Length)
            IncreaseContainer();

        _length++;
        this[_length] = key;

        HeapifyUp(_length);
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

    public TKey ExtractMimimum()
    {
        var mimimum = _keys[0];

        (_keys[0], _keys[_length - 1]) = (_keys[_length - 1], _keys[0]);
        _length--;

        HeapifyDown(1);

        return mimimum;
    }

    #endregion

    #region Service methods

    private void IncreaseContainer()
    {
        TKey[] keys = new TKey[_length * 2];
        Array.Copy(_keys, 0, keys, 0, _keys.Length);
        _keys = keys;
    }

    private int GetParentPosition(int childPosition)
    {
        if (_length == 1)
            return 1;

        return childPosition / 2;
    }

    private void HeapifyUp(int position)
    {
        if (position is 1)
            return;

        int parent = GetParentPosition(position);

        if (this[parent] <= this[position])
            return;

        (this[parent], this[position]) = (this[position], this[parent]);

        HeapifyUp(parent);
    }

    private void HeapifyDown(int position)
    {
        int leftChildPos = GetLeftChildPosition(position);
        int rightChildPos = GetRightChildPosition(position);

        int nextPos = GetHeapifyDownPosition(position, leftChildPos, rightChildPos);

        if (nextPos == position)
            return;

        (this[position], this[nextPos]) = (this[nextPos], this[position]);
        HeapifyDown(nextPos);
    }

    private int GetHeapifyDownPosition(int parent, int left, int right) => (parent, left, right) switch
    {
        (_, -1, -1) => parent,
        (_, -1, _) when this[right] < this[parent] => right,
        (_, -1, _) when this[right] >= this[parent] => parent,
        (_, _, -1) when this[left] < this[parent] => left,
        (_, _, -1) when this[left] >= this[parent] => parent,
        (_, _, _) when this[left] < this[parent] && this[left] <= this[right] => left,
        (_, _, _) when this[right] < this[parent] && this[right] < this[left] => right,
        (_, _, _) => parent
    };

    #endregion
}
