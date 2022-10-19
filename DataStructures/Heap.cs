using System.Numerics;

namespace DataStructures;

public abstract class Heap<T> where T : INumber<T>
{
    private const int InitialSize = 8;

    private int _length = 0;

    private T[] _keys;

    #region Constructors

    public Heap(int size)
    {
        _keys = new T[size];
    }

    public Heap()
    {
        _keys = new T[InitialSize];
    }

    #endregion

    #region public

    public T this[int i]
    {
        get { return _keys[i - 1]; }
        protected set { _keys[i - 1] = value; }
    }

    public int Length { get => _length; }

    public T Extremum { get => _keys[0]; }

    public void Insert(T key)
    {
        if (_length == _keys.Length)
            IncreaseContainer();

        _length++;
        this[_length] = key;

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

    public T Extract()
    {
        var mimimum = _keys[0];

        (_keys[0], _keys[_length - 1]) = (_keys[_length - 1], _keys[0]);
        _length--;

        SiftDown(1);

        return mimimum;
    }

    #endregion

    #region Service methods

    private void IncreaseContainer()
    {
        T[] keys = new T[_length * 2];
        Array.Copy(_keys, 0, keys, 0, _keys.Length);
        _keys = keys;
    }

    protected int GetParentPosition(int childPosition)
    {
        if (_length == 1)
            return 1;

        return childPosition / 2;
    }

    protected abstract void SiftUp(int position);

    protected abstract void SiftDown(int position);

    #endregion
}
