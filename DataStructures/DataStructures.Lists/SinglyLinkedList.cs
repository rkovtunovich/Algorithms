namespace DataStructures.Lists;

public class SinglyLinkedList<T> : ICollection<T>
{
    private ListNode<T>? _head;

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public void Add(T value)
    {
        var node = new ListNode<T>(value);
        if (_head is null)
        {
            _head = node;
        }
        else
        {
            node.Next = _head;
            _head = node;
        }

        Count++;
    }

    public void RemoveFirst()
    {
        if (_head == null)
            throw new InvalidOperationException("List is empty.");
        else
            _head = _head.Next;

        Count--;
    }

    public void Remove(ListNode<T> node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));

        var current = _head;

        while (true)
        {
            if (current == node)
            {

            }
            else
            {

            }
        }
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        _head = null;
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