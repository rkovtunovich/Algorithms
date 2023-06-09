namespace DataStructures.BinaryTrees.Search.RedBlack;

public class RedBlackTreeNode<TKey, TValue> : TreeNode<TKey, TValue> where TKey : INumber<TKey>
{
    public Color Color { get; set; }

    public bool IsRed => Color == Color.Red;

    public bool IsBlack => Color == Color.Black;

    public RedBlackTreeNode<TKey, TValue> GetParent() => Parent as RedBlackTreeNode<TKey, TValue>
                                                         ?? throw new Exception("It isn't reb-black tree");
}