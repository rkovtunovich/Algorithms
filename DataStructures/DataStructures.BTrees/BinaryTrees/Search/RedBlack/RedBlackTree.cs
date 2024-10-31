namespace DataStructures.Trees.BinaryTrees.Search.RedBlack;

// A Red-Black tree is a type of self-balancing binary search tree where each node has an extra bit for denoting the color of the node,
// either red or black. A Red-Black tree satisfies the following properties:
// 
// 1. **Every node is either red or black.**
// 2. **The root is black.** This rule is sometimes omitted.
//      Since the root can always be changed from red to black, but not necessarily vice versa, this rule has little effect on analysis.
// 3. **All leaves (null or NIL) are black.**
// 4. **If a node is red, then both its children are black.**
// 5. **Every path from a given node to any of its descendant NIL nodes contains the same number of black nodes.**
// 
// Some of the applications of Red-Black trees are:
// 
// 1. **In Most of the Self-balancing BST, like AVL Tree, Cartersian Tree, Weight Balanced Tree, Tango Tree, Splay Tree, and Treap**
// 2. **Used in the GNU STL** - The GNU standard template library's map and set are implemented using Red-Black tree.
// 3. **Used in the core Linux kernel.**
// 
// Efficiency of Red-Black Trees:
// 
// 1. **Search is O(log n)**: Because the tree is balanced,
//      the longest possible path from the root to a leaf is no more than twice as long as the shortest possible path.
//      The maximum path is thus O(log n) nodes long, where n is the number of nodes in the tree.
// 2. **Insertion is O(log n)**: To insert a node, we start by adding it as we would for a regular unbalanced binary search tree.
//      Then, we color it red. If its parent is black, we are done. If its parent is red, we have violated the "no two reds" rule,
//      and we need to fix the tree.
// 3. **Deletion is O(log n)**: Deletion in a Red-Black tree is similar to deletion in a binary search tree,
//      but with additional steps to ensure the tree remains balanced after the deletion.
// 
// In summary, Red-Black trees are a good general-purpose self-balancing binary search tree that provides efficient lookup, insertion,
// and removal operations. They are used in many languages' standard libraries to implement ordered collections.

public class RedBlackTree<TKey, TValue> : SearchTree<TKey, TValue> where TKey : INumber<TKey>
{
    public override void Insert(TKey key, TValue? value = default)
    {
        var newNode = CreateNode(key, value);
        InsertRecursively(Root as RedBlackTreeNode<TKey, TValue>, newNode);
    }

    public override void Remove(TKey key)
    {
        if (Root is null)
            return;

        if (SearchRecursively(Root, key) is not RedBlackTreeNode<TKey, TValue> node)
            return;

        RemoveRecursively(node);
    }

    protected override RedBlackTreeNode<TKey, TValue> CreateNode(TKey key, TValue? value)
    {
        return new RedBlackTreeNode<TKey, TValue>()
        {
            Key = key,
            Value = value,
            Color = Color.Red
        };
    }

    private void InsertRecursively(RedBlackTreeNode<TKey, TValue>? parent, RedBlackTreeNode<TKey, TValue> child)
    {
        if (Root is null)
        {
            SetRoot(child);
        }
        else if (child.Key <= parent.Key)
        {
            if (!parent.HasLeftChild)           
                parent.AttachLeft(child);         
            else
                InsertRecursively(parent.LeftChild, child);
        }
        else
        {
            if (!parent.HasRightChild)          
                parent.AttachRight(child);            
            else
                InsertRecursively(parent.RightChild, child);
        }

        Balance(child);
    }

    private void RemoveRecursively(RedBlackTreeNode<TKey, TValue> node)
    {
        if (node.HasBothChildren)
        {
            var successor = SearchMaximum(node.LeftChild) as RedBlackTreeNode<TKey, TValue> ?? throw new NullReferenceException(nameof(node.LeftChild));

            node.Key = successor.Key;
            node.Value = successor.Value;

            DeleteRedBlackNode(successor);
        }
        else
        {
            DeleteRedBlackNode(node);
        }
    }

