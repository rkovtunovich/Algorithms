using DataStructures.Lists;
using System.Numerics;

namespace DataStructures.BinaryTrees.Search;

public class AVLTree<TKey, TValue> : SearchTree<TKey, TValue> where TKey : INumber<TKey>
{
    public override void Insert(TKey key, TValue? value = default)
    {
        var newNode = CreateNode(key, value) as AVLTreeNode<TKey, TValue>;

        if (Root is null)
        {
            SetRoot(newNode);
            return;
        }

        InsertRecursively(Root as AVLTreeNode<TKey, TValue>, newNode);
    }

    public override void Remove(TKey key)
    {
        if (Root is null)
            return;

        RemoveRecursively(Root as AVLTreeNode<TKey, TValue>, key);

        ActualizeHeight(Root);
        if (HasLeftChild(Root))
            ActualizeHeight(Root.LeftChild);
        if (HasRightChild(Root))
            ActualizeHeight(Root.RightChild);

        var nonBalancedChild = GetNonBalancedNode(Root as AVLTreeNode<TKey, TValue>);
        if (nonBalancedChild is not null)
        {
            Balance(Root as AVLTreeNode<TKey, TValue>, nonBalancedChild);
            ActualizeHeight(Root);
        }
    }

    protected override AVLTreeNode<TKey, TValue> CreateNode(TKey key, TValue? value)
    {
        return new AVLTreeNode<TKey, TValue>()
        {
            Key = key,
            Value = value,
            Hight = 1
        };
    }

    #region Service methods

    #region Rotation

    private void RotateLeft(AVLTreeNode<TKey, TValue> parent, AVLTreeNode<TKey, TValue> child)
    {
        if (HasLeftChild(child))
            AttachRight(child.LeftChild, parent);
        else
            Detach(child);

        if (!HasParent(parent))
            SetRoot(child);
        else if (IsLeftChild(parent))
            AttachLeft(child, parent.Parent);
        else
            AttachRight(child, parent.Parent);

        AttachLeft(parent, child);
    }

    private void RotateRight(AVLTreeNode<TKey, TValue> parent, AVLTreeNode<TKey, TValue> child)
    {
        if (HasRightChild(child))
            AttachLeft(child.RightChild, parent);
        else
            Detach(child);

        if (!HasParent(parent))
            SetRoot(child);
        else if (IsRightChild(parent))
            AttachRight(child, parent.Parent);
        else
            AttachLeft(child, parent.Parent);

        AttachRight(parent, child);
    }

    private void RotateLeftRight(AVLTreeNode<TKey, TValue> parent, AVLTreeNode<TKey, TValue> child)
    {
        RotateLeft(child, child.RightChild as AVLTreeNode<TKey, TValue>);

        ActualizeHeight(child);
        ActualizeHeight(parent.LeftChild as AVLTreeNode<TKey, TValue>);

        RotateRight(parent, parent.LeftChild as AVLTreeNode<TKey, TValue>);
        ActualizeHeight(parent);
    }

    private void RotateRightLeft(AVLTreeNode<TKey, TValue> parent, AVLTreeNode<TKey, TValue> child)
    {
        RotateRight(child, child.LeftChild as AVLTreeNode<TKey, TValue>);

        ActualizeHeight(child);
        ActualizeHeight(parent.RightChild as AVLTreeNode<TKey, TValue>);

        RotateLeft(parent, parent.RightChild as AVLTreeNode<TKey, TValue>);
        ActualizeHeight(parent);
    }

    private void ActualizeHeight(TreeNode<TKey, TValue> node)
    {
        (node as AVLTreeNode<TKey, TValue>).Hight = Math.Max(GetHight(node.LeftChild), GetHight(node.RightChild)) + 1;
    }

    #endregion

    #region Balance

    private int GetBalanceFactor(AVLTreeNode<TKey, TValue> node)
    {
        return ((node.LeftChild as AVLTreeNode<TKey, TValue>)?.Hight ?? 0) - ((node.RightChild as AVLTreeNode<TKey, TValue>)?.Hight ?? 0);
    }

    private void Balance(AVLTreeNode<TKey, TValue> parent, AVLTreeNode<TKey, TValue> child)
    {
        int balanceFactor = GetBalanceFactor(parent);

        if (balanceFactor > 1)
        {
            if (child.Key <= parent.LeftChild.Key)
                RotateRight(parent, parent.LeftChild as AVLTreeNode<TKey, TValue>);
            else
                RotateLeftRight(parent, parent.LeftChild as AVLTreeNode<TKey, TValue>);
        }
        else if (balanceFactor < -1)
        {
            if (child.Key > parent.RightChild.Key)
                RotateLeft(parent, parent.RightChild as AVLTreeNode<TKey, TValue>);
            else
                RotateRightLeft(parent, parent.RightChild as AVLTreeNode<TKey, TValue>);
        }

        ActualizeHeight(parent);
    }

    private int GetHight(TreeNode<TKey, TValue>? node)
    {
        return (node as AVLTreeNode<TKey, TValue>)?.Hight ?? 0;
    }

    private AVLTreeNode<TKey, TValue>? GetNonBalancedNode(AVLTreeNode<TKey, TValue> parent)
    {
        int balanceFactor = GetBalanceFactor(parent);

        if (balanceFactor > 1)
            return parent.LeftChild as AVLTreeNode<TKey, TValue>;
        else if (balanceFactor < -1)
            return parent.RightChild as AVLTreeNode<TKey, TValue>;
        else
            return null;
    }

    #endregion

    private void InsertRecursively(AVLTreeNode<TKey, TValue>? parent, AVLTreeNode<TKey, TValue> child)
    {
        if (child.Key <= parent.Key)
        {
            if (!HasLeftChild(parent))
                AttachLeft(child, parent);
            else
                InsertRecursively(parent.LeftChild as AVLTreeNode<TKey, TValue>, child);
        }
        else
        {
            if (!HasRightChild(parent))
                AttachRight(child, parent);
            else
                InsertRecursively(parent.RightChild as AVLTreeNode<TKey, TValue>, child);
        }

        ActualizeHeight(parent);

        Balance(parent, child);
    }

    private void RemoveRecursively(AVLTreeNode<TKey, TValue>? node, TKey key)
    {
        if (node is null)
            return;

        if (key < node.Key)
        {
            RemoveRecursively(node.LeftChild as AVLTreeNode<TKey, TValue>, key);
            ActualizeHeight(node);

            if (HasLeftChild(node))
            {
                ActualizeHeight(node.LeftChild as AVLTreeNode<TKey, TValue>);
                Balance(node, node.LeftChild as AVLTreeNode<TKey, TValue>);
            }
        }
        else if (key > node.Key)
        {
            RemoveRecursively(node.RightChild as AVLTreeNode<TKey, TValue>, key);
            ActualizeHeight(node);

            if (HasRightChild(node))
            {
                ActualizeHeight(node.RightChild as AVLTreeNode<TKey, TValue>);
                Balance(node, node.RightChild as AVLTreeNode<TKey, TValue>);
            }
        }
        else
            DeleteNode(node);
    }

    #endregion
}
