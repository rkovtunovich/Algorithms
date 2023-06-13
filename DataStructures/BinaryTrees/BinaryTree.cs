namespace DataStructures.BinaryTrees;

// A binary tree is a tree-like model of data where each node can have at most two children, referred to as the left child and the right child. 
// 1. **Structure**: Each node in a binary tree contains a data element, and two pointers to other nodes.
//      The left pointer points to the left child and the right pointer points to the right child.
//      If a child does not exist, the pointer is null.
// 
// 2. **Uses**: Binary trees are used in many areas of computer science including:
//    - **Binary Search Trees (BST)**: A special kind of binary tree where each node has a value greater than all the values in its left subtree and less than all the values in its right subtree.
//        BSTs are used for creating databases and file systems due to their efficient search capability.
//    - **Heap**: A special tree structure in which each parent node is less than or equal to its child node.
//        It is used in Heap Sort and Priority Queue.
//    - **Syntax Trees**: Used in compilers to represent syntax of programming languages.
//    - **Huffman Coding Trees**: Used in compression algorithms.
// 
// 3. **Efficiency**: The efficiency of operations on a binary tree depends on the type of binary tree and its balance.
//      For a binary search tree, if the tree is balanced (i.e., the left and right subtrees of every node differ in height by at most one),
//      then operations like insertion, deletion, and lookup can be performed in O(log n) time, where n is the number of nodes in the tree.
//      However, in the worst case (when the tree is completely unbalanced), these operations can take O(n) time.
// 
// 4. **Traversal**: Binary trees can be traversed in different ways - in-order, pre-order, and post-order.
//      In-order traversal first visits the left subtree, then the root, and finally the right subtree.
//      Pre-order traversal first visits the root, then the left subtree, and finally the right subtree.
//      Post-order traversal first visits the left subtree, then the right subtree, and finally the root.
// 
// 5. **Types**: There are several types of binary trees, including full (every node has 0 or 2 children),
//      complete (all levels are fully filled except possibly for the last level, which is filled from left to right), and perfect (all levels are fully filled).
// 
// 6. **Space Complexity**: The space complexity of a binary tree is O(n), where n is the number of nodes in the tree.
//      This is because we need to store each node in the tree.

public class BinaryTree<TKey, TValue> where TKey : INumber<TKey>
{
    private TreeNode<TKey, TValue>? _root;

    public TreeNode<TKey, TValue>? Root => _root;

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

    public void AttachLeft(TreeNode<TKey, TValue> child, TreeNode<TKey, TValue> parent)
    {
        child.Parent = parent;
        parent.LeftChild = child;
    }

    public void AttachRight(TreeNode<TKey, TValue> child, TreeNode<TKey, TValue> parent)
    {
        child.Parent = parent;
        parent.RightChild = child;
    }

    protected virtual TreeNode<TKey, TValue> CreateNode(TKey key, TValue? value)
    {
        return new()
        {
            Key = key,
            Value = value
        };
    }

    public void Clean()
    {
        _root = null;
    }

    #endregion

    #region Service methods

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

    protected void Swap(TreeNode<TKey, TValue> source, TreeNode<TKey, TValue> target)
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

    #region Searching

    protected virtual TreeNode<TKey, TValue>? SearchRecursively(TreeNode<TKey, TValue>? node, TKey key)
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

    protected virtual TreeNode<TKey, TValue>? SearchMinimum(TreeNode<TKey, TValue>? node)
    {
        if (node is null)
            return null;

        var min = node;

        if (min.LeftChild is null)
            return min;
        else
            return SearchMinimum(node.LeftChild);

    }

    protected virtual TreeNode<TKey, TValue>? SearchMaximum(TreeNode<TKey, TValue>? node)
    {
        if (node is null)
            return null;

        var min = node;

        if (min.RightChild is null)
            return min;
        else
            return SearchMaximum(node.RightChild);

    }

    protected virtual TreeNode<TKey, TValue>? GetBrother(TreeNode<TKey, TValue> node)
    {
        if (node.IsRoot)
            return null;

        if (node.IsLeftChild)
            return node?.Parent?.RightChild;
        else
            return node?.Parent?.LeftChild;
    }

    protected virtual TreeNode<TKey, TValue>? GetUncle(TreeNode<TKey, TValue> node)
    {
        if (!node.HasParent)
            return null;

        if (node.Parent.IsLeftChild)
            return node?.Parent?.Parent?.RightChild;
        else
            return node?.Parent?.Parent?.LeftChild;
    }

    #endregion

    #region Cheks

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

    #endregion
}
