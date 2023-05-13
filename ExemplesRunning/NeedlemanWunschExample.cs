using NeedlemanWunsch;

namespace ExamplesRunning;

internal class NeedlemanWunschExample
{
    internal static void Run()
    {
        var alphabet = new List<string>() { "A", "C", "G", "T" };

        //var sequence1 = SequenceGenerator<string>.Generate(alphabet, 5);
        //var sequence2 = SequenceGenerator<string>.Generate(alphabet, 6);

        var sequence1 = new List<string>() { "A", "G", "G", "G", "C", "T" };
        var sequence2 = new List<string>() { "A", "G", "G", "C", "A" };

        Viewer.ShowArray(sequence1.ToArray());
        Viewer.ShowArray(sequence2.ToArray());

        var result = ProteinSequenceAligning<string>.Align(sequence1, sequence2, 2, 1);

        Viewer.ShowMatrix(result);
    }
}
