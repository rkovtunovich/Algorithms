namespace DataStructures.Trees.BinaryTrees.Search.AVL;

public class AVLTreeNode<TKey, TValue> : TreeNode<TKey, TValue> where TKey : INumber<TKey>
{
    public int Hight { get; set; }
}
