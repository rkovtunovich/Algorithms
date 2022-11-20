﻿using System.Numerics;

namespace DataStructures.SearchTrees;

public class AVLTreeNode<TKey, TValue> : TreeNode<TKey, TValue> where TKey : INumber<TKey>
{
    public int Hight { get; set; }
}
