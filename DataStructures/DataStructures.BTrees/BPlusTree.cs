namespace DataStructures.BTrees;

public class BPlusTree<TKey, TValue> : BTree<TKey> where TKey : INumber<TKey>
{
    public BPlusTree(int order) : base(order)
    {
    }

    public override BPlusTreeNode<TKey, TValue>? Search(TKey key)
    {
        if (Root is null)
            return null;

        var leafNode = FindLeafNode(Root, key);
        return leafNode.ContainsKey(key) ? leafNode : null;
    }

    public override void Insert(TKey key)
    {
        if (Root is null)
        {
            Root = new BPlusTreeNode<TKey, TValue>(key);
            return;
        }

        var leafNode = FindLeafNode(Root, key);
        leafNode.AddKey(key);
        if (leafNode.IsOverflow(Order))
        {
            var newNode = SplitNode(leafNode);
            UpdateLeavesLinksInsertion(leafNode, newNode);
            var newNodeKey = newNode.Keys.Min();
            InsertParentNode(leafNode, newNode, newNodeKey);
        }
    }

    private void UpdateLeavesLinksInsertion(BPlusTreeNode<TKey, TValue> leafNode, BPlusTreeNode<TKey, TValue> newNode)
    {
        if (leafNode.Right is not null)
            leafNode.Right.Left = newNode;

        newNode.Right = leafNode.Right;
        leafNode.Right = newNode;
        newNode.Left = leafNode;
    }

    private void InsertParentNode(BPlusTreeNode<TKey, TValue> leafNode, BPlusTreeNode<TKey, TValue> newNode, TKey? newNodeKey)
    {
        if (leafNode.Parent is null)
        {
            var newRoot = new BPlusTreeNode<TKey, TValue>(newNodeKey);
            newRoot.Attach(leafNode);
            newRoot.Attach(newNode);
            Root = newRoot;
            return;
        }

        var parentNode = leafNode.Parent;
        parentNode.AddKey(newNodeKey);
        parentNode.Attach(newNode);
        if (parentNode.IsOverflow(Order))
        {
            var newParentNode = SplitNode(parentNode);
            var newParentNodeKey = newParentNode.ExtractFirsKey();
            InsertParentNode(parentNode, newParentNode, newParentNodeKey);
        }
    }

    private BPlusTreeNode<TKey, TValue> SplitNode(BPlusTreeNode<TKey, TValue> node)
    {
        var newNode = new BPlusTreeNode<TKey, TValue>();
        var keysLength = node.Keys.Count;
        var keysToMove = Math.Ceiling((double)keysLength / 2);
        while (keysToMove > 0)
        {
            var key = node.Keys[^1];
            node.RemoveKey(key);
            newNode.AddKey(key);

            // Move values for leaf nodes
            if (node.Values is not null)
            {
                var value = node.ExtractLastValue();
                newNode.AddValue(value);
            }

            // Move children for non-leaf nodes
            if (node.Children is not null)
            {
                var child = node.ExtractLastChild();
                newNode.Attach(child);
            }

            keysToMove--;
        }

        return newNode;
    }

    private BPlusTreeNode<TKey, TValue> FindLeafNode(BTreeNode<TKey> node, TKey key)
    {
        if (!node.HasChildren)
            return node as BPlusTreeNode<TKey, TValue> ?? throw new InvalidCastException($"Can't convert to {nameof(BPlusTreeNode<TKey, TValue>)}");

        var keysLength = node.Keys.Count;
        for (int i = 0; i < keysLength; i++)
        {
            if (key < node.Keys[i])
                return FindLeafNode(node.Children[i], key);
            else if (key >= node.Keys[i] && i + 1 <= keysLength - 1 && key <= node.Keys[i + 1])
                return FindLeafNode(node.Children[i + 1], key);
        }

        return FindLeafNode(node.Children[^1], key);
    }

    public override void Remove(TKey key)
    {
        if (Root is null)
            return;

        var leafNode = Search(key);
        if (leafNode is null)
            return;

        RemoveRecursively(leafNode, key);
    }

