namespace DataStructures.Heaps;

public struct HeapNode<TKey, TValue> where TKey : notnull
{
    public TKey Key { get; internal set; }

    public TValue? Value { get; internal set; }
}