    private void Balance(RedBlackTreeNode<TKey, TValue> child)
    {
        // case 1:
        // The current node N at the root of the tree.
        // In this case, it is repainted black to keep Property 2 (Root is black) true.
        // Since this action adds one black node to each path, Property 5 (All paths from any given node to leaf nodes contain the same number of black nodes) is not violated.
        if (child.IsRoot)
        {
            child.Color = Color.Black;
            return;
        }

        // case 2:
        // The parent P of the current node is black, i.e.
        // Property 4 (Both children of every red node are black) is not violated.
        // In this case, the tree remains correct.
        // Property 5 (All paths from any given node to leaf nodes contain the same number of black nodes) is not violated because the current node N has two black leaf children,
        // but since N is red, the path to each of these children contains the same number of black nodes, which is the path to the black leaf that was replaced by the current node,
        // so the property remains true.
        var parent = child.Parent ?? throw new NullReferenceException($"The parent of {child} must be not null");
        if (parent.Color is Color.Black)
            return;

        // case 3:
        // If both parent P and uncle U are red, then they can both be recolored black,
        // and grandparent G becomes red (to preserve property 5 (All paths from any given node to leaf nodes contain the same number of black nodes)).
        // Now the current red node N has a black parent.
        // Since any path through the parent or uncle must go through the grandparent, the number of black nodes in these paths will not change.
        // However, G's grandparent can now violate properties 2 (Root is black) or 4 (Both children of each red node are black)
        // (property 4 can be violated because G's parent can be red).
        // To fix this, the whole procedure is performed recursively on G from Case 1.
        var uncle = GetUncle(child) as RedBlackTreeNode<TKey, TValue>;
        var grandParent = parent.Parent;
        if (child.IsRed && (uncle?.IsRed ?? false))
        {
            parent.Color = Color.Black;
            uncle.Color = Color.Black;
            grandParent.Color = Color.Red;

            Balance(grandParent);
        }

        // case 4:
        // P's parent is red, but U's uncle is black.
        // Also, the current node N is the right child of P, and P, in turn, is the left child of its parent G.
        // In this case, a tree rotation can be performed that changes the roles of the current node N and its parent P.
        // Then, for the former parent node P in For the updated structure, we use case 5 because Property 4
        // (Both children of any red node are black) is still violated.
        // The rotation causes some paths (in the subtree labeled "1" in the diagram) to pass through node N, which was not the case before.
        // This also causes some paths (in the subtree labeled "3") to not pass through node P.
        // However, both of these nodes are red, so Property 5 (All paths from any given node to leaf nodes contain the same number of black knots) is not broken during rotation.
        // However, Property 4 is still violated, but now the problem is reduced to Case 5.
        // If the current node N is the left child of its parent P, a right rotation is performed on P.
        if (child.IsRightChild && child.IsRed && parent.IsLeftChild && parent.IsRed && (uncle?.IsBlack ?? true))
        {
            RotateLeft(parent, child);
            Balance(parent);
        }
        // If the current node N is the right child of its parent P, a left rotation is performed on P.
        if (child.IsLeftChild && child.IsRed && parent.IsRightChild && parent.IsRed && (uncle?.IsBlack ?? true))
        {
            RotateRight(parent, child);
            Balance(parent);
        }

        // case 5:
        // The parent of P is red, but the uncle of U is black, the current node N is the left child of P, and P is the left child of G.
        // In this case, the tree is rotated by G.
        // The result is a tree in which the former parent P is now the parent of the current node as well.
        // N and G's former grandparent.
        // G is known to be black, since its former descendant P could not otherwise be red (without violating Property 4).
        // Then the colors of P and G change and as a result the tree satisfies Property 4 (Both children of any red node are black).
        // Property 5 (All paths from any given node to leaf nodes contain the same number of black nodes) also remains true,
        // since all paths that pass through any of these three nodes previously went through G, so they now all go through P.
        // B In each case, of these three nodes, only one is colored black.
        if (child.IsLeftChild && child.IsRed && parent.IsLeftChild && parent.IsRed && (uncle?.IsBlack ?? true))
        {
            parent.Color = Color.Black;
            grandParent.Color = Color.Red;
            RotateRight(parent.Parent, parent);
        }
        if (child.IsRightChild && child.IsRed && parent.IsRightChild && parent.IsRed && (uncle?.IsBlack ?? true))
        {
            parent.Color = Color.Black;
            grandParent.Color = Color.Red;
            RotateLeft(parent.Parent, parent);
        }
    }

    #region Delete

    private void DeleteRedBlackNode(RedBlackTreeNode<TKey, TValue> node)
    {
        var parent = node.Parent;

        if ((node.LeftChild ?? node.RightChild) is RedBlackTreeNode<TKey, TValue> child)
        {
            Swap(node, child);
            if (parent.IsRoot)
            {
                child.Color = Color.Black;
            }
            else if (node.IsBlack)
            {
                if (child.IsRed)
                {
                    child.Color = Color.Black;
                    DeleteNode(node);
                }
                else
                {
                    DeleteCase1(child);
                }
            }
        }
        else if (parent.IsRoot)
        {
            SetRoot(null);
        }
        else
        {
            if (node.IsBlack)
                DeleteCase1(node);

            DeleteNode(node);
        }
    }

