namespace KnapsackProblem;

public static class Knapsack
{
    public static int[][] Choose(List<int> values, List<int> sizes, int capacity)
    {
        var outcomes = new int[values.Count + 1][];
        for (int i = 0; i < outcomes.Length; i++)
        {
            outcomes[i] = new int[capacity + 1];
        }

        for (int i = 1; i < values.Count; i++)
        {
            for (int j = 0; j <= capacity; j++)
            {
                if (sizes[i - 1] > j)
                    outcomes[i][j] = outcomes[i - 1][j];
                else
                    outcomes[i][j] = Math.Max(outcomes[i - 1][j], outcomes[i - 1][j - sizes[i - 1]] + values[i - 1] );
            }
        }

        return outcomes;
    }
}
