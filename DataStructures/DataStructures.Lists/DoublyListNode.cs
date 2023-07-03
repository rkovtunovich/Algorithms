namespace DataStructures.Lists;

public class DoublyListNode<T> : ListNode<T>
{
    public DoublyListNode(T? value) : base(value)
    {
    }

    public DoublyListNode<T>? Prev { get; set; }

    public new DoublyListNode<T>? Next { get; set; }
}
