namespace Scheduling.Common;

public class FreightScheduling
{
    /// <summary>
    /// Determines the minimum cost schedule to ship supplies over n steps.
    /// - Company A charges a per-pound rate each step.
    /// - Company B charges a flat rate each term of m steps.
    /// </summary>
    /// <param name="supplies">Array of supplies to ship each step.</param>
    /// <param name="ratePerUnit">Cost per unit for Company A.</param>
    /// <param name="flatCost">Flat cost for Company B.</param>
    /// <param name="minimumTerm">Minimum term for Company B (default is 4).</param>
    /// <returns>Tuple of minimum cost and schedule (list of "A" or "B" representing the chosen company each step).</returns>
    public static (int MinCost, List<string> Schedule) MinShippingCost(int[] supplies, int ratePerUnit, int flatCost, int minimumTerm = 4)
    {
        var n = supplies.Length;
        var dp = new int[n + 1];
        var tracking = new string[n + 1]; // Tracks the choice for step i   

        // Initialize DP for the first step
        dp[0] = 0; // No cost at the beginning

        for (int i = 1; i <= n; i++)
        {
            // Calculate cost if we choose company A for the current step
            dp[i] = dp[i - 1] + ratePerUnit * supplies[i - 1];
            tracking[i] = "A";

            // Skip if we haven't reached the minimum term for company B
            if (i < minimumTerm)
                continue;

            var termCost = dp[i - minimumTerm];
            for (int j = 1; j <= minimumTerm; j++)
            {
                termCost += flatCost;

                if (termCost > dp[i - minimumTerm + j])
                {
                    termCost = int.MaxValue;
                    break;
                }

            }

            // Calculate cost if we choose company B for the current step
            if (termCost > dp[i])
                continue;

            // change the cost and tracking for the whole term
            for (int j = 1; j <= minimumTerm; j++)
            {
                var currentStep = i - minimumTerm + j;
                dp[currentStep] = dp[currentStep - 1] + flatCost;
                tracking[currentStep] = "B";
            }
        }

        return (dp[n], tracking.Skip(1).ToList());
    }
}
