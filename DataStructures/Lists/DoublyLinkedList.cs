namespace DataStructures.Lists;

public class DoublyLinkedList<T> : ICollection<T>
{
    private DoublyListNode<T>? _head;
    private DoublyListNode<T>? _tail;

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public void Add(T value)
    {
        var node = new DoublyListNode<T>(value);
        if (_head == null)
        {
            _head = node;
            _tail = node;
        }
        else
        {
            node.Next = _head;
            _head.Prev = node;
            _head = node;
        }

        Count++;
    }

    public void AddLast(T value)
    {
        var node = new DoublyListNode<T>(value);
        if (_tail == null)
        {
            _head = node;
            _tail = node;
        }
        else
        {
            node.Prev = _tail;
            _tail.Next = node;
            _tail = node;
        }
        Count++;
    }

    public void RemoveFirst()
    {
        if (_head == null)
        {
            throw new InvalidOperationException("List is empty.");
        }
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        else
        {
            _head = _head.Next;
            _head.Prev = null;
        }
        Count--;
    }

    public void RemoveLast()
    {
        if (_tail == null)
        {
            throw new InvalidOperationException("List is empty.");
        }
        if (_head == _tail)
        {
            _head = null;
            _tail = null;
        }
        else
        {
            _tail = _tail.Prev;
            _tail.Next = null;
        }
        Count--;
    }

    public void Remove(DoublyListNode<T> node)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }
        if (node.Prev == null)
        {
            RemoveFirst();
        }
        else if (node.Next == null)
        {
            RemoveLast();
        }
        else
        {
            node.Prev.Next = node.Next;
            node.Next.Prev = node.Prev;
            Count--;
        }
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        for (int i = arrayIndex; i < array.Length; i++)
        {
            Add(array[i]);
            Count++;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}