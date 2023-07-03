namespace DataStructures.Common.BinaryTrees.Search.RedBlack;

public class RedBlackTreeNode<TKey, TValue> : TreeNode<TKey, TValue> where TKey : INumber<TKey>
{
    public override RedBlackTreeNode<TKey, TValue>? Parent { get => base.Parent as RedBlackTreeNode<TKey, TValue>; }

    public override RedBlackTreeNode<TKey, TValue>? LeftChild { get => base.LeftChild as RedBlackTreeNode<TKey, TValue>; }

    public override RedBlackTreeNode<TKey, TValue>? RightChild { get => base.RightChild as RedBlackTreeNode<TKey, TValue>; }

    public Color Color { get; set; }

    public bool IsRed => Color == Color.Red;

    public bool IsBlack => Color == Color.Black;

    public override RedBlackTreeNode<TKey, TValue>? Brother => base.Brother as RedBlackTreeNode<TKey, TValue>;
}