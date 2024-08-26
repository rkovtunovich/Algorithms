namespace Biology.Common;

// Needleman-Wunsch algorithm
public static class ProteinSequenceAligning<T> where T : notnull
{
    public static double[][] Align(List<T> sequence1, List<T> sequence2, double symbolPenalty, double gapPenalty)
    {
        var alignedSequence = new List<T>();

        var span = new double[sequence1.Count + 1][];

        for (int i = 0; i <= sequence1.Count; i++)
        {
            span[i] = new double[sequence2.Count + 1];
            span[i][0] = i * gapPenalty;
        }

        for (int i = 0; i <= sequence2.Count; i++)
        {
            span[0][i] = i * gapPenalty;
        }

        var cases = new List<double>(3);

        for (int i = 1; i <= sequence1.Count; i++)
        {
            for (int j = 1; j <= sequence2.Count; j++)
            {
                var currentSymbolPenalty = sequence1[i - 1].Equals(sequence2[j - 1]) ? 0 : symbolPenalty;

                cases.Add(span[i - 1][j - 1] + currentSymbolPenalty);
                cases.Add(span[i - 1][j] + gapPenalty);
                cases.Add(span[i][j - 1] + gapPenalty);

                span[i][j] = cases.Min();

                cases.Clear();
            }
        }

        return span;
    }
}