    private void RemoveRecursively(BPlusTreeNode<TKey, TValue> node, TKey key)
    {
        int index = node.Keys.IndexOf(key);
        node.RemoveKey(key);

        if (!node.IsUnderflow(Order))
        {
            if (node.IsLeaf)
                UpdateIndexes(node.Parent, key, index == node.Keys.Count ? node.Keys[index - 1] : node.Keys[index]);

            return;
        }

        var leftSibling = node.GetLeftSibling();
        var rightSibling = node.GetRightSibling();
        if (leftSibling is not null && leftSibling.CanLendKey(Order))
        {
            var keyToMove = leftSibling.ExtractLastKey();
            node.AddKey(keyToMove);

            if (node.Values is not null)
            {
                var valueToMove = leftSibling.ExtractLastValue();
                node.AddValue(valueToMove);
            }

            if (leftSibling.Parent.ContainsKey(keyToMove))
            {
                leftSibling.Parent.RemoveKey(keyToMove);
                leftSibling.Parent.AddKey(leftSibling.LastKey);
            }

            if (node.IsLeaf)
                UpdateIndexes(node.Parent, key, keyToMove);
        }
        else if (rightSibling is not null && rightSibling.CanLendKey(Order))
        {
            var keyToMove = rightSibling.ExtractFirsKey();
            node.AddKey(keyToMove);

            if (node.Values is not null)
            {
                var valueToMove = rightSibling.ExtractFirstValue();
                node.AddValue(valueToMove);
            }

            if (rightSibling.Parent.ContainsKey(keyToMove))
            {
                rightSibling.Parent.RemoveKey(keyToMove);
                rightSibling.Parent.AddKey(rightSibling.FirstKey);
            }

            if (node.IsLeaf)
                UpdateIndexes(node.Parent, key, keyToMove);

        }
        else if (leftSibling is not null)
        {
            UpdateLeavesLinksDeleting(node);

            if (node.IsLeaf)
                UpdateIndexes(node.Parent, key, leftSibling.LastKey);

            node?.Parent?.Detach(node);

            // merge with left sibling
            MergeToLeft(node, leftSibling);
        }
        else if (rightSibling is not null)
        {
            UpdateLeavesLinksDeleting(node);
            
            if (node.IsLeaf)
                UpdateIndexes(node.Parent, key, rightSibling.FirstKey);

            node?.Parent?.Detach(node);

            // merge with right sibling
            MergeToRight(node, rightSibling);
        }
    }

    private void MergeToRight(BPlusTreeNode<TKey, TValue> node, BPlusTreeNode<TKey, TValue> rightSibling)
    {
        var parent = rightSibling.Parent;

        var indexAsChild = parent.Children.IndexOf(rightSibling);
        rightSibling.AddKeyRange(node.Keys);

        if (node.Values is not null)
            (rightSibling?.Values ?? new()).AddRange<TValue>(node.Values);

        if (node.Children is not null)
            rightSibling.AttachRange(node.Children);

        var parentKey = parent.Keys[indexAsChild];
        parent.RemoveKey(parentKey);

        if (!rightSibling.Keys.Contains(parentKey))
            rightSibling.AddKey(parentKey);

        if (parent.IsUnderflow(Order))
            RemoveRecursively(parent, parentKey);
    }

    private void MergeToLeft(BPlusTreeNode<TKey, TValue> node, BPlusTreeNode<TKey, TValue> leftSibling)
    {
        var parent = leftSibling.Parent;

        var indexAsChild = parent.Children.IndexOf(leftSibling);
        leftSibling.AddKeyRange(node.Keys);

        if (node.Values is not null)
            (leftSibling?.Values ?? new()).AddRange<TValue>(node.Values);

        if (node.Children is not null)
            leftSibling.AttachRange(node.Children);

        var parentKey = parent.Keys[indexAsChild];
        parent.RemoveKey(parentKey);

        if (!leftSibling.Keys.Contains(parentKey))
            leftSibling.AddKey(parentKey);

        if (parent.IsUnderflow(Order))
            RemoveRecursively(parent, parentKey);
    }

    private void UpdateLeavesLinksDeleting(BPlusTreeNode<TKey, TValue> deletedNode)
    {
        if (!deletedNode.IsLeaf)
            return;

        if (deletedNode.Left is not null)
            deletedNode.Left.Right = deletedNode.Right;


        if (deletedNode.Right is not null)
            deletedNode.Right.Left = deletedNode.Left;
    }

    private void UpdateIndexes(BPlusTreeNode<TKey, TValue> node, TKey deletedKey, TKey newKey)
    {
        if (node is null)
            return;

        if (node.ContainsKey(deletedKey))
        {
            node.RemoveKey(deletedKey);
            node.AddKey(newKey);
        }

        if (node.Parent is null)
            return;

        UpdateIndexes(node.Parent, deletedKey, newKey);
    }
}