    private void DeleteCase1(RedBlackTreeNode<TKey, TValue> node)
    {
        if (node.HasParent)
            DeleteCase2(node);
    }

    private void DeleteCase2(RedBlackTreeNode<TKey, TValue> node)
    {
        var sibling = node.Brother;

        if ((sibling?.IsRed ?? false) && sibling.HasAnyChild)
        {
            node.Parent.Color = Color.Red;
            sibling.Color = Color.Black;

            if (node.IsLeftChild)
                RotateLeft(node.Parent, sibling);
            else
                RotateRight(node.Parent, sibling);
        }

        DeleteCase3(node);
    }

    private void DeleteCase3(RedBlackTreeNode<TKey, TValue> node)
    {
        var sibling = node.Brother;
        var parentSibling = node.Parent.Brother;

        if (node.Parent.IsBlack && (sibling?.IsBlack ?? false) && (sibling.LeftChild?.IsBlack ?? false) && (sibling.RightChild?.IsBlack ?? false))
        {
            sibling.Color = Color.Red;
            DeleteCase1(node.Parent);
        }
        else if (node.Parent.IsBlack && (parentSibling?.IsBlack ?? false) && (parentSibling?.LeftChild.IsBlack ?? false) && (parentSibling?.LeftChild.IsBlack ?? false))
        {
            sibling.Color = Color.Red;
            parentSibling.Color = Color.Red;
            DeleteNode(node);
        }
        else
        {
            DeleteCase4(node);
        }
    }

    private void DeleteCase4(RedBlackTreeNode<TKey, TValue> node)
    {
        var sibling = node.Brother;

        if (node.Parent.IsRed && (sibling?.IsBlack ?? false) && (sibling.LeftChild?.IsBlack ?? false) && (sibling.RightChild?.IsBlack ?? false))
        {
            sibling.Color = Color.Red;
            node.Parent.Color = Color.Black;
        }
        else
        {
            DeleteCase5(node);
        }
    }

    private void DeleteCase5(RedBlackTreeNode<TKey, TValue> node)
    {
        var sibling = node.Brother;

        if (sibling?.IsBlack ?? false)
        {
            if (node == node.Parent.LeftChild && sibling.RightChild.IsBlack &&
                sibling.LeftChild.IsRed)
            {
                sibling.Color = Color.Red;
                sibling.LeftChild.Color = Color.Black;
                RotateRight(sibling, sibling.LeftChild);
            }
            else if (node == node.Parent.RightChild && (sibling.LeftChild?.IsBlack ?? false) && (sibling.RightChild?.IsRed ?? false))
            {
                sibling.Color = Color.Red;
                sibling.RightChild.Color = Color.Black;
                RotateLeft(sibling, sibling.RightChild);
            }
        }

        DeleteCase6(node);
    }

    private void DeleteCase6(RedBlackTreeNode<TKey, TValue> node)
    {
        var sibling = node.Brother;

        sibling.Color = node.Parent.Color;
        node.Parent.Color = Color.Black;

        if (node.IsLeftChild)
        {
            if (sibling.HasRightChild)
            {
                sibling.RightChild.Color = Color.Black;
                RotateLeft(node.Parent, sibling);
            }
        }
        else if (node.IsRightChild)
        {
            if (sibling.HasLeftChild)
            {
                sibling.LeftChild.Color = Color.Black;
                RotateRight(node.Parent, sibling);
            }
        }

        DeleteNode(node);
    }

    #endregion

    #region Rotation

    private void RotateLeft(RedBlackTreeNode<TKey, TValue> parent, RedBlackTreeNode<TKey, TValue> child)
    {
        if (child.HasLeftChild)
            parent.AttachRight(child.LeftChild);
        else
            Detach(child);

        if (!parent.HasParent)
            SetRoot(child);
        else if (parent.IsLeftChild)
            parent.Parent.AttachLeft(child);
        else
            parent.Parent.AttachRight(child);

        child.AttachLeft(parent);
    }

    private void RotateRight(RedBlackTreeNode<TKey, TValue> parent, RedBlackTreeNode<TKey, TValue> child)
    {
        if (child.HasRightChild)
             parent.AttachLeft(child.RightChild);
        else
            Detach(child);

        if (!parent.HasParent)
            SetRoot(child);
        else if (parent.IsRightChild)
            parent.Parent.AttachRight(child);
        else
            parent.Parent.AttachLeft(child);

        child.AttachRight(parent);
    }

    #endregion
}