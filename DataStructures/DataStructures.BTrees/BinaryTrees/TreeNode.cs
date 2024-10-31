namespace DataStructures.Trees.BinaryTrees;

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

    public bool HasParent => Parent is not null;

    public bool HasLeftChild => LeftChild is not null;

    public bool HasRightChild => RightChild is not null;

    public bool HasBothChildren => HasLeftChild && HasRightChild;

    public bool HasAnyChild => HasLeftChild || HasRightChild;

    public bool IsRoot => !HasParent;

    public bool HasParentAsRoot => HasParent && !Parent!.HasParent;

    public virtual TreeNode<TKey, TValue>? Brother
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

    public void AttachLeft(TreeNode<TKey, TValue> child)
    {
        LeftChild = child;
        child.Parent = this;
    }

    public void AttachRight(TreeNode<TKey, TValue> child)
    {
        RightChild = child;
        child.Parent = this;
    }
}
