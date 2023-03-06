using DataStructures.Lists;
using System.Numerics;

namespace DataStructures.BinaryTrees.Search;

public class AVLTreeNode<TKey, TValue> : TreeNode<TKey, TValue> where TKey : INumber<TKey>
{
    public int Hight { get; set; }
}
