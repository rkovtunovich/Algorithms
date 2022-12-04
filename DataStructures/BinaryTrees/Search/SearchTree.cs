using System.Numerics;

namespace DataStructures.BinaryTrees.Search;

public class SearchTree<TKey, TValue> : BinaryTree<TKey, TValue> where TKey : INumber<TKey>
{
    public TreeNode<TKey, TValue>? Mimimum => SearchMinimum(Root);

    public TreeNode<TKey, TValue>? Maximum => SearchMaximum(Root);

    public TreeNode<TKey, TValue>? Search(TKey key)
    {
        return SearchRecursively(Root, key);
    }

    #region Traversing

    public TreeNode<TKey, TValue>? GetPredecessor(TKey key)
    {
        var node = SearchRecursively(Root, key);
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
        var node = SearchRecursively(Root, key);
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

    private void TraverseInOrderRec(TreeNode<TKey, TValue>? node, Action<TreeNode<TKey, TValue>> action)
    {
        if (node is null)
            return;

        TraverseInOrderRec(node.LeftChild, action);

        action(node);

        TraverseInOrderRec(node.RightChild, action);
    }

    #endregion
}
