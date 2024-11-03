namespace Scheduling.Common.Allocation;

public static class ResourcePlacement
{
    /// <summary>
    /// Optimizes resource placement across a sequence of nodes to minimize placement and access costs.
    /// </summary>
    /// <param name="numNodes">The total number of nodes.</param>
    /// <param name="placementCost">A function that returns the placement cost for a given node.</param>
    /// <param name="accessCost">A function that returns the access cost from one node to another. Access has forward direction only.</param>
    /// <param name="preferMorePlacements">A flag to prefer more placements in case of equal costs.</param>
    /// <returns>A tuple containing the minimum total cost and a list of nodes where the resource is placed.</returns>
    public static (double MinCost, List<int> PlacementNodes) Optimize(int numNodes, Func<int, double> placementCost, Func<int, int, double> accessCost, bool preferMorePlacements = false)
    {
        // Initialize DP table and placement track
        var dp = new double[numNodes + 1];
        var placementTrack = new int[numNodes + 1];

        // Base case
        dp[0] = 0;

        // Compute DP table
        for (int i = 1; i <= numNodes; i++)
        {
            // Initialize cost to maximum value
            dp[i] = double.MaxValue;

            // Compute cost for each placement
            for (int k = 1; k <= i; k++)
            {
                // Compute cost for placement k
                double cost = dp[k - 1] + placementCost(k);
                for (int j = k + 1; j <= i; j++)               
                    cost += accessCost(k, j);

                // Update DP table if cost is less
                if (cost < dp[i])
                {
                    dp[i] = cost;

                    placementTrack[i] = k;

                    // Update placementTrack to prefer more placements in case of equal costs
                    if (preferMorePlacements && cost == dp[i - 1] + placementCost(k))
                        placementTrack[i] = i; 
                }
            }
        }

        // Compute placement nodes
        var placementNodes = new List<int>();
        int index = numNodes;
        while (index > 0)
        {
            int placementNode = placementTrack[index];
            placementNodes.Add(placementNode);
           
            index = placementNode - 1;
        }

        placementNodes.Reverse();
        
        return (dp[numNodes], placementNodes);
    }
}
