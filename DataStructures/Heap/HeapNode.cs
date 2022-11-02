using System.Numerics;

namespace DataStructures.Heap;

public struct HeapNode<Tkey, TValue> where Tkey: INumber<Tkey>
{
    public Tkey Key { get; internal set; }

    public TValue? Value { get; internal set; }
}
