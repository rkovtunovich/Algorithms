using System.Numerics;

namespace DataStructures.SearchTrees;

public class SearchTree<TKey, TValue> where TKey : INumber<TKey>
{
    private TreeNode<TKey, TValue>? _root;

    public TreeNode<TKey, TValue>? Root => _root;

    public TreeNode<TKey, TValue>? Mimimum => SearchMinimum(Root);

    public TreeNode<TKey, TValue>? Maximum => SearchMaximum(Root);

    public TreeNode<TKey, TValue>? Search(TKey key)
    {
        return SearchRecursively(_root, key);
    }

    #region Modification

    public virtual void Insert(TKey key, TValue? value = default)
    {
        var newNode = CreateNode(key, value);

        if (Root is null)
        {
            SetRoot(newNode);
            return;
        }

        InsertRecursively(Root, newNode);
    }

    public virtual void Remove(TKey key)
    {
        var node = SearchRecursively(_root, key);
        if (node is null)
            return;

        DeleteNode(node);    
    }

    protected virtual TreeNode<TKey, TValue> CreateNode(TKey key, TValue? value)
    {
        return new() { 
            Key = key, 
            Value = value
        };
    }

    public void Clean()
    {
        _root = null;
    }

    #endregion

    #region Traversing

    public TreeNode<TKey, TValue>? GetPredecessor(TKey key)
    {
        var node = SearchRecursively(_root, key);
        if (node is null)
            return null;

        if (node.LeftChild is not null)
            return SearchMaximum(node.LeftChild);

        if (node.Parent?.RightChild == node)
            return node.Parent;
        else
            return node.Parent?.Parent;
    }

    public TreeNode<TKey, TValue>? GetSuccessor(TKey key)
    {
        var node = SearchRecursively(_root, key);
        if (node is null)
            return null;

        if (node.RightChild is not null)
            return SearchMinimum(node.RightChild);

        if (node.Parent?.LeftChild == node)
            return node.Parent;
        else
            return node.Parent?.Parent;
    }

    public void TraverseInOrder(Action<TreeNode<TKey, TValue>> action)
    {
        TraverseInOrderRec(Root, action);
    }

    #endregion

    #region Service methods

    #region Searching

    private TreeNode<TKey, TValue>? SearchRecursively(TreeNode<TKey, TValue>? node, TKey key)
    {
        if (node is null)
            return default;

        if (node.Key == key)
            return node;

        if (key <= node.Key)
            return SearchRecursively(node.LeftChild, key);
        else
            return SearchRecursively(node.RightChild, key);
    }

    private TreeNode<TKey, TValue>? SearchMinimum(TreeNode<TKey, TValue>? node)
    {
        if (node is null)
            return null;

        var min = node;

        if (min.LeftChild is null)
            return min;
        else
            return SearchMinimum(node.LeftChild);

    }

    private TreeNode<TKey, TValue>? SearchMaximum(TreeNode<TKey, TValue>? node)
    {
        if (node is null)
            return null;

        var min = node;

        if (min.RightChild is null)
            return min;
        else
            return SearchMaximum(node.RightChild);

    }

    #endregion

    private void TraverseInOrderRec(TreeNode<TKey, TValue>? node, Action<TreeNode<TKey, TValue>> action)
    {
        if (node is null)
            return;

        TraverseInOrderRec(node.LeftChild, action);
        
        action(node);
        
        TraverseInOrderRec(node.RightChild, action);
    }

    #region Modification

    private void InsertRecursively(TreeNode<TKey, TValue>? parent, TreeNode<TKey, TValue> child)
    {
        if (child.Key <= parent.Key)
        {
            if (!HasLeftChild(parent))
                AttachLeft(child, parent);
            else
                InsertRecursively(parent.LeftChild, child);
        }
        else
        {
            if (!HasRightChild(parent))
                AttachRight(child, parent);
            else
                InsertRecursively(parent.RightChild, child);
        }
    }

    protected void SetRoot(TreeNode<TKey, TValue> node)
    {
        node.Parent = null;
        _root = node;
    }

