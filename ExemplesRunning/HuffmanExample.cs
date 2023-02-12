using Graphs.Generators;
using Graphs;
using Huffman;

namespace ExamplesRunning;
public static class HuffmanExample
{
    public static void Run()
    {
        //var alphabet = new Dictionary<string, double>()
        //{
        //    { "A", 0.32 },
        //    { "B", 0.25 },
        //    { "C", 0.2 },
        //    { "D", 0.18 },
        //    { "E", 0.05 }
        //};
        var alphabet = new Dictionary<string, double>()
        {
            { "A", 0.07 },
            { "B", 0.5 },
            { "C", 0.2 },
            { "D", 0.18 },
            { "E", 0.05 }
        };

        var codeGenerator = new HuffmanCodeGenerator();
        var tree = codeGenerator.Generate(alphabet);

        var graphGenerator = new GraphSearchTreeGenerator<double, string>(tree);
        var graph = graphGenerator.Generate("huffman_code_tree");
        DOTVisualizer.VisualizeGraph(graph);
    }
}
