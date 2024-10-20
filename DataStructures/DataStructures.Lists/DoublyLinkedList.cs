namespace DataStructures.Lists;

public class DoublyLinkedList<T> : ICollection<T>
{
    public DoublyListNode<T>? Head { get; private set; }

    public DoublyListNode<T>? Tail { get; private set; }

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public void Add(T value)
    {
        var node = new DoublyListNode<T>(value);
        if (Head is null)
        {
            Head = node;
            Tail = node;
        }
        else
        {
            node.Next = Head;
            Head.Prev = node;
            Head = node;
        }

        Count++;
    }

    public void AddLast(T value)
    {
        var node = new DoublyListNode<T>(value);
        if (Tail is null)
        {
            Head = node;
            Tail = node;
        }
        else
        {
            node.Prev = Tail;
            Tail.Next = node;
            Tail = node;
        }
        Count++;
    }

    public void RemoveFirst()
    {
        if (Head is null)       
            throw new InvalidOperationException("List is empty.");
        
        if (Head == Tail)
        {
            Head = null;
            Tail = null;
        }
        else
        {
            Head = Head.Next;
            Head.Prev = null;
        }
        Count--;
    }

    public void RemoveLast()
    {
        if (Tail is null)
            throw new InvalidOperationException("List is empty.");
        
        if (Head == Tail)
        {
            Head = null;
            Tail = null;
        }
        else
        {
            Tail = Tail.Prev;
            Tail.Next = null;
        }

        Count--;
    }

    public void Remove(DoublyListNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node.Prev is null)
        {
            RemoveFirst();
        }
        else if (node.Next is null)
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
        var node = Find(item);
        if (node is null)
            return false;

        Remove(node);

        return true;
    }

    public void Clear()
    {
        Head = null;
        Tail = null;
    }

    public DoublyListNode<T>? Find(T item)
    {
        var current = Head;
        while (current is not null)
        {
            if (current.Value!.Equals(item))
                return current;

            current = current.Next;
        }

        return null;
    }

    public bool Contains(T item)
    {
        return Find(item) is not null;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        for (int i = arrayIndex; i < array.Length; i++)
        {
            Add(array[i]);
            Count++;
        }
    }

    public void UnionWith(DoublyLinkedList<T> other)
    {
        if (other is null)
            return;

        Tail.Next = other.Head;
        other.Head.Prev = Tail;
        Tail = other.Tail;

        Count += other.Count;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = Head;
        while (current is not null)
        {
            yield return current.Value!;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}