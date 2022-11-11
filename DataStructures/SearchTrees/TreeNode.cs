using System.Numerics;

namespace DataStructures.SearchTrees;

public class TreeNode<TKey, TValue> where TKey: INumber<TKey>  
{
    public TreeNode<TKey, TValue>? Parent { get; set; }

    public TreeNode<TKey, TValue>? LeftChild { get; set; }

    public TreeNode<TKey, TValue>? RightChild { get; set;}

    public required TKey Key { get; set; }

    public TValue? Value { get; set; }
}
