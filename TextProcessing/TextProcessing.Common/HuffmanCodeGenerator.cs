using DataStructures.Common.BinaryTrees.Search;
using DataStructures.Common.BinaryTrees;
using DataStructures.Heaps;

namespace TextProcessing.Common;

// Huffman tree is also called the optimal binary tree. It is a binary tree with the smallest weighted path length(WPL)
// 1.According to the weights of the given n leaf nodes, we can view them as n binary trees with only one root node.Suppose F is the set composed of these n binary trees.
// 2. Select two trees with the smallest root node values as the left, right subtrees,
//    and construct a new binary tree.Set the weight of the root of the new binary tree as the sum of the weight values of the roots of left and right subtrees.
//3. Remove these two trees from F, and add the new constructed tree into F.
//4. Repeat 2 and 3, until there is only one tree left in F.

public class HuffmanCodeGenerator
{
    public BinaryTree<double, string> Generate(Dictionary<string, double> alphabet)
    {

        var codes = new Dictionary<string, string>();

        var heapMin = new HeapMin<double, BinaryTree<double, string>>();

        foreach (var code in alphabet)
        {
            var tree = new SearchTree<double, string>();
            tree.Insert(code.Value, code.Key);

            heapMin.Insert(code.Value, tree);
        }

        while (heapMin.Length > 1)
        {
            var x = heapMin.ExtractValue();
            var y = heapMin.ExtractValue();

            var tree = new BinaryTree<double, string>();
            double key = x.Root.Key + y.Root.Key;
            tree.Insert(key, x.Root.Value + y.Root.Value);
            tree.AttachLeft(x.Root, tree.Root);
            tree.AttachRight(y.Root, tree.Root);

            heapMin.Insert(key, tree);
        }

        var result = heapMin.ExtractValue();

        return result;
    }
}
