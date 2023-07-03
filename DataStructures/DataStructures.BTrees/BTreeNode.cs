using DataStructures.Lists;

namespace DataStructures.BTrees;

public class BTreeNode<TKey> : IComparable<BTreeNode<TKey>> where TKey : INumber<TKey>
{
    #region Constructors

    public BTreeNode()
    {
    }

    public BTreeNode(TKey key)
    {
        Keys.Add(key);
    }

    #endregion

    #region Properties

    public BTreeNode<TKey>? Parent { get; set; }

    public SequentialList<TKey> Keys { get; set; } = new();

    public SequentialList<BTreeNode<TKey>>? Children { get; set; }

    #region Calculated properties

    public TKey FirstKey => Keys[0];

    public TKey LastKey => Keys[^1];

    public BTreeNode<TKey> FirstChild => Children?[0] ?? throw new ArgumentNullException(nameof(Children));

    public BTreeNode<TKey> LastChild => Children?[^1] ?? throw new ArgumentNullException(nameof(Children));

    #endregion

    #endregion

    #region Getting value

    public TKey GetKeyByIndex(int index)
    {
        return Keys[index];
    }

    #endregion

    #region Checks

    public bool HasParent => Parent != null;

    public bool IsRoot => !HasParent;

    public bool HasChildren => Children is not null && Children.Count is not 0;

    public bool IsEmpty => Keys.Count is 0 && (Children?.Count ?? 0) is 0;

    public bool IsFirstChild(BTreeNode<TKey> node) => Children?[0] == node;

    public bool IsLastChild(BTreeNode<TKey> node) => Children?[^1] == node;

    public bool ContainsKey(TKey key)
    {
        return Keys.Contains(key);
    }

    #endregion

    #region Modification

    public TKey ExtractKeyByIndex(int index)
    {
        var key = Keys[index];
        Keys.RemoveAt(index);

        return key;
    }

    public TKey ExtractFirsKey()
    {
        var key = Keys[0];
        Keys.Remove(key);

        return key;
    }

    public TKey ExtractLastKey()
    {
        var key = Keys[^1];
        Keys.Remove(key);

        return key;
    }

    public void DeleteKey(TKey key)
    {
        Keys.Remove(key);
    }

    public void Attach(BTreeNode<TKey> node)
    {
        Children ??= new();
        Children.Add(node);
        Children.Sort();

        node.Parent = this;
    }

    public void Detach(BTreeNode<TKey> node)
    {
        Children?.Remove(node);
    }

    public void ReplaceKey(TKey originKey, TKey newKey)
    {
        var index = Keys.IndexOf(originKey);
        if (index is -1)
            throw new KeyNotFoundException($"Current node doesn't contain key {originKey}");

        Keys[index] = newKey;
    }

    public void AddKey(TKey key)
    {
        Keys.Add(key);
        Keys.Sort();
    }

    #endregion

    #region Comparison

    public int CompareTo(BTreeNode<TKey>? other)
    {
        if (other is null)
            return 1;

        if (other.Keys[0] > Keys[0])
            return -1;
        else if (other.Keys[0] < Keys[0])
            return 1;

        return 0;
    }

    public static bool operator <(BTreeNode<TKey> left, BTreeNode<TKey> right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(BTreeNode<TKey> left, BTreeNode<TKey> right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(BTreeNode<TKey> left, BTreeNode<TKey> right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(BTreeNode<TKey> left, BTreeNode<TKey> right)
    {
        return left.CompareTo(right) >= 0;
    }

    #endregion
}
