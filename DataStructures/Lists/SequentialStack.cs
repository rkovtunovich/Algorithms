namespace DataStructures.Lists;

public class SequentialStack<T>
{
    private T[] _items;
    private int _top;

    public SequentialStack(int capacity)
    {
        _items = new T[capacity];
        _top = -1;
    }

    public void Push(T item)
    {
        if (_top == _items.Length - 1)
            throw new InvalidOperationException("Stack is full.");

        _top++;
        _items[_top] = item;
    }

    public T Pop()
    {
        if (_top == -1)
            throw new InvalidOperationException("Stack is empty.");

        T item = _items[_top];
        _top--;

        return item;
    }

    public T Peek()
    {
        if (_top == -1)
            throw new InvalidOperationException("Stack is empty.");

        return _items[_top];
    }

    public int Count
    {
        get { return _top + 1; }
    }
}