    private void Swap(TreeNode<TKey, TValue> source, TreeNode<TKey, TValue> target)
    {
        var targetParent = target.Parent;
        var targetLeftChild = target.LeftChild;
        var targetRightChild = target.RightChild;

        if (!HasParent(source))
            SetRoot(target);

        target.Parent = source.Parent;
        if (HasParent(target))
        {
            if (target.Parent.LeftChild == source)
                target.Parent.LeftChild = target;
            else
                target.Parent.RightChild = target;
        }

        source.Parent = targetParent;
        if (HasParent(source))
        {
            if (source.Parent.LeftChild == target)
                source.Parent.LeftChild = source;
            else
                source.Parent.RightChild = source;
        }

        target.LeftChild = source.LeftChild;
        if (HasLeftChild(target))
            target.LeftChild.Parent = target;

        target.RightChild = source.RightChild;
        if (HasRightChild(target))
            target.RightChild.Parent = target;

        source.LeftChild = targetLeftChild;
        if (HasLeftChild(source))
            source.LeftChild.Parent = source;

        source.RightChild = targetRightChild;
        if (HasRightChild(source))
            source.RightChild.Parent = source;
    }

    protected void Detach(TreeNode<TKey, TValue> node)
    {
        if (node.Parent is null)
            return;

        if (IsLeftChild(node))
            node.Parent.LeftChild = null;
        else
            node.Parent.RightChild = null;

        node.Parent = null;
    }

    protected void AttachLeft(TreeNode<TKey, TValue> child, TreeNode<TKey, TValue> parent)
    {
        child.Parent = parent;
        parent.LeftChild = child;
    }

    protected void AttachRight(TreeNode<TKey, TValue> child, TreeNode<TKey, TValue> parent)
    {
        child.Parent = parent;
        parent.RightChild = child;
    }

    protected void DeleteNode(TreeNode<TKey, TValue> node)
    {
        // node is leaf
        if (IsLeaf(node))
        {
            Detach(node);

            if (node == Root)
                Clean();

            return;
        }

        // node has one child (right)
        if (!HasLeftChild(node))
        {
            var next = node.RightChild;
            var parent = node.Parent;
            bool isLeft = IsLeftChild(node);

            Detach(node);
            //if (parent is not null)
            //    AttachRight(next, parent);

            if (isLeft)
                AttachLeft(next, parent);
            else
                AttachRight(next, parent);

            if (node == Root)
                SetRoot(next);

            return;
        }

        // node has one child (left)
        if (!HasRightChild(node))
        {
            var next = node.LeftChild;
            var parent = node.Parent;
            bool isLeft = IsLeftChild(node);

            Detach(node);
            //if (parent is not null)
            //    AttachLeft(next, parent);

            if (isLeft)
                AttachLeft(next, parent);
            else
                AttachRight(next, parent);

            if (node == Root)
                SetRoot(next);

            return;
        }

        // node has two children
        var maxLeftChild = SearchMaximum(node.LeftChild);
        Swap(node, maxLeftChild);

        if (IsLeaf(node))
        {
            Detach(node);
        }
        else
        {
            var next = node.LeftChild;
            var parent = node.Parent;
            bool isLeft = IsLeftChild(node);

            Detach(node);
            if (isLeft)
                AttachLeft(next, parent);
            else
                AttachRight(next, parent);
        }
    }

    #endregion

    protected bool IsLeaf(TreeNode<TKey, TValue> node)
    {
        return node.LeftChild is null && node.RightChild is null;
    }

    protected bool IsLeftChild(TreeNode<TKey, TValue> node)
    {
        if (node.Parent is null)
            throw new Exception("This node hasn't parent");

        return node.Parent.LeftChild == node;
    }

    protected bool IsRightChild(TreeNode<TKey, TValue> node)
    {
        if (node.Parent is null)
            throw new Exception("This node hasn't parent");

        return node.Parent.RightChild == node;
    }

    protected bool HasLeftChild(TreeNode<TKey, TValue> node)
    {
        return node.LeftChild is not null;
    }

    protected bool HasRightChild(TreeNode<TKey, TValue> node)
    {
        return node.RightChild is not null;
    }

    protected bool HasParent(TreeNode<TKey, TValue> node)
    {
        return node.Parent is not null;
    }

    #endregion
}
