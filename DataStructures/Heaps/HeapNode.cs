namespace DataStructures.Heaps;

public struct HeapNode<TKey, TValue> where TKey : INumber<TKey>
{
    public TKey Key { get; internal set; }

    public TValue? Value { get; internal set; }
}
