namespace DataStructures.Lists;

public class SequentialList<T> : IList<T>
{
    private const int DefaultInitialCapacity = 4;

    private T[] _items;
    private int _count = 0;

    public SequentialList(int capacity)
    {
        _items = new T[capacity];
    }

    public SequentialList()
    {
        _items = new T[DefaultInitialCapacity];
    }

    public int Count
    {
        get { return _count; }
    }

    public bool IsReadOnly => false;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException("Index out of range.");
            
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _count)       
                throw new IndexOutOfRangeException("Index out of range.");
            
            _items[index] = value;
        }
    }

    public void Add(T item)
    {
        if (_count == _items.Length)       
            Array.Resize(ref _items, _items.Length * 2);
        
        _items[_count] = item;
        _count++;
    }

    public void AddRange<TKey>(IList<T> items) 
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));
        
        if (_count + items.Count > _items.Length)       
            Array.Resize(ref _items, (_count + items.Count) * 2);
        
        foreach (var item in items)
        {
            _items[_count] = item;
            _count++;
        }
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > _count)    
            throw new IndexOutOfRangeException("Index out of range.");
        
        if (_count == _items.Length)     
            Array.Resize(ref _items, _items.Length * 2);
        
        for (int i = _count; i > index; i--)
        {
            _items[i] = _items[i - 1];
        }

        _items[index] = item;
        _count++;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _count)    
            throw new IndexOutOfRangeException("Index out of range.");
        
        for (int i = index; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        _count--;
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_items[i]?.Equals(item) ?? false)
                return i;
        }

        return -1;
    }

    public void Clear()
    {
        _count = 0;
    }

    public bool Contains(T item)
    {
        return IndexOf(item) is not -1;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        for (int i = 0; i < Count; i++)
        {
            array[arrayIndex++] = _items[i];
        }
    }

    public bool Remove(T item)
    {
        var index = IndexOf(item);

        if (index is -1)
            return false;

        for (int i = index; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }

        _count--;

        return true;
    }

    public void RemoveRange(int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= _count)
            throw new IndexOutOfRangeException(nameof(startIndex));
        if (count < 0 || startIndex + count > _count)
            throw new ArgumentException(nameof(count));

        for (int i = startIndex + count; i < _count; i++)
        {
            _items[i - count] = _items[i];
            _items[i] = default;
        }

        _count -= count;
        if (_count <= _items.Length / 2)       
            Array.Resize(ref _items, _items.Length / 2);
        
    }

    public void Sort()
    {
        Array.Sort(_items, 0, _count);
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return _items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return _items[i];
        }
    }
}
