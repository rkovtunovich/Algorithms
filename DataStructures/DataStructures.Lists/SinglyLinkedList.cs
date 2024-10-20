namespace DataStructures.Lists;

public class SinglyLinkedList<T> : ICollection<T>
{
    public ListNode<T>? Head { get; private set; }

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public bool IsEmpty => Head is null;

    public void Add(T value)
    {
        var node = new ListNode<T>(value);
        if (IsEmpty)
        {
            Head = node;
        }
        else
        {
            node.Next = Head;
            Head = node;
        }

        Count++;
    }

    public ListNode<T>? Find(T value)
    {
        var current = Head;
        while (current is not null)
        {
            if (current.Value!.Equals(value))
                return current;

            current = current.Next;
        }

        return null;
    }

    public void RemoveFirst()
    {
        if (IsEmpty)
            throw new InvalidOperationException("List is empty.");
        else
            Head = Head.Next;

        Count--;
    }

    public void Remove(ListNode<T> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (Head is null)
            throw new InvalidOperationException("List is empty.");


        if (node == Head)
        {
            Head = Head.Next;
            Count--;
            return;
        }

        var current = Head;
        while (current is not null)
        {
            if (current.Next == node)
            {
                current.Next = node.Next;
                Count--;
                return;
            }

            current = current.Next;
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
        Count = 0;
    }

    public void Reverse()
    {
        if (IsEmpty)
            return;

        ListNode<T>? previous = null;
        var current = Head;

        while (current is not null)
        {
            var next = current.Next;
            current.Next = previous;
            previous = current;
            current = next;
        }

        Head = previous;
    }

    public bool Contains(T item)
    {
        return Find(item) is not null;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (arrayIndex < 0 || arrayIndex >= array.Length)
            throw new IndexOutOfRangeException();

        if (array.Length - arrayIndex < Count)
            throw new InvalidOperationException("Array is not large enough.");

        var current = Head;
        while (current is not null)
        {
            array[arrayIndex++] = current.Value!;
            current = current.Next;
        }
    }

    public void UnionWith(SinglyLinkedList<T> other)
    {
        if (other.IsEmpty)
            return;

        if (IsEmpty)
        {
            Head = other.Head;
            Count = other.Count;
            return;
        }

        var current = Head;
        while (current.Next is not null)
        {
            current = current.Next;      
        }

        current.Next = other.Head;
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

        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}