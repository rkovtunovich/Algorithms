namespace DataStructures.Lists;

public class SequentialQueue<T>
{
    private T[] _array;
    private int _head;
    private int _tail;

    public SequentialQueue(int capacity)
    {
        _array = new T[capacity];
        _head = 0;
        _tail = 0;
    }

    public void Enqueue(T item)
    {
        if (_tail == _array.Length)
            throw new InvalidOperationException("Queue is full.");

        _array[_tail] = item;
        _tail++;
    }

    public T Dequeue()
    {
        if (_head == _tail)
            throw new InvalidOperationException("Queue is empty.");

        T item = _array[_head];
        _head++;
        return item;
    }

    public T Peek()
    {
        if (_head == _tail)
            throw new InvalidOperationException("Queue is empty.");

        return _array[_head];
    }

    public int Count
    {
        get { return _tail - _head; }
    }
}
