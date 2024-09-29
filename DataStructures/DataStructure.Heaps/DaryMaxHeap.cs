namespace DataStructures.Heaps;

public class DaryMaxHeap<TKey, TValue> : Heap<TKey, TValue> where TKey : IComparable<TKey>
{
    public DaryMaxHeap(HeapOptions<TKey> options) : base(options)
    {
    }

    protected override void SiftUp(int position)
    {
        if (IsRoot(position))
            return;

        // Get the parent position of the current position.
        int parentPosition = GetParentPosition(position);

        // if the current node is greater than its parent node, return.
        if (this[position].Key.CompareTo(this[parentPosition].Key) <= 0)
            return;

        // Swap the current node with its parent node.
        Swap(position, parentPosition);

        // Recursively sift up the parent node.
        SiftUp(parentPosition);
    }

    protected override void SiftDown(int position)
    {
        // Get the left children positions of the current position.
        int[] childrenPositions = GetChildrenPositions(position);

        // Get the position of the child node with the maximum key value.
        int nextPosition = GetHeapifyDownPosition(position, childrenPositions);

        // If the current node is greater than or equal to the child node with the maximum key value,
        // return.
        if (nextPosition == position)
            return;

        // Swap the current node with the child node with the maximum key value.
        Swap(position, nextPosition);

        // Recursively sift down the child node with the maximum key value.
        SiftDown(nextPosition);
    }

    private int GetHeapifyDownPosition(int position, int[] childrenPositions)
    {
        // find largest child
        int largestChildPosition = position;

        for (int i = 0; i < Degree; i++)
        {
            if (childrenPositions[i] == -1)
                continue;

            if (this[childrenPositions[i]].Key.CompareTo(this[largestChildPosition].Key) > 0)
                largestChildPosition = childrenPositions[i];
        }

        return largestChildPosition;
    }
}
