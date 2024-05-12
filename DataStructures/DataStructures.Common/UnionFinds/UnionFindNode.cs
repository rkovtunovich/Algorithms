namespace DataStructures.Common.UnionFinds;

public class UnionFindNode<T>
{
    public int ParentIndex { get; set; }

    public int Size { get; set; }

    public T Item { get; set; }
}
