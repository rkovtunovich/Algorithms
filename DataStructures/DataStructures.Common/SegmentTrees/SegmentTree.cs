namespace DataStructures.Common.SegmentTrees;

// A Segment Tree is a versatile data structure used for storing information about intervals or segments.
// It allows efficient range queries and updates, making it useful for various applications, such as range sum queries,
// range minimum queries, and range maximum queries.
// 
// Segment Tree Basics
// Key Properties:
// Static Structure: A Segment Tree is usually built on a static array of data, meaning the array's size does not change.
// Binary Tree Representation: It is represented as a binary tree, where each node represents an interval or segment of the array.
// Efficient Queries and Updates: It supports efficient range queries and updates, typically in O(logn) time,
// where 𝑛 is the size of the array.
// Structure:
// The root of the Segment Tree represents the entire array.
// Each internal node represents the union of its two child nodes, covering a subarray.
// Leaf nodes represent individual elements of the array.
// Segment Tree Construction
// The Segment Tree is typically represented using an array,
// where each node at index i has its left child at index 2i+1 and right child at index 2i+2.
public class SegmentTree<T> where T : INumber<T>
{
    public T[] Values { get; private set; }

    public int SegmentSize { get; private set; }

    public SegmentTree(T[] array)
    {
        // Get the number of elements in the array
        SegmentSize = array.Length;

        // Initialize the segment tree with 2 * n elements
        Values = new T[SegmentSize * 2];

        Build(array);
    }

    private void Build(T[] array)
    {
        // Copy the array elements to the leaf nodes of the segment tree
        for (var i = 0; i < SegmentSize; i++) 
            Values[SegmentSize + i] = array[i];

        // Build the segment tree by calculating the sum of the children nodes
        for (var i = SegmentSize - 1; i > 0; i--)
            Values[i] = Values[2 * i] + Values[2 * i + 1];
    }

    public void Update(int index, T value)
    {
        // Adjust the index to point to the corresponding leaf node in the segment tree
        index += SegmentSize;

        // Update the value at the leaf node
        Values[index] = value;

        // Traverse upwards and update the internal nodes
        while (index > 1)
        {
            // Move to the parent node
            index /= 2;

            // Update the parent node's value as the sum of its two children
            Values[index] = Values[2 * index] + Values[2 * index + 1];
        }
    }

    public T? Query(int left, int right)
    {
        // Initialize the result
        var result = default(T);

        // Adjust the indices to point to the corresponding leaf nodes in the segment tree
        left += SegmentSize;
        right += SegmentSize;

        // Traverse the segment tree to find the sum of the given range
        while (left <= right)
        {
            // If the left index is odd, add the value and move to the right
            if (left % 2 is 1)
            {
                result += Values[left];
                left++;
            }

            // If the right index is even, add the value and move to the left
            if (right % 2 is 0)
            {
                result += Values[right];
                right--;
            }

            // Move to the parent nodes
            left /= 2;
            right /= 2;
        }

        return result;
    }
}
