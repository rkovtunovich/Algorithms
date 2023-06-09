namespace DataStructures.BinaryTrees.Search;

public class OptimalBinarySearchTree<TKey, TValue> : BinaryTree<TKey, TValue> where TKey : INumber<TKey>
{
    public double[][] Span { get; set; }

    public OptimalBinarySearchTree(TKey[] keys, TValue[] values, double[] frequencies)
    {
        Build(keys, values, frequencies);
    }

    private void Build(TKey[] keys, TValue[] values, double[] frequencies)
    {
        Span = new double[keys.Length + 1][];

        for (int i = 0; i <= keys.Length; i++)
        {
            Span[i] = new double[keys.Length + 1];
        }

        for (int size = 0; size < keys.Length; size++)
        {
            for (int i = 1; i <= keys.Length - size; i++)
            {
                Span[i][i + size] = GetCurrentFrequency(frequencies, size, i) + GetCurrentMinimum(Span, size, i);
            }
        }
    }

    private double GetCurrentFrequency(double[] frequencies, int size, int i)
    {
        double frequency = 0;

        for (int j = i - 1; j < size + i; j++)
        {
            frequency += frequencies[j];
        }

        return frequency;
    }

    private double GetCurrentMinimum(double[][] span, int size, int i)
    {
        double minimum = 0;

        for (int j = i; j < size + i; j++)
        {
            var curr = span[i][j - 1] + span[j + 1][i + size];

            if(minimum is 0 || curr < minimum)
                minimum = curr;
        }

        return minimum;
    }
}

