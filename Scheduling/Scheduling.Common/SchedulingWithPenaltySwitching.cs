namespace Scheduling.Common;

public class SchedulingWithPenaltySwitching
{
    /// <summary>
    /// Scheduling with switching between two states (e.g., machines, tasks, modes) and incurring a delay or penalty when switching.
    /// Returns the maximum obtainable value and a dictionary of switches.
    /// </summary>
    /// <param name="stateA">An array representing the revenue (or steps) from state A in each time step.</param>
    /// <param name="stateB">An array representing the revenue (or steps) from state B in each time step.</param>
    /// <param name="penaltyAtoB">The penalty how many steps it takes to switch from state A to state B.</param>
    /// <param name="penaltyBtoA">The penalty how many steps it takes to switch from state B to state A.</param>
    /// <returns>
    /// A tuple containing the maximum obtainable value and a dictionary of switching steps.
    /// The dictionary maps each step to a tuple (from, to) where 1 is A and 2 is B.
    /// </returns>
    public static (int MaxValue, Dictionary<int, (int from, int to)> Switches) Solve(int[] stateA, int[] stateB, int penaltyAtoB, int penaltyBtoA)
    {
        int n = stateA.Length;
        if (n is 0)
            return (0, new Dictionary<int, (int, int)>());

        var dpA = new int[n + 1];
        var dpB = new int[n + 1];
        var prevA = new int[n + 1]; // 0: initial, 1: from A, 2: from B
        var prevB = new int[n + 1];

        for (int i = 0; i <= n; i++)
        {
            dpA[i] = int.MinValue;
            dpB[i] = int.MinValue;
            prevA[i] = 0;
            prevB[i] = 0;
        }
        dpA[0] = 0;
        dpB[0] = 0;

        for (int i = 1; i <= n; i++)
        {
            // Stay in A
            if (dpA[i - 1] is not int.MinValue)
            {
                int value = dpA[i - 1] + stateA[i - 1];
                if (dpA[i] < value)
                {
                    dpA[i] = value;
                    prevA[i] = 1; // came from A
                }
            }

            // Switch from B to A
            if (i - penaltyBtoA >= 0 && dpB[i - penaltyBtoA] != int.MinValue)
            {
                int value = dpB[i - penaltyBtoA]; // No revenue during penalty
                if (dpA[i] < value)
                {
                    dpA[i] = value;
                    prevA[i] = 2; // came from B via switch
                }
            }

            // Stay in B
            if (dpB[i - 1] is not int.MinValue)
            {
                int value = dpB[i - 1] + stateB[i - 1];
                if (dpB[i] < value)
                {
                    dpB[i] = value;
                    prevB[i] = 2; // came from B
                }
            }

            // Switch from A to B
            if (i - penaltyAtoB >= 0 && dpA[i - penaltyAtoB] is not int.MinValue)
            {
                int value = dpA[i - penaltyAtoB]; // No revenue during penalty
                if (dpB[i] < value)
                {
                    dpB[i] = value;
                    prevB[i] = 1; // came from A via switch
                }
            }
        }

        // The maximum value is the best value we can obtain in the last step, either in state A or state B
        int maxValue = Math.Max(dpA[n], dpB[n]);

        // Backtracking to get the full decision history
        var switches = BacktrackSwitches(penaltyAtoB, penaltyBtoA, n, dpA, dpB, prevA, prevB);

        return (maxValue, switches);
    }

    private static Dictionary<int, (int from, int to)> BacktrackSwitches(int penaltyAtoB, int penaltyBtoA, int n, int[] dpA, int[] dpB, int[] prevA, int[] prevB)
    {
        int currentStep = n;
        int currentState = dpA[n] >= dpB[n] ? 1 : 2; // 1 = A, 2 = B

        var switches = new Dictionary<int, (int from, int to)>();

        while (currentStep > 0)
        {
            int prevState;
            if (currentState is 1)
                prevState = prevA[currentStep];
            else
                prevState = prevB[currentStep];

            if (prevState is 0)
            {
                // Reached the beginning
                break;
            }

            if (prevState != currentState)
            {
                // There was a switch
                int penalty = (currentState == 1) ? penaltyBtoA : penaltyAtoB;

                int switchTime = currentStep - penalty + 1;
                if (switchTime >= 1)
                    switches[switchTime] = (prevState, currentState);

                // Move back penalty steps
                currentStep -= penalty;
            }
            else
            {
                // No switch, move back one step
                currentStep--;
            }

            currentState = prevState;
        }

        return switches;
    }
}
