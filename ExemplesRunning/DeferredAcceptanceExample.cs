using DeferredAcceptance;

namespace ExamplesRunning;

internal class DeferredAcceptanceExample
{
    internal static void Run()
    {
        // Define preferences for men and women
        int[][] menPreferences = new int[][] {
            new int[] {1, 0, 3, 2},
            new int[] {2, 3, 0, 1},
            new int[] {1, 0, 2, 3},
            new int[] {0, 3, 1, 2}
        };

        int[][] womenPreferences = new int[][] {
            new int[] {0, 1, 3, 2},
            new int[] {2, 0, 1, 3},
            new int[] {1, 2, 3, 0},
            new int[] {3, 2, 0, 1}
        };

        // Run the Gale-Shapley algorithm
        int[] partners = GaleShapley.Match(menPreferences, womenPreferences);

        // Print the matching
        for (int i = 0; i < partners.Length; i++)
        {
            Console.WriteLine("Woman {0} is matched with man {1}", i, partners[i]);
        }
    }
}
