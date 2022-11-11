using System.Numerics;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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

    public void Insert(TKey key, TValue? value = default)
    {
        if (_root is null)
        {
            _root = new() { Key = key, Value = value };
            return;
        }

        InsertRecursively(_root, key, value);
    }

    public void Remove(TKey key)
    {
        var node = SearchRecursively(_root, key);
        if (node is null) 
            return;     

        // node is leaf
        if(IsLeaf(node))
        {
            Detach(node);

            if (node == Root)
                _root = null;

            return;
        }

        // node has one child (right)
        if(node.LeftChild is null)
        {
            var next = node.RightChild;
            var parent = node.Parent;

            Detach(node);
            if(parent is not null)
                AttacRight(next, parent);

            if (node == Root)
                _root = next;
            
            return;
        }

        // node has one child (left)
        if (node.RightChild is null)
        {
            var next = node.LeftChild;
            var parent = node.Parent;
            
            Detach(node);         
            if(parent is not null)
                AttachLeft(next, parent);

            if (node == Root)
                _root = next;

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
                AttacRight(next, parent);
        }
    }   

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

    #region Service methods

    public void InsertRecursively(TreeNode<TKey, TValue>? node, TKey key, TValue? value = default)
    {
        if (key <= node.Key)
        {
            if (node.LeftChild is null)
            {
                node.LeftChild = new() { Key = key, Value = value, Parent = node };
                return;
            }
            else
            {
                InsertRecursively(node.LeftChild, key, value);
            }
        }
        else
        {
            if (node.RightChild is null)
            {
                node.RightChild = new() { Key = key, Value = value, Parent = node };
                return;
            }
            else
            {
                InsertRecursively(node.RightChild, key, value);
            }
        }
    }

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

    private bool IsLeaf(TreeNode<TKey, TValue> node)
    {
        return node.LeftChild is null && node.RightChild is null;
    }

    private void Swap(TreeNode<TKey, TValue> source, TreeNode<TKey, TValue> target)
    {
        var targetParent = target.Parent;
        var targetLeftChild = target.LeftChild;
        var targetRightChild = target.RightChild;

        if (source.Parent is null)
            _root = target;

        target.Parent = source.Parent;
        if(target.Parent is not null)
        {
            if (target.Parent.LeftChild == source)
                target.Parent.LeftChild = target;
            else
                target.Parent.RightChild = target;
        }

        source.Parent = targetParent;
        if (source.Parent is not null)
        {
            if (source.Parent.LeftChild == target)
                source.Parent.LeftChild = source;
            else
                source.Parent.RightChild = source;
        }

        target.LeftChild = source.LeftChild;
        if(target.LeftChild is not null)
            target.LeftChild.Parent = target;
        
        target.RightChild = source.RightChild;
        if(target.RightChild is not null)
            target.RightChild.Parent = target;

        source.LeftChild = targetLeftChild;
        if (source.LeftChild is not null)
            source.LeftChild.Parent = source;

        source.RightChild = targetRightChild;
        if (source.RightChild is not null)
            source.RightChild.Parent = source;
    }

    private void Detach(TreeNode<TKey, TValue> node)
    {
        if (node.Parent is null)
            return;

        if (IsLeftChild(node))
            node.Parent.LeftChild = null;
        else
            node.Parent.RightChild = null;

        node.Parent = null;
    }

    private void AttachLeft(TreeNode<TKey, TValue> sourse, TreeNode<TKey, TValue> destination)
    {
        sourse.Parent = destination;
        destination.LeftChild = sourse;
    }

    private void AttacRight(TreeNode<TKey, TValue> sourse, TreeNode<TKey, TValue> destination)
    {
        sourse.Parent = destination;
        destination.RightChild = sourse;
    }

    private bool IsLeftChild(TreeNode<TKey, TValue> node)
    {
        if (node.Parent is null)
            throw new Exception("This node hasn't parent");

        return node.Parent.LeftChild == node;
    }

    #endregion
}
