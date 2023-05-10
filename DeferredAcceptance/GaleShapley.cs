namespace DeferredAcceptance;

// The Gale-Shapley algorithm, also known as the deferred acceptance algorithm, is a solution to the stable matching problem,
// which is a common problem in economics and game theory. It was proposed by David Gale and Lloyd S. Shapley in 1962.
// 
// Here's the problem it aims to solve: Suppose there are two sets of equal size,
// for example, men and women. Each person has a preference ranking for the individuals in the other set.
// The goal is to find a stable match, where no two individuals prefer each other to their current partners.
// 
// The algorithm works as follows:
// 
// 1. Each man proposes to the woman he prefers most.
// 2. Each woman, if proposed to, tentatively accepts the proposal from the man she prefers most and rejects all other proposals.
//    She may replace her current accepted proposal with a new proposal from a man she prefers more.
// 3. The men who were rejected in the previous step propose to their next most preferred woman who hasn't rejected them yet.
// 4. Steps 2 and 3 are repeated until every man has a partner.
//
// The resulting match is considered "stable" because there are no two people of opposite sex who would both rather have each other than their current partners.
// If there were, the match would not be stable because those two people would have an incentive to leave their current partners and pair up.
// 
// The stability provided by the Gale-Shapley algorithm is not necessarily an optimal solution for all individuals.
// For instance, the algorithm can be "gamed" by misrepresenting preferences.
// It's also worth noting that the algorithm favors the group that does the proposing (in the above example, the men).
// The proposers get their most preferred valid match, while the acceptors may end up with a less preferred match.

public static class GaleShapley
{
    public static int[] Match(int[][] menPreferences, int[][] womenPreferences)
    {
        int n = menPreferences.Length;

        // Initialize women partners array with -1 indicating that all women are initially free
        int[] womenPartners = new int[n];
        for (int i = 0; i < n; i++)
        {
            womenPartners[i] = -1;
        }

        // Initialize men engaged array with false indicating that all men are initially free
        bool[] menEngaged = new bool[n];

        // Count of free men
        int freeMenCount = n;

        while (freeMenCount > 0)
        {
            // Find the first free man
            int m;
            for (m = 0; m < n; m++)
                if (!menEngaged[m])
                    break;

            // For this man, go through his preference list until he gets engaged
            for (int i = 0; i < n && !menEngaged[m]; i++)
            {
                // Preferred woman of this man
                int w = menPreferences[m][i];

                // If the woman is free, engage them
                if (womenPartners[w] == -1)
                {
                    womenPartners[w] = m;
                    menEngaged[m] = true;
                    freeMenCount--;
                }
                else
                {
                    // The woman is currently engaged with
                    int currentPartner = womenPartners[w];

                    // If the woman prefers m over her current partner, engage woman with m
                    if (!PrefersCurrentPartnerOverNew(womenPreferences[w], m, currentPartner))
                    {
                        womenPartners[w] = m;
                        menEngaged[m] = true;
                        menEngaged[currentPartner] = false;
                    }
                }
            }
        }

        // Return the partners of women
        return womenPartners;
    }

    // Function to check if a woman prefers current partner over new man
    private static bool PrefersCurrentPartnerOverNew(int[] preferences, int @new, int current)
    {
        for (int i = 0; i < preferences.Length; i++)
        {
            // If current comes before new in preference list, she prefers current
            if (preferences[i] == current)
                return true;

            // If m comes before current in preference list, she prefers new
            if (preferences[i] == @new)
                return false;
        }

        return false;
    }
}
