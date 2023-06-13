namespace DataStructures.BinaryTrees;

public class TreeNode<TKey, TValue> where TKey : INumber<TKey>
{
    public virtual TreeNode<TKey, TValue>? Parent { get; set; }

    public virtual TreeNode<TKey, TValue>? LeftChild { get; set; }

    public virtual TreeNode<TKey, TValue>? RightChild { get; set; }

    public required TKey Key { get; set; }

    public TValue? Value { get; set; }

    public bool IsLeaf => LeftChild is null && RightChild is null;

    public bool IsLeftChild => HasParent && Parent?.LeftChild == this;

    public bool IsRightChild => HasParent && Parent?.RightChild == this;

    public bool HasParent => Parent != null;

    public bool HasLeftChild => LeftChild != null;

    public bool HasRightChild => RightChild != null;

    public bool HasBothChildren => HasLeftChild && HasRightChild;

    public bool HasAnyChild => HasLeftChild || HasRightChild;

    public bool IsRoot => !HasParent;

    public bool HasParentAsRoot => HasParent && !Parent!.HasParent;

    public virtual TreeNode<TKey, TValue>? AnyBrother
    {
        get
        {
            if (IsRoot)
                return null;

            if (IsLeftChild)
                return Parent?.RightChild;
            else
                return Parent?.LeftChild;
        }
    }
}
