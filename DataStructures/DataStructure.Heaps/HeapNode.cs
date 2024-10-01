namespace DataStructures.Heaps;

public struct HeapNode<TKey, TValue>(TKey key, TValue? value) where TKey : notnull
{
    public TKey Key { get; internal set; } = key;

    public TValue? Value { get; internal set; } = value;
}
