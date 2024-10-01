namespace DataStructures.Heaps;

/// <summary>
/// Implements a Tournament Tree Min-Heap structure, where the extremum value 
/// (the minimum value) is always at the root, and leaf updates propagate upwards.
/// </summary>
/// <typeparam name="TKey">The type of the keys used for comparison.</typeparam>
/// <typeparam name="TValue">The type of the values stored in the heap.</typeparam>
public class TournamentTreeMin<TKey, TValue> where TKey : notnull
{
    // Total number of leaves (input elements)
    private readonly int _leafCount;

    // Array of HeapNode containing the key-value pairs (leaves)
    private readonly HeapNode<TKey, TValue>[] _leaves;

    // Array representing the tree, storing indices of the leaves
    private readonly int[] _tree;

    // Comparer used to determine the order of keys (for min-heap, it defaults to Comparer<TKey>.Default)
    private readonly IComparer<TKey> _comparer;

    /// <summary>
    /// Initializes a new instance of the TournamentTreeMin with the provided options and initial leaves.
    /// </summary>
    /// <param name="options">Options for the heap, including a comparer for the keys.</param>
    /// <param name="initialLeaves">Initial array of HeapNode elements.</param>
    public TournamentTreeMin(HeapOptions<TKey> options, HeapNode<TKey, TValue>[] initialLeaves)
    {
        _leafCount = initialLeaves.Length;
        _leaves = initialLeaves;
        _comparer = options.Comparer ?? Comparer<TKey>.Default; // Default comparer if none provided
        _tree = new int[_leafCount * 2 - 1]; // Complete binary tree structure

        BuildTree();
    }

    /// <summary>
    /// Constructs the tournament tree by comparing leaf nodes and propagating the minimums upwards.
    /// </summary>
    private void BuildTree()
    {
        // Initialize leaves in the last level of the binary tree
        for (int i = 0; i < _leafCount; i++)
        {
            _tree[i + _leafCount - 1] = i;
        }

        // Build the upper levels by comparing the child nodes
        for (int i = _leafCount - 2; i >= 0; i--)
        {
            var leftIndex = _tree[2 * i + 1];
            var rightIndex = _tree[2 * i + 2];

            _tree[i] = CompareNodes(leftIndex, rightIndex);
        }
    }

    /// <summary>
    /// Retrieves the current extremum (minimum) node in the heap.
    /// </summary>
    /// <returns>The <see cref="HeapNode{TKey, TValue}"/> that contains the minimum key.</returns>
    public HeapNode<TKey, TValue> GetExtremum()
    {
        return _leaves[_tree[0]]; // Root contains the minimum key
    }

    /// <summary>
    /// Updates the value of a specific leaf and rebalances the tree.
    /// </summary>
    /// <param name="leafIndex">Index of the leaf to update.</param>
    /// <param name="newKey">New key to assign to the leaf.</param>
    /// <param name="newValue">New value to assign to the leaf.</param>
    public void Update(int leafIndex, TKey newKey, TValue newValue)
    {
        // Update the leaf with new key-value pair
        _leaves[leafIndex] = new HeapNode<TKey, TValue> { Key = newKey, Value = newValue };

        // Adjust the tree from the updated leaf upwards
        int i = leafIndex + _leafCount - 1;

        while (i > 0)
        {
            i = (i - 1) / 2; // Move to parent node

            var leftIndex = _tree[2 * i + 1];
            var rightIndex = _tree[2 * i + 2];

            _tree[i] = CompareNodes(leftIndex, rightIndex); // Rebalance based on comparison
        }
    }

    /// <summary>
    /// Convenience method to update the root (first leaf) of the tree.
    /// </summary>
    /// <param name="newKey">New key for the root leaf.</param>
    /// <param name="newValue">New value for the root leaf.</param>
    public void UpdateRoot(TKey newKey, TValue newValue)
    {
        Update(0, newKey, newValue);
    }

    /// <summary>
    /// Compares two nodes (leaf indices) based on their keys and returns the index of the smaller key.
    /// </summary>
    /// <param name="leftIndex">Index of the left node.</param>
    /// <param name="rightIndex">Index of the right node.</param>
    /// <returns>Index of the node with the smaller key.</returns>
    private int CompareNodes(int leftIndex, int rightIndex)
    {
        if (leftIndex == -1)
            return rightIndex; // No left node

        if (rightIndex == -1)
            return leftIndex; // No right node

        // Retrieve the nodes from the leaves array
        var leftNode = _leaves[leftIndex];
        var rightNode = _leaves[rightIndex];

        // Compare the keys
        int cmp = _comparer.Compare(leftNode.Key, rightNode.Key);
        return cmp <= 0 ? leftIndex : rightIndex; // Return index of smaller key
    }
}
