namespace ResourceOptimization.Inventory;

/// <summary>
/// Provides a dynamic programming solver for the classical Wagner–Whitin
/// inventory lot-sizing problem specialized for a gas station.
/// </summary>
public static class GasStationLotSizing
{
    /// <summary>
    /// Describes a single replenishment order.
    /// </summary>
    /// <param name="Day">1-based index of the day when the order is placed.</param>
    /// <param name="Quantity">Amount ordered for that day.</param>
    public readonly record struct Order(int Day, int Quantity);

    /// <summary>
    /// Result of a lot-sizing computation.
    /// </summary>
    /// <param name="TotalCost">Minimum achievable cost.</param>
    /// <param name="Orders">Sequence of orders achieving <paramref name="TotalCost"/>.</param>
    public readonly record struct Plan(double TotalCost, IReadOnlyList<Order> Orders);

    /// <summary>
    /// Computes the optimal ordering plan for the given demand sequence using
    /// Wagner–Whitin dynamic programming.
    /// </summary>
    /// <param name="demand">Daily demand in gallons. Index 0 corresponds to day&nbsp;1.</param>
    /// <param name="fixedCost">Fixed cost paid whenever an order is placed.</param>
    /// <param name="unitCost">Per-gallon purchase cost.</param>
    /// <param name="holdingCost">Cost to store one gallon for one day.</param>
    /// <param name="capacity">Maximum gallons that can be stored at any time.</param>
    /// <returns>
    /// A <see cref="Plan"/> describing the minimum total cost and the schedule of orders.
    /// </returns>
    /// <remarks>Runs in O(n²) time.</remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the demand cannot be satisfied due to capacity limitations.
    /// </exception>
    public static Plan Solve(
        int[] demand,
        double fixedCost,
        double unitCost,
        double holdingCost,
        int capacity)
    {
        int n = demand.Length;
        if (n is 0)
            return new Plan(0, []);

        // Precompute prefix sums of demand and demand*index for O(1) cost queries.
        var prefix = new int[n + 1];
        var weighted = new int[n + 1];
        for (int i = 0; i < n; i++)
        {
            prefix[i + 1] = prefix[i] + demand[i];
            weighted[i + 1] = weighted[i] + demand[i] * i;
            if (demand[i] > capacity)
                throw new InvalidOperationException("Daily demand exceeds capacity.");
        }

        var dp = new double[n + 1];
        var next = new int[n + 1];
        dp[n] = 0;
        next[n] = -1;

        // Compute DP backwards.
        for (int i = n - 1; i >= 0; i--)
        {
            // If today's demand is zero we could “skip” without ordering,
            // otherwise force an order option by initializing to +∞.
            int todayDemand = prefix[i + 1] - prefix[i];
            double bestCost = todayDemand is 0
                            ? dp[i + 1]
                            : double.PositiveInfinity;
            int bestNext = i + 1;

            for (int j = i; j < n; j++)
            {
                int quantity = prefix[j + 1] - prefix[i];
                if (quantity > capacity)
                    break; // further j will only increase quantity

                if (quantity is 0)
                    continue; // ordering nothing is pointless

                int weightedSum = weighted[j + 1] - weighted[i];
                double holding = holdingCost * (weightedSum - i * (double)quantity);
                double purchase = unitCost * quantity;
                double cost = fixedCost + purchase + holding + dp[j + 1];

                if (cost < bestCost)
                {
                    bestCost = cost;
                    bestNext = j + 1;
                }
            }

            dp[i] = bestCost;
            next[i] = bestNext;
        }

        // Reconstruct the optimal ordering plan.
        var orders = new List<Order>();
        for (int day = 0; day < n;)
        {
            int nextDay = next[day];
            if (nextDay == day + 1 && prefix[day + 1] - prefix[day] is 0)
            {
                // No order, zero demand for this day.
                day++;
                continue;
            }

            int qty = prefix[nextDay] - prefix[day];
            orders.Add(new Order(day + 1, qty));
            day = nextDay;
        }

        return new Plan(dp[0], orders);
    }
}
