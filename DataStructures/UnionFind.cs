using System.Security.AccessControl;

namespace DataStructures;

// aslo cold non-related set
public class UnionFind<T>
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

        return FindRecursevely(index);
    }

    public void Union(T firsrt, T second)
    {
        var firstNode = Find(firsrt);
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

    private UnionFindNode<T> FindRecursevely(int index)
    {
        if (_nodes[index].ParentIndex == index)
            return _nodes[index];

        return FindRecursevely(_nodes[index].ParentIndex);
    }
}
