namespace DataStructures.Lists;

public class Deque<T>
{
    private T[] _items;

    private int _head;
    private int _tail;
    private int _capacity;
    private int _headCapacity;
    private int _tailCapacity;

    public Deque(int capacity = 4)
    {
        _capacity = capacity;
        _items = new T[_capacity];
        _head = 0;
        _tail = 0;
        _headCapacity = 0;
        _tailCapacity = 0;
    }

    public int Count => _tail - _head;

    public bool IsEmpty => Count is 0;

    public void AddFirst(T item)
    {
        if (_head is 0)
        {
            if (_tail == _capacity)           
                Resize();
            
            _head = _capacity;
            _headCapacity = _capacity;
            _capacity *= 2;
        }

        _items[--_head] = item;
    }

    public void AddLast(T item)
    {
        if (_tail == _capacity)
        {
            if (_head is 0)           
                Resize();
            
            _tail = 0;
            _tailCapacity = _capacity;
            _capacity *= 2;
        }

        _items[_tail++] = item;
    }

    public T RemoveFirst()
    {
        if (IsEmpty)       
            throw new InvalidOperationException("Deque is empty");
        
        var item = _items[_head];
        _items[_head++] = default!;

        if (_head == _headCapacity)
        {
            _head = 0;
            _headCapacity = 0;
        }

        return item;
    }

    public T RemoveLast()
    {
        if (IsEmpty)        
            throw new InvalidOperationException("Deque is empty");
        
        var item = _items[--_tail];
        _items[_tail] = default!;

        if (_tail == _tailCapacity)
        {
            _tail = _capacity;
            _tailCapacity = 0;
        }

        return item;
    }

    private void Resize()
    {
        var newItems = new T[_capacity];
        var newTail = 0;

        for (var i = _head; i < _tail; i++)       
            newItems[newTail++] = _items[i];
        
        _head = 0;
        _tail = newTail;
        _headCapacity = 0;
        _tailCapacity = 0;
        _items = newItems;
    }
}
