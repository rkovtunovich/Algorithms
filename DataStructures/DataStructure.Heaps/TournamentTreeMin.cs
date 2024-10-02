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

    // Tracks the number of active elements in the tree (not yet extracted)
    private int _activeLeaves;

    // Array of HeapNode containing the key-value pairs (leaves)
    private readonly HeapNode<TKey, TValue>?[] _leaves;

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
        _activeLeaves = _leafCount;
        _comparer = options.Comparer ?? Comparer<TKey>.Default; // Default comparer if none provided
        _tree = new int[_leafCount * 2 - 1]; // Complete binary tree structure

        // Initialize the leaves array with the provided elements
        _leaves = new HeapNode<TKey, TValue>?[_leafCount];
        for (int i = 0; i < _leafCount; i++)
        {
            _leaves[i] = initialLeaves[i];
        }

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
            _tree[i + _tree.Length - _leafCount] = i; // Leaves start at index _tree.Length - _leafCount
        }

        // Build the upper levels by comparing the child nodes
        for (int i = _tree.Length - _leafCount - 1; i >= 0; i--)
        {
            int leftChildIndex = 2 * i + 1;
            int rightChildIndex = 2 * i + 2;

            int leftIndex = _tree[leftChildIndex];
            int rightIndex = rightChildIndex < _tree.Length ? _tree[rightChildIndex] : -1;

            _tree[i] = CompareNodes(leftIndex, rightIndex);
        }
    }

    /// <summary>
    /// Determines if the heap is empty (no elements left).
    /// </summary>
    public bool Empty => _activeLeaves is 0;

    /// <summary>
    /// Retrieves the current extremum (minimum) node in the heap.
    /// </summary>
    /// <returns>The <see cref="HeapNode{TKey, TValue}"/> that contains the minimum key.</returns>
    public HeapNode<TKey, TValue> GetExtremum()
    {
        if (Empty)
            throw new InvalidOperationException("The heap is empty.");

        int winnerIndex = _tree[0];
        if (winnerIndex == -1 || _leaves[winnerIndex] is null)
            throw new InvalidOperationException("The heap is empty.");

        return _leaves[winnerIndex].Value;
    }

    /// <summary>
    /// Extracts the current extremum from the heap.
    /// </summary>
    /// <returns>The <see cref="HeapNode{TKey, TValue}"/> that contains the minimum key.</returns>
    public HeapNode<TKey, TValue> ExtractExtremum()
    {
        if (Empty)
            throw new InvalidOperationException("The heap is empty.");

        var extremum = GetExtremum();

        // Get the index of the leaf from which the extremum came
        int leafIndex = _tree[0];

        // Mark the leaf as exhausted
        _leaves[leafIndex] = null;
        _activeLeaves--;

        // Adjust the tree from the leaf upwards
        AdjustTreeUpwards(leafIndex + _leafCount - 1);

        return extremum;
    }

    /// <summary>
    /// Extracts the current extremum and replaces it with a new key-value pair.
    /// </summary>
    /// <param name="newKey">New key to insert into the heap.</param>
    /// <param name="newValue">New value to insert into the heap.</param>
    /// <returns>The <see cref="HeapNode{TKey, TValue}"/> that contains the previous minimum key.</returns>
    public HeapNode<TKey, TValue> ExtractWithReplacement(TKey newKey, TValue newValue)
    {
        if (Empty)
            throw new InvalidOperationException("The heap is empty.");

        var extremum = GetExtremum();

        // Replace the leaf from which the extremum came
        int leafIndex = _tree[0];

        _leaves[leafIndex] = new HeapNode<TKey, TValue> { Key = newKey, Value = newValue };

        // Adjust the tree from the leaf upwards
        AdjustTreeUpwards(leafIndex + _tree.Length - _leafCount);

        return extremum;
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
        int i = leafIndex + _activeLeaves - 1;

        AdjustTreeUpwards(i);
    }

    /// <summary>
    /// Convenience method to update the root (first leaf) of the tree.
    /// </summary>
    /// <param name="newKey">New key for the root leaf.</param>
    /// <param name="newValue">New value for the root leaf.</param>
    public void UpdateRoot(TKey newKey, TValue newValue)
    {
        var leafIndex = _tree[0];

        Update(leafIndex, newKey, newValue);
    }

    /// <summary>
    /// Compares two nodes (leaf indices) based on their keys and returns the index of the smaller key.
    /// </summary>
    /// <param name="leftIndex">Index of the left node.</param>
    /// <param name="rightIndex">Index of the right node.</param>
    /// <returns>Index of the node with the smaller key.</returns>
    private int CompareNodes(int leftIndex, int rightIndex)
    {
        bool leftNull = leftIndex is -1 || _leaves[leftIndex] is null;
        bool rightNull = rightIndex is -1 || _leaves[rightIndex] is null;

        if (leftNull && rightNull)
            return -1; // Both nodes are null, return invalid index
        if (leftNull)
            return rightIndex;
        if (rightNull)
            return leftIndex;

        var leftNode = _leaves[leftIndex];
        var rightNode = _leaves[rightIndex];

        int cmp = _comparer.Compare(leftNode.Value.Key, rightNode.Value.Key);
        return cmp <= 0 ? leftIndex : rightIndex;
    }

    /// <summary>
    /// Adjusts the tree upwards from the specified leaf index.
    /// </summary>
    /// <param name="treeIndex">Index of the leaf to start adjusting from.</param>
    private void AdjustTreeUpwards(int treeIndex)
    {
        while (treeIndex > 0)
        {
            int parentIndex = (treeIndex - 1) / 2;

            int leftChildIndex = 2 * parentIndex + 1;
            int rightChildIndex = 2 * parentIndex + 2;

            int leftIndex = leftChildIndex < _tree.Length ? _tree[leftChildIndex] : -1;
            int rightIndex = rightChildIndex < _tree.Length ? _tree[rightChildIndex] : -1;

            _tree[parentIndex] = CompareNodes(leftIndex, rightIndex);

            treeIndex = parentIndex;
        }
    }
}
