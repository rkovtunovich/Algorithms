namespace DataStructures.Lists;

public class ListNode<T>
{
    public ListNode<T>? Next { get; set; }

    public T? Value { get; set; }

    public ListNode(T? value)
    {
        Value = value;
    }
}
