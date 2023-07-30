using DataStructures.Lists;

namespace DataStructures.BTrees;

public class BPlusTreeNode<TKey, TValue> : BTreeNode<TKey> where TKey : INumber<TKey>
{
    public BPlusTreeNode()
    {
    }

    public BPlusTreeNode(TKey key) : base(key)
    {
    }

    public override BPlusTreeNode<TKey, TValue>? Parent { get => base.Parent as BPlusTreeNode<TKey, TValue>; }

    public BPlusTreeNode<TKey, TValue>? Right { get; set; }

    public BPlusTreeNode<TKey, TValue>? Left { get; set; }

    public SequentialList<TValue>? Values { get; set; }

    #region Getting value

    public override BPlusTreeNode<TKey, TValue>? GetLeftSibling()
    {
        return base.GetLeftSibling() as BPlusTreeNode<TKey, TValue>;
    }

    public override BPlusTreeNode<TKey, TValue>? GetRightSibling()
    {
        return base.GetRightSibling() as BPlusTreeNode<TKey, TValue>;
    }

    #endregion

    #region Checks

    public bool IsOverflow(int order)
    {
        return Keys.Count > order - 1;
    }

    internal bool IsUnderflow(int order)
    {
        return Keys.Count < order / 2;
    }

    internal bool CanLendKey(int order)
    {
        return Keys.Count > order / 2;
    }

    #endregion

    #region Modifying

    public override void RemoveKey(TKey key)
    {
        if (IsLeaf)
            Values?.RemoveAt(Keys.IndexOf(key));

        base.RemoveKey(key);
    }

    internal void AddValue(TValue value)
    {
        Values ??= new();
        Values.Add(value);
    }

    internal void RemoveValueByIndex(int index)
    {
        Values?.RemoveAt(index);
    }

    public TValue ExtractFirstValue()
    {
        if (Values is null)
            throw new InvalidOperationException("Values is null");

        var value = Values[0];
        Values.Remove(value);

        return value;
    }

    public TValue ExtractLastValue()
    {
        if (Values is null)
            throw new InvalidOperationException("Values is null");

        var value = Values[^1];
        Values.Remove(value);

        return value;
    }

    #endregion
}
