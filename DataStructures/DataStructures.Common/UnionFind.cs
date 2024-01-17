namespace DataStructures.Common;

// The union-find data structure, also known as the disjoint-set data structure, is a data structure that keeps track of a collection of disjoint sets (non-overlapping sets).
// It supports two main operations: union and find.
// The union operation is used to merge two sets, while the find operation is used to determine which set a particular element belongs to.
// Union-find is particularly useful for solving problems that involve manipulating and querying disjoint sets,
// such as in Kruskal's algorithm for finding the minimum spanning tree.
// 
// The union-find data structure consists of a set of elements, each of which initially belongs to its own set.
// Each set is represented by a tree, where the root node is considered the representative of the set.
// Each element contains a reference to its parent in the tree. If an element is the root of the tree, it is its own parent.
// 
// Here's a brief overview of the two main operations in a union-find data structure:
// 
// 1.   Find(x): This operation returns the representative of the set that element x belongs to.
//      To find the representative, the algorithm follows the parent pointers of x until it reaches the root node.
//      Path compression is a common optimization technique used during the find operation,
//      where the parent pointers of visited nodes are updated to point directly to the root,
//      thus flattening the tree and speeding up future find operations.
// 
// 2.   Union(x, y): This operation merges the sets containing elements x and y.
//      To do this, the algorithm first finds the root nodes (representatives) of the sets containing x and y using the find operation.
//      If the roots are distinct, it merges the two trees by making one of the root nodes the parent of the other.
//      Union by rank is a common optimization technique used during the union operation,
//      where the tree with the smaller rank (an approximation of the tree height) is attached to the root of the tree with the larger rank,
//      minimizing the increase in tree height.
// 
// The union-find data structure, with path compression and union by rank optimizations,
// has an extremely efficient time complexity for the combined sequence of union and find operations.
// The time complexity is nearly constant per operation, specifically O(m α(n)),
// where m is the number of operations, n is the number of elements, and α(n) is the inverse Ackermann function, which grows very slowly.
// In practice, this complexity is considered almost constant, as the inverse Ackermann function remains very small for all practical values of n.
// 
// In summary, the union-find data structure is an efficient data structure for managing disjoint sets, supporting union and find operations.
// It is particularly useful for problems involving manipulation and querying of disjoint sets,
// such as in Kruskal's minimum spanning tree algorithm. With path compression and union by rank optimizations,
// the time complexity for a sequence of union and find operations is almost constant, making it highly efficient for practical use cases.

public class UnionFind<T> where T : notnull
{
    private readonly UnionFindNode<T>[] _nodes;
    private readonly Dictionary<T, int> _indexes;

    public UnionFind(T[] source)
    {
        _nodes = new UnionFindNode<T>[source.Length];
        _indexes = new();

        for (int i = 0; i < source.Length; i++)
        {
            _nodes[i] = new()
            {
                ParentIndex = i,
                Size = 1,
                Item = source[i]
            };

            _indexes.Add(source[i], i);
        }
    }

    public UnionFindNode<T> Find(T item)
    {
        int index = _indexes[item];

        return FindRecursively(index);
    }

    public void Union(T first, T second)
    {
        var firstNode = Find(first);
        var secondNode = Find(second);

        if (firstNode.Size >= secondNode.Size)
        {
            secondNode.ParentIndex = _indexes[firstNode.Item];
            firstNode.Size += secondNode.Size;
        }
        else
        {
            firstNode.ParentIndex = _indexes[secondNode.Item];
            secondNode.Size += firstNode.Size;
        }
    }

    private UnionFindNode<T> FindRecursively(int index)
    {
        if (_nodes[index].ParentIndex == index)
            return _nodes[index];

        return FindRecursively(_nodes[index].ParentIndex);
    }
}
