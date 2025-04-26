namespace ResourceOptimization.Allocation;

// The generalized problem can be formalized as follows:
// 1. Resource Arrival: at each time step t, a certain number of "opportunities" (e.g. jobs, resources, tasks, items, etc.) arrive.
// 2. Resource Action: we have a tool or mechanism that can act on these opportunities, 
//    but its effectiveness depends on how long we've waited since its last action.
// 3. Objective: we want to maximize the cumulative total "value" by deciding when to act,
//    balancing the trade-off between waiting for greater efficiency and acting sooner to capture more opportunities.
// 
// Key components:
// 1. Time steps: at each time step t, a certain number of opportunities xt arrive.
// 2. Resource power function: a function f(j) defines how effective the resource is after j time steps since its last action.
// 3. Decision points: at each time step t, we decide whether to act on the opportunities or wait for a better opportunity.
// 4. Objective function: we want to maximize the cumulative total value of the opportunities we capture.

public class SchedulingWithDelaysProblem
{
    public static (double Value, List<int> ActionsSequence) Solve(int[] arrivals, Func<int, double> powerFunction)
    {
        var dp = new double[arrivals.Length + 1]; // to store the optimal value at each time step
        var tracking = new int[arrivals.Length + 1]; // to track the optimal decision at each time step

        for (int i = 1; i <= arrivals.Length; i++)
        {
            // the value of not acting on the opportunities
            dp[i] = dp[i - 1];

            // consider all possible decisions at each time step
            for (int j = 1; j <= i; j++)
            {
                // choose the maximum value between the current value and the value of acting on the opportunities
                var actingValue = dp[i - j] + Math.Min(powerFunction(j), arrivals[i - 1]);
                dp[i] = Math.Max(dp[i], actingValue);

                // track the optimal decision at each time step
                if (dp[i] == actingValue)
                    tracking[i] = j;
            }
        }

        // reconstruct the optimal action sequence
        var actionSequence = Enumerable.Repeat(0, arrivals.Length).ToList();

        // start from the last time step and backtrack to the first time step
        for (int i = arrivals.Length, j = 0; i > 0; i -= tracking[i], j++)
            actionSequence[j] = tracking[i];

        // reverse the action sequence to get the correct order
        actionSequence.Reverse();

        return (dp[arrivals.Length], actionSequence);
    }
}
