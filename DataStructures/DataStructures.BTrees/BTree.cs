namespace DataStructures.BTrees;

public abstract class BTree<TKey> where TKey : INumber<TKey>
{
    protected BTree(int order)
    {
        Order = order;
    }

    public int Order { get; init; }

    public BTreeNode<TKey>? Root { get; set; }

    public abstract void Insert(TKey key);

    public abstract void Remove(TKey key);

    public virtual BTreeNode<TKey>? Search(TKey key)
    {
        if (Root is null)
            return null;

        return SearchRecursively(Root, key);
    }

    public BTreeNode<TKey> BelongValue(BTreeNode<TKey> node, TKey key)
    {
        if (!node.HasChildren)
            return node;

        var keysLength = node.Keys.Count;
        for (int i = 0; i < keysLength; i++)
        {
            if (key <= node.Keys[i])
                return BelongValue(node.Children[i], key);
            else if (key > node.Keys[i] && i + 1 <= keysLength - 1 && key <= node.Keys[i + 1])
                return BelongValue(node.Children[i + 1], key);
        }

        return BelongValue(node.Children[^1], key);
    }

    private BTreeNode<TKey>? SearchRecursively(BTreeNode<TKey> node, TKey key)
    {
        if (!node.HasChildren)
        {
            if (node.ContainsKey(key))
                return node;
            else
                return null;
        }

        var keysLength = node.Keys.Count;
        for (int i = 0; i < keysLength; i++)
        {
            if (node.Keys[i] == key)
                return node;
            else if (key < node.Keys[i])
                return SearchRecursively(node.Children[i], key);
        }

        return SearchRecursively(node.Children[^1], key);
    }

    protected virtual BTreeNode<TKey>? SearchMinimumChildNode(BTreeNode<TKey>? node)
    {
        if (node is null)
            return null;

        var min = node;

        if (!min.HasChildren)
            return min;
        else
            return SearchMinimumChildNode(node.FirstChild);

    }

    protected virtual BTreeNode<TKey>? SearchMaximumChildNode(BTreeNode<TKey>? node)
    {
        if (node is null)
            return null;

        var min = node;

        if (!min.HasChildren)
            return min;
        else
            return SearchMinimumChildNode(node.LastChild);

    }
}
