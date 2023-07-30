namespace DataStructures.BTrees;

// Properties of 2-3 tree:
// 
// 1. Nodes with two children are called 2-nodes. The 2-nodes have one data value and two children
// 2. Nodes with three children are called 3-nodes. The 3-nodes have two data values and three children.
// 3. Data is stored in sorted order.
// 4. It is a balanced tree.
// 5. All the leaf nodes are at same level.
// 6. Each node can either be leaf, 2 node, or 3 node.
// 7. Always insertion is done at leaf.

public class TwoThreeTree<TKey> : BTree<TKey> where TKey : INumber<TKey>
{
    private const int ORDER = 3;

    public TwoThreeTree() : base(ORDER)
    {
    }

    public override void Insert(TKey key)
    {
        if (Root is null)
        {
            Root = new BTreeNode<TKey>(key);
            return;
        }

        var node = BelongValue(Root, key);
        InsertRecursively(node, key);
    }

    public override void Remove(TKey key)
    {
        var node = Search(key);

        if (node is null)
            return;

        if (node.IsRoot && !node.HasChildren)
        {
            node.RemoveKey(key);
            if (node.IsEmpty)
                Root = null;

            return;
        }
        else if (node.HasChildren)
        {
            var successorNode = SearchMaximumChildNode(node.FirstChild) ?? throw new NullReferenceException(nameof(node.FirstChild));
            var successorKey = successorNode.ExtractLastKey();

            node.ReplaceKey(key, successorKey);
            successorNode.ReplaceKey(successorKey, key); //+

            Case1(successorNode, key);
        }
        else
        {
            Case1(node, key);
        }
    }

    #region Private methods

    #region Removing


    // Node with removed key is not empty (has another key because it had 2 keys before removing).
    // In this case properties of the tree isn't violated. 
    private void Case1(BTreeNode<TKey> node, TKey key)
    {
        node.RemoveKey(key);

        if (node.Keys.Count is 1)
            return;

        Case2(node, key);
    }

    // we have a brother with two keys and we can take one key with rearranging
    private void Case2(BTreeNode<TKey> node, TKey key)
    {
        if (node.IsRoot)
        {
            Case3(node, key);
            return;
        }

        var brothers = node.Parent.Children;

        BTreeNode<TKey>? brotherWithTwoKeys = null;
        foreach (var brother in brothers)
        {
            if (node == brother)
                continue;

            if (brother.Keys.Count < 2)
                continue;

            brotherWithTwoKeys = brother;
            break;
        }

        if (brotherWithTwoKeys is not null)
        {
            var isLeftShifting = key < brotherWithTwoKeys.FirstKey;

            if (isLeftShifting)
                ShiftKeysLeft(node, brotherWithTwoKeys);
            else
                ShiftKeysRight(node, brotherWithTwoKeys);

            return;
        }

        Case3(node, key);
    }

    private void Case3(BTreeNode<TKey> node, TKey key)
    {
        if (node.IsRoot)
        {
            Root = node.FirstChild;
            return;
        }

        var parent = node.Parent;
        if (parent.Keys.Count is 1)
        {
            node.Parent.Detach(node);
            var brother = key < parent.FirstKey ? parent.LastChild : parent.FirstChild;
            var ParentsKey = parent.ExtractFirsKey();
            brother.AddKey(ParentsKey);

            if (node.HasChildren)
                brother.Attach(node.FirstChild);

            Case2(parent, ParentsKey);
        }
        else
        {
            node.Parent.Detach(node);

            if (parent.IsLastChild(node))
            {
                var brother = parent.LastChild;
                brother.AddKey(parent.ExtractLastKey());
            }
            else
            {
                var brother = parent.FirstChild;
                brother.AddKey(parent.ExtractFirsKey());
            }
        }
    }

    private void ShiftKeysLeft(BTreeNode<TKey> firstNode, BTreeNode<TKey> lastNode)
    {
        var parent = firstNode.Parent;
        var startIndex = parent.Children.IndexOf(firstNode);
        firstNode.AddKey(parent.ExtractFirsKey());

        var firstRightBrother = parent.Children[startIndex + 1];
        parent.AddKey(firstRightBrother.ExtractFirsKey());

        if (firstRightBrother == lastNode)
            return;

        var secondRightBrother = parent.Children[startIndex + 2];
        firstRightBrother.AddKey(parent.ExtractLastKey());
        parent.AddKey(secondRightBrother.ExtractFirsKey());
    }

    private void ShiftKeysRight(BTreeNode<TKey> firstNode, BTreeNode<TKey> lastNode)
    {
        var parent = firstNode.Parent;
        var startIndex = parent.Children.IndexOf(firstNode);
        firstNode.AddKey(parent.ExtractLastKey());

        var firstLeftBrother = parent.Children[startIndex - 1];
        parent.AddKey(firstLeftBrother.ExtractLastKey());

        if (firstLeftBrother == lastNode)
            return;

        var secondLeftBrother = parent.Children[startIndex - 2];
        firstLeftBrother.AddKey(parent.ExtractFirsKey());
        parent.AddKey(secondLeftBrother.ExtractLastKey());
    }

    #endregion

    private void InsertRecursively(BTreeNode<TKey> node, TKey key)
    {
        node.Keys.Add(key);
        node.Keys.Sort();

        if (node.Keys.Count < ORDER)
            return;

        var parent = node.Parent;
        var left = new BTreeNode<TKey>(node.FirstKey);
        var right = new BTreeNode<TKey>(node.LastKey);

        if (node.Children?.Count is 4)
        {
            left.Attach(node.Children[0]);
            left.Attach(node.Children[1]);
            right.Attach(node.Children[2]);
            right.Attach(node.Children[3]);
        }

        var middleIndex = 1;
        var middleKey = node.GetKeyByIndex(middleIndex);

        if (node.IsRoot)
        {
            Root = new BTreeNode<TKey>(middleKey);
            Root.Attach(left);
            Root.Attach(right);
        }
        else
        {
            parent.Detach(node);
            parent.Attach(left);
            parent.Attach(right);

            InsertRecursively(parent, middleKey);
        }
    }

    #endregion
}
