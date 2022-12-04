using DataStructures.BinaryTrees;
using DataStructures.BinaryTrees.Search;
using DataStructures.Heaps;

namespace Huffman;

public class HuffmanCodeGenerator
{
    public BinaryTree<double, string> Generate(Dictionary<string, double> alphabet ) { 
        
        var codes = new Dictionary<string, string>();

        var heapMin = new HeapMin<double, BinaryTree<double, string>>();

        foreach ( var code in alphabet )
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
