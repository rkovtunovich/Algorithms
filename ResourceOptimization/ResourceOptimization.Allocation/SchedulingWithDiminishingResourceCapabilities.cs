namespace ResourceOptimization.Allocation;

// The generalized problem can be formalized as follows:
// 1. Resource Arrival: At each time step t, a certain number of "opportunities" (e.g. jobs, resources, tasks, data, etc.) arrive. Resources don't accumulate.
// 2. Resource Diminishment: We have a tool or mechanism with diminishing capabilities that can act on these opportunities.
// 3. Rebooting: The tool can "reboot" to restore its capabilities, but it takes time to do so.
// 4. Objective: Maximize the cumulative total "value" by deciding when to act.

// Key components:
// 1. Time steps: At each time step t, a certain number of opportunities x(t) arrive.
// 2. Resource power s(t): The tool's diminishing capabilities at time step t.
// 3. Reboot time r: The time it takes to reboot the tool, resetting its power to s(1).
// 4. Decision points: At each time step t, decide whether to act on the opportunities or wait for a better opportunity.
// 5. Objective function: Maximize the cumulative total value of the opportunities we capture.

public class SchedulingWithDiminishingResourceCapabilities
{
    // Main method to solve the scheduling problem.
    // Takes an array of arrivals, a power function (how much the tool can process at any given time), and reboot time.
    public static (double Value, List<int> Reboots) Solve(int[] arrivals, Func<int, double> powerFunction, int rebootTime)
    {
        // Handle the special case when reboot time is zero, as it simplifies the problem significantly.
        if (rebootTime is 0)
            return HandleWhenZeroRebootTime(arrivals, powerFunction);

        var n = arrivals.Length;
        var dp = new Dictionary<int, double>[n + 1]; // dp[i][t] stores the max value at time i with state t (time since the last reboot).
        var tracking = new int[n + 1]; // Keeps track of the optimal decision at each time step.

        // Initialization: at time 0, the tool is just rebooted and hasn't processed anything.
        dp[0] = new Dictionary<int, double> { { 0, 0 } }; // At t = 0, the tool is in a "fresh" reboot state and has processed nothing.

        // Iterating over each time step.
        for (int i = 1; i <= n; i++)
        {
            dp[i] = []; // Initialize the dictionary for time i.

            foreach (var entry in dp[i - 1])
            {
                var t = entry.Key; // The state of the tool (how long since the last reboot).
                var value = entry.Value; // The cumulative value up until time i-1.

                if (t >= 0) // Case where the tool is available and can process.
                {
                    // Time since the last reboot increases by 1.
                    var timeSinceReboot = t + 1;
                    // Get the power of the tool at this step.
                    var power = powerFunction(timeSinceReboot);

                    // Option 1: Continue processing at time step i without rebooting.
                    var newValue = value + Math.Min(power, arrivals[i - 1]);

                    // Store the value if it's greater than the current value at this state.
                    if (!dp[i].TryGetValue(timeSinceReboot, out double currentValue))
                        dp[i][timeSinceReboot] = newValue;
                    else
                        dp[i][timeSinceReboot] = Math.Max(currentValue, newValue);

                    // Option 2: Reboot the tool.
                    // Rebooting puts the tool in a rebooting state (-r time steps).
                    var rebootState = rebootTime == 0 ? 0 : -rebootTime + 1;
                    if (!dp[i].TryGetValue(rebootState, out double rebootValue))
                        dp[i][rebootState] = value;
                    else
                        dp[i][rebootState] = Math.Max(rebootValue, value);
                }
                else
                {
                    // Case where the tool is rebooting.
                    // Simply transition to the next state in rebooting.
                    var nextState = t + 1;

                    if (!dp[i].TryGetValue(nextState, out double currentValue))
                        dp[i][nextState] = value;
                    else
                        dp[i][nextState] = Math.Max(currentValue, value);
                }
            }
        }

        // Now we need to reconstruct the sequence of reboots and calculate the final value.
        var reboots = new List<int>(); // List to hold the time steps when reboots occur.
        var max = dp[n].Values.Max(); // Find the maximum value processed.
        var maxValue = max; // Store the maximum value as the result.

        // Backtrack to find when the reboots occurred.
        while (n > 0)
        {
            var step = dp[n]; // Get the dp state at time step n.
            var optimal = step.Where(x => x.Value == max).FirstOrDefault(); // Find the optimal state leading to the max value.
            var beforeReboot = optimal.Key; // Get the state (time since reboot).

            if (beforeReboot is 0)
            {
                // No reboot needed at this step, so move to the previous time step.
                n--;
                continue;
            }

            // Track back over the time since the last reboot.
            while (beforeReboot > 0)
            {
                // Deduct the processing done during this state.
                max -= powerFunction(beforeReboot);

                n--; // Move back to the previous time step.
                beforeReboot--; // Decrement the time since the last reboot.

                if (beforeReboot is 0 && n > 0)
                {
                    // If we reach a reboot point, track it and move further back by the reboot time.
                    reboots.Add(n);
                    n -= rebootTime; // Skip over the reboot period.
                }
            }
        }

        reboots.Reverse(); // Reboots were tracked in reverse, so we reverse them to get the correct order.

        return (maxValue, reboots); // Return the final value and the list of reboots.
    }

    // Special handling for the case when reboot time is 0 (i.e., the tool never needs to reboot).
    private static (double Value, List<int> Reboots) HandleWhenZeroRebootTime(int[] arrivals, Func<int, double> powerFunction)
    {
        // If reboot time is 0, the tool is always available, so we treat it as a regular scheduling problem.
        var (Value, ActionsSequence) = SchedulingWithDelaysProblem.Solve(arrivals, powerFunction);

        var reboots = new List<int>(arrivals.Length);
        for (int i = 1; i <= arrivals.Length; i++)
            reboots.Add(i); // Reboot at each time step (although it's unnecessary in this case).

        return (Value, reboots);
    }
}
