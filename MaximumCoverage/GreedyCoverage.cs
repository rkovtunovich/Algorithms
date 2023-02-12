namespace MaximumCoverage;

public static class GreedyCoverage
{
    public static List<int> GetCoverage(List<List<int>> sets, int budget)
    {
        var coverage = new List<int>();
        var uniqueItems = new HashSet<int>();

        for (int i = 1; i <= budget; i++)
        {
            var index = GetNextBestSetIndex(sets, uniqueItems);
            coverage.Add(index);
        }

        return coverage;
    }

    private static int GetNextBestSetIndex(List<List<int>> sets, HashSet<int> uniqueItems)
    {
        int max = 0;
        int bestSetIndex = -1; 

        for (int i = 0; i < sets.Count; i++)
        {
            int length = sets[i].Count;

            for (int j = length - 1; j >= 0; j--)
            {
                if (uniqueItems.Contains(sets[i][j]))
                    sets[i].RemoveAt(j);
            }

            if(sets[i].Count > max)
            {
                max = sets[i].Count;
                bestSetIndex = i;
            }
        }

        if (bestSetIndex is not -1)
            uniqueItems.UnionWith(sets[bestSetIndex]);

        return bestSetIndex;
    }
}
