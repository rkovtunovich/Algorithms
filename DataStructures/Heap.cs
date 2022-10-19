using System.Numerics;

namespace DataStructures;

public abstract class Heap<TKey> where TKey : INumber<TKey>
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
        protected set { _keys[i - 1] = value; }
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

    public TKey Extract()
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

    protected int GetParentPosition(int childPosition)
    {
        if (_length == 1)
            return 1;

        return childPosition / 2;
    }

    protected abstract void HeapifyUp(int position);

    protected abstract void HeapifyDown(int position);

    #endregion
}
